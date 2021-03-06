﻿using DolDoc.Editor.Commands;
using DolDoc.Editor.Entries;
using DolDoc.Editor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DolDoc.Editor.Core
{
    public abstract class DocumentEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public IList<Flag> Flags { get; }

        public IList<Argument> Arguments { get; }

        public string Tag
        {
            get => Arguments.FirstOrDefault(arg => arg.Key == "T" || arg.Key == null)?.Value;
            set => Arguments.FirstOrDefault(arg => arg.Key == "T" || arg.Key == null).Value = value;
        }

        public DocumentEntry(IList<Flag> flags, IList<Argument> args)
        {
            Flags = flags;
            Arguments = args;
        }

        public static DocumentEntry CreateTextCommand(IList<Flag> flags, string value) =>
            new Text(flags, new[] { new Argument(null, value) });

        public bool HasFlag(string flag, bool status = true) => Flags.Any(f => f.Value == flag && f.Status == status);

        public virtual void CharKeyPress(ViewerState state, char key, int relativeOffset)
        {
            Console.WriteLine("CharKeyPress on entry not supporting input!");
        }

        public virtual void KeyPress(ViewerState state, ConsoleKey key, int relativeOffset)
        {
            switch (key)
            {
                case ConsoleKey.Delete:
                    state.Document.Entries.Remove(this);
                    break;
            }
        }

        public abstract CommandResult Evaluate(EntryRenderContext ctx);

        public abstract override string ToString();

        public virtual bool Clickable => false;

        public virtual void Click()
        {
            // Do nothing atm.
        }

        protected string GetArgument(string key) => Arguments.FirstOrDefault(arg => arg.Key == key)?.Value;

        protected void WriteBorder(EntryRenderContext ctx, int length)
        {
            // TODO: add support for multiline borders!

            // Write top border
            ctx.State.Pages[ctx.RenderPosition - ctx.State.Columns - 1] = new Character(this, 0, 0xDA, new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);
            for (int i = 0; i < length; i++)
                ctx.State.Pages[ctx.RenderPosition - ctx.State.Columns + i] = new Character(this, 0, 0xC4, new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);
            ctx.State.Pages[ctx.RenderPosition - ctx.State.Columns + length] = new Character(this, 0, 0xBF, new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);

            // Write bottom border
            ctx.State.Pages[ctx.RenderPosition + ctx.State.Columns - 1] = new Character(this, 0, 0xC0, new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);
            for (int i = 0; i < length; i++)
                ctx.State.Pages[ctx.RenderPosition + ctx.State.Columns + i] = new Character(this, 0, 0xC4, new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);
            ctx.State.Pages[ctx.RenderPosition + ctx.State.Columns + length] = new Character(this, 0, 0xD9, new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);

            // Write left border
            ctx.State.Pages[ctx.RenderPosition - 1] = new Character(this, 0, 0xB3, new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);

            // Write right border
            ctx.State.Pages[ctx.RenderPosition + length] = new Character(this, 0, 0xB3, new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);
        }

        /// <summary>
        /// Write a string with the context information provided.
        /// </summary>
        /// <param name="ctx">The context</param>
        /// <param name="str">The string to write</param>
        /// <returns>The amount of <seealso cref="Character"/>s written.</returns>
        protected int WriteString(EntryRenderContext ctx, string str)
        {
            if (ctx.CollapsedTreeNodeIndentationLevel.HasValue && ctx.Indentation > ctx.CollapsedTreeNodeIndentationLevel.Value)
                return 0;

            int charsWritten = 0, renderPosition = ctx.RenderPosition;
            if (HasFlag("CX"))
            {
                renderPosition = (renderPosition - (renderPosition % ctx.State.Columns)) + ((ctx.State.Columns / 2) - (str.Length / 2));
                charsWritten = renderPosition - ctx.RenderPosition;
            }

            for (var i = 0; i < str.Length; i++)
            {
                char ch = str[i];

                // Wordwrap: see if the current word fits in the remainder of the line. If not, move the 
                // render position to the new line.
                if (!char.IsWhiteSpace(ch))
                {
                    var wsIndex = str.IndexOfWhitespace(i);

                    int remainder = ctx.State.Columns - (renderPosition % ctx.State.Columns);
                    if (wsIndex.HasValue/* && (((wsIndex.Value + renderPosition) % ctx.State.Columns) - ctx.State.Columns) > remainder*/)
                    {
                        int remainderWord = wsIndex.Value - i;
                        if ((renderPosition % ctx.State.Columns) + remainderWord >= ctx.State.Columns)
                        {
                            renderPosition += remainder;
                            charsWritten += remainder;
                        }
                    }
                }

                // Check indentation.
                if (renderPosition % ctx.State.Columns == 0)
                {
                    for (int indent = 0; indent < ctx.Indentation; indent++)
                        ctx.State.Pages[renderPosition + indent] = new Character(this, indent, (byte)' ', new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);

                    renderPosition += ctx.Indentation;
                    charsWritten += ctx.Indentation;
                }

                if (ch == '\n')
                {
                    var charsUntilEndOfLine = ctx.State.Columns - (renderPosition % ctx.State.Columns);
                    renderPosition += charsUntilEndOfLine;
                    charsWritten += charsUntilEndOfLine;
                    continue;
                }
                else if (ch == '\t')
                {
                    var charsUntilMultipleOf5 = 8 - (renderPosition % 8);
                    renderPosition += charsUntilMultipleOf5;
                    charsWritten += charsUntilMultipleOf5;
                    continue;
                }
                else
                    charsWritten++;

                var chFlags = CharacterFlags.None;
                if (ctx.Underline)
                    chFlags |= CharacterFlags.Underline;
                if (ctx.Blink)
                    chFlags |= CharacterFlags.Blink;
                if (ctx.Inverted)
                    chFlags |= CharacterFlags.Inverted;
                if (HasFlag("H"))
                    chFlags |= CharacterFlags.Hold;

                byte shiftX = 0, shiftY = 0;
                /*if (Arguments.Any(arg => arg.Key == "SX"))
                    shiftX = byte.Parse(Arguments.First(arg => arg.Key == "SX").Value);*/

                ctx.State.Pages[renderPosition++] = new Character(
                    this,
                    i,
                    (byte)str[i],
                    new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor),
                    chFlags
                );
            }

            return charsWritten;
        }
    }
}
