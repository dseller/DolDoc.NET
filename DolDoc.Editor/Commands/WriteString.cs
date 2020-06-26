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

                if (ch == '\n')
                {
                    var charsUntilEndOfLine = ctx.State.Columns - (renderPosition % ctx.State.Columns);
                    renderPosition += charsUntilEndOfLine;
                    charsWritten += charsUntilEndOfLine;
                    continue;
                }
                else
                    charsWritten++;

                var chFlags = ctx.Underline ? CharacterFlags.Underline : CharacterFlags.None;
                ctx.State.Pages[renderPosition++] =
                    new Character((byte)str[i], (byte)(((byte)ctx.ForegroundColor << 4) | (byte)ctx.BackgroundColor), ctx.TextOffset, chFlags);
            }

            return new CommandResult(true, charsWritten);
        }
    }
}
