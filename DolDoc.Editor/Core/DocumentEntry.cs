using DolDoc.Editor.Commands;
using DolDoc.Editor.Entries;
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
                charsWritten = ctx.State.Columns - (renderPosition - (renderPosition % ctx.State.Columns));
            }

            for (var i = 0; i < str.Length; i++)
            {
                char ch = str[i];

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
                    ctx.State.Pages[renderPosition] =
                       new Character(this, i, (byte)' ', new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);

                    var charsUntilEndOfLine = ctx.State.Columns - (renderPosition % ctx.State.Columns);
                    for (int i2 = 1; i2 < charsUntilEndOfLine; i2++)
                        ctx.State.Pages[renderPosition + i2] = new Character(null, 0, 0x00, new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);

                    renderPosition += charsUntilEndOfLine;
                    charsWritten += charsUntilEndOfLine;
                    continue;
                }
                else if (ch == '\t')
                {
                    var charsUntilMultipleOf5 = 5 - (renderPosition % 5);
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
