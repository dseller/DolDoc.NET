using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DolDoc.Editor.Entries
{
    public class Text : DocumentEntry
    {
        public Text(int textOffset, IList<Flag> flags, IList<Argument> args) 
            : base(textOffset, flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            // TODO:
            //else if ((data.Flags & CharacterFlags.Right) == CharacterFlags.Right)
            //    CursorX = Columns - data.Data.Length;

            return new CommandResult(true, WriteString(ctx, Tag));
        }

        protected int WriteString(EntryRenderContext ctx, string str)
        {
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
                        ctx.State.Pages[indent] = new Character(this, 0, (byte)' ', new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);

                    renderPosition += ctx.Indentation;
                    charsWritten += ctx.Indentation;
                }

                if (ch == '\n')
                {
                    ctx.State.Pages[renderPosition] =
                       new Character(this, i, (byte)' ', new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);

                    var charsUntilEndOfLine = ctx.State.Columns - (renderPosition % ctx.State.Columns);
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
                if (Arguments.Any(arg => arg.Key == "SX"))
                    shiftX = byte.Parse(Arguments.First(arg => arg.Key == "SX").Value);

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

        public override string ToString()
        {
            return $"$TX,\"{Tag}\"$";
        }
    }
}
