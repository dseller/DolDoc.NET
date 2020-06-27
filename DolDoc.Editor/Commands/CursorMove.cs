using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Commands
{
    public class CursorMove : IDolDocCommand
    {
        public CommandResult Execute(CommandContext ctx)
        {
            var columns = int.Parse(ctx.Arguments[0].Value);
            var rows = int.Parse(ctx.Arguments[1].Value);

            int originX = ctx.RenderPosition % ctx.State.Columns, originY = ctx.RenderPosition / ctx.State.Columns;
            if (ctx.HasFlag("LX"))
                originX = 0;
            else if (ctx.HasFlag("RX"))
                originX = ctx.State.Columns;
            else if (ctx.HasFlag("CX"))
                originX = ctx.State.Columns / 2;

            if (ctx.HasFlag("TY"))
                originY = 0;
            else if (ctx.HasFlag("BY"))
                originY = ctx.State.Rows - 1;
            else if (ctx.HasFlag("CY"))
                originY = (ctx.State.Rows - 1) / 2;

            ctx.RenderPosition = (originX + columns) + ((originY + rows) * ctx.State.Columns);

            return new CommandResult(true);
        }
    }
}
