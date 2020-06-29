using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class CursorMove : IDolDocCommand
    {
        public CommandResult Execute(DocumentEntry entry, CommandContext ctx)
        {
            var columns = int.Parse(entry.Arguments[0].Value);
            var rows = int.Parse(entry.Arguments[1].Value);

            int originX = ctx.RenderPosition % ctx.State.Columns, originY = ctx.RenderPosition / ctx.State.Columns;
            if (entry.HasFlag("LX"))
                originX = 0;
            else if (entry.HasFlag("RX"))
                originX = ctx.State.Columns;
            else if (entry.HasFlag("CX"))
                originX = ctx.State.Columns / 2;

            if (entry.HasFlag("TY"))
                originY = 0;
            else if (entry.HasFlag("BY"))
                originY = ctx.State.Rows - 1;
            else if (entry.HasFlag("CY"))
                originY = (ctx.State.Rows - 1) / 2;

            ctx.RenderPosition = (originX + columns) + ((originY + rows) * ctx.State.Columns);

            return new CommandResult(true);
        }
    }
}
