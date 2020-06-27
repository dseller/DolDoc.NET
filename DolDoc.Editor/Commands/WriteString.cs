using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class WriteString : IDolDocCommand
    {
        public virtual CommandResult Execute(CommandContext ctx)
        {
            // TODO:
            //else if ((data.Flags & CharacterFlags.Right) == CharacterFlags.Right)
            //    CursorX = Columns - data.Data.Length;

            var str = ctx.Arguments[0].Value;
            int charsWritten = 0, renderPosition = ctx.RenderPosition;
            if (ctx.HasFlag("CX"))
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
                        ctx.State.Pages[indent] = new Character((byte)' ', (byte)(((byte)ctx.ForegroundColor << 4) | (byte)ctx.BackgroundColor), ctx.TextOffset, CharacterFlags.None);

                    renderPosition += ctx.Indentation;
                    charsWritten += ctx.Indentation;
                }

                if (ch == '\n')
                {
                    ctx.State.Pages[renderPosition] =
                       new Character((byte)' ', (byte)(((byte)ctx.ForegroundColor << 4) | (byte)ctx.BackgroundColor), ctx.TextOffset + i, CharacterFlags.None);

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

                //var chFlags = ctx.Underline ? CharacterFlags.Underline : CharacterFlags.None;
                var chFlags = CharacterFlags.None;
                if (ctx.Underline)
                    chFlags |= CharacterFlags.Underline;
                if (ctx.Blink)
                    chFlags |= CharacterFlags.Blink;

                ctx.State.Pages[renderPosition++] =
                    new Character((byte)str[i], (byte)(((byte)ctx.ForegroundColor << 4) | (byte)ctx.BackgroundColor), ctx.TextOffset + i, chFlags);
            }

            return new CommandResult(true, charsWritten);
        }
    }
}
