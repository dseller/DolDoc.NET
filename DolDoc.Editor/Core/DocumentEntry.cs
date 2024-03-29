﻿using DolDoc.Editor.Commands;
using DolDoc.Editor.Entries;
using DolDoc.Editor.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DolDoc.Editor.Core
{
    /// <summary>
    /// Base class for all document entries.
    /// </summary>
    public abstract class DocumentEntry
    {
        /// <summary>
        /// Gets the collection of flags for this entry.
        /// </summary>
        public IList<Flag> Flags { get; }

        /// <summary>
        /// Gets the collection of argumens for this entry.
        /// </summary>
        public IList<Argument> Arguments { get; }

        public string Tag => Arguments.FirstOrDefault(arg => arg.Key == "T" || arg.Key == null)?.Value;

        public string Aux => GetArgument("A");

        protected DocumentEntry(IList<Flag> flags, IList<Argument> args)
        {
            Flags = flags;
            Arguments = args;
        }

        public bool HasFlag(string flag, bool status = true) => Flags.Any(f => f.Value == flag && f.Status == status);

        public virtual void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            switch (key)
            {
                case Key.DEL:
                    state.Document.Remove(this);
                    break;
            }
        }

        public bool Selected { get; set; }

        public abstract CommandResult Evaluate(EntryRenderContext ctx);

        public abstract override string ToString();

        public virtual bool Clickable => false;

        public virtual bool IsInput => false;

        public virtual void Click(ViewerState state)
        {
            // Do nothing atm.
        }

        public string GetArgument(string key, string defaultValue = null) => Arguments.FirstOrDefault(arg => arg.Key == key)?.Value ?? defaultValue;

        public void SetArgument(string key, string value)
        {
            if (!HasArgument(key))
                Arguments.Add(new Argument(key, value));
            else
                Arguments.First(arg => arg.Key == key).Value = value;
        }

        public bool HasArgument(string key) => Arguments.Any(arg => arg.Key == key);

        protected void WriteBorder(EntryRenderContext ctx, int length, int? renderPositionOverride = null)
        {
            // TODO: add support for multiline borders!
            var renderPosition = renderPositionOverride ?? ctx.RenderPosition;
            var color = new CombinedColor(ctx.Options.BackgroundColor, ctx.Options.ForegroundColor);

            // Write top border
            ctx.State.Pages[renderPosition - ctx.State.Columns - 1].Write(this, 0, Codepage437.SingleTopLeftCorner, color);
            for (var i = 0; i < length; i++)
                ctx.State.Pages[renderPosition - ctx.State.Columns + i].Write(this, 0, Codepage437.SingleHorizontalLine, color);
            ctx.State.Pages[renderPosition - ctx.State.Columns + length].Write(this, 0, Codepage437.SingleTopRightCorner, color);

            // Write bottom border
            ctx.State.Pages[renderPosition + ctx.State.Columns - 1].Write(this, 0, Codepage437.SingleBottomLeftCorner, color);
            for (var i = 0; i < length; i++)
                ctx.State.Pages[renderPosition + ctx.State.Columns + i].Write(this, 0, Codepage437.SingleHorizontalLine, color);
            ctx.State.Pages[renderPosition + ctx.State.Columns + length].Write(this, 0, Codepage437.SingleBottomRightCorner, color);

            // Write left border
            ctx.State.Pages[renderPosition - 1].Write(this, 0, Codepage437.SingleVerticalLine, color);

            // Write right border
            ctx.State.Pages[renderPosition + length].Write(this, 0, Codepage437.SingleVerticalLine, color);
        }

        /// <summary>
        /// Write a string with the context information provided.
        /// </summary>
        /// <param name="ctx">The context</param>
        /// <param name="str">The string to write</param>
        /// <returns>The amount of <seealso cref="Character"/>s written.</returns>
        protected int WriteString(EntryRenderContext ctx, string str)
        {
            if (ctx.Options.CollapsedTreeNodeIndentationLevel.HasValue && ctx.Options.Indentation > ctx.Options.CollapsedTreeNodeIndentationLevel.Value)
                return 0;

            var startRenderPosition = CalculateStartRenderPositionAndCharsWritten(ctx, str, out var charsWritten);
            var renderPosition = startRenderPosition;

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
                    for (int indent = 0; indent < ctx.Options.Indentation; indent++)
                        ctx.State.Pages[renderPosition + indent].Write(this, indent, (byte)' ', new CombinedColor(ctx.Options.BackgroundColor, ctx.Options.ForegroundColor));

                    renderPosition += ctx.Options.Indentation;
                    charsWritten += ctx.Options.Indentation;
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
                if (ctx.Options.Underline)
                    chFlags |= CharacterFlags.Underline;
                if (ctx.Options.Blink)
                    chFlags |= CharacterFlags.Blink;
                if (ctx.Options.Inverted)
                    chFlags |= CharacterFlags.Inverted;
                if (HasFlag("H"))
                    chFlags |= CharacterFlags.Hold;

                /*byte shiftX = 0, shiftY = 0;
                if (Arguments.Any(arg => arg.Key == "SX"))
                    shiftX = byte.Parse(Arguments.First(arg => arg.Key == "SX").Value);*/

                ctx.State.Pages[renderPosition++].Write(
                    this,
                    i,
                    (byte)str[i],
                    new CombinedColor(ctx.Options.BackgroundColor, ctx.Options.ForegroundColor),
                    chFlags,
                    0,
                    sbyte.Parse(GetArgument("SX", "0")),
                    sbyte.Parse(GetArgument("SY", "0"))
                );
            }

            if (HasFlag("B", true))
                WriteBorder(ctx, str.Length, startRenderPosition);

            return charsWritten;
        }

        private int CalculateStartRenderPositionAndCharsWritten(EntryRenderContext ctx, string str, out int charsWritten)
        {
            var offset = 0;
            var renderPosition = ctx.RenderPosition;

            if (HasFlag("CX"))
                renderPosition = (renderPosition - (renderPosition % ctx.State.Columns)) + ((ctx.State.Columns / 2) - (str.Length / 2));
            else if (HasFlag("RX"))
            {
                offset = HasFlag("B", true) ? 3 : 2;
                renderPosition = (renderPosition - (renderPosition % ctx.State.Columns)) + (ctx.State.Columns - str.Length - offset);
            }
            else if (HasFlag("LX"))
            {
                offset = HasFlag("B", true) ? 1 : 0;
                renderPosition = (renderPosition - (renderPosition % ctx.State.Columns) + offset);
            }

            charsWritten = renderPosition - ctx.RenderPosition + offset;
            return renderPosition;
        }

        protected string AsString(string cmd)
        {
            var builder = new StringBuilder();
            builder.Append($"${cmd}");

            foreach (var flag in Flags)
                builder.Append($"{(flag.Status ? "+" : "-")}{flag.Value}");

            foreach (var arg in Arguments)
            {
                if (arg.Key == null)
                    builder.Append($",\"{arg.Value}\"");
                else
                    builder.Append($",{arg.Key}=\"{arg.Value}\"");
            }

            builder.Append('$');
            return builder.ToString();
        }
    }
}
