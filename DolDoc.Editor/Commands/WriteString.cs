using DolDoc.Editor.Core;
using System.Linq;

namespace DolDoc.Editor.Commands
{
    public class WriteString : IDolDocCommand
    {
        public virtual CommandResult Execute(DocumentEntry entry, CommandContext ctx)
        {
            // TODO:
            //else if ((data.Flags & CharacterFlags.Right) == CharacterFlags.Right)
            //    CursorX = Columns - data.Data.Length;

            var str = entry.Arguments[0].Value;
            int charsWritten = 0, renderPosition = ctx.RenderPosition;
            if (entry.HasFlag("CX"))
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
                        ctx.State.Pages[indent] = new Character(entry, 0, (byte)' ', new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);

                    renderPosition += ctx.Indentation;
                    charsWritten += ctx.Indentation;
                }

                if (ch == '\n')
                {
                    ctx.State.Pages[renderPosition] =
                       new Character(entry, i, (byte)' ', new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), CharacterFlags.None);

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
                if (entry.HasFlag("H"))
                    chFlags |= CharacterFlags.Hold;

                byte shiftX = 0, shiftY = 0;
                if (entry.Arguments.Any(arg => arg.Key == "SX"))
                    shiftX = byte.Parse(entry.Arguments.First(arg => arg.Key == "SX").Value);

                ctx.State.Pages[renderPosition++] = new Character(
                    entry,
                    i,
                    (byte)str[i],
                    new CombinedColor(ctx.BackgroundColor, ctx.ForegroundColor), 
                    chFlags
                );
            }

            return new CommandResult(true, charsWritten);
        }
    }
}
