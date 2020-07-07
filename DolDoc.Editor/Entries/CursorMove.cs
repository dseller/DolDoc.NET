using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Entries
{
    public class CursorMove : DocumentEntry
    {
        public CursorMove(IList<Flag> flags, IList<Argument> args) 
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var columns = int.Parse(Arguments[0].Value);
            var rows = HasFlag("RE", false) ? 0 : int.Parse(Arguments[1].Value);

            int originX = ctx.RenderPosition % ctx.State.Columns, originY = ctx.RenderPosition / ctx.State.Columns;
            if (HasFlag("LX"))
                originX = 0;
            else if (HasFlag("RX"))
                originX = ctx.State.Columns;
            else if (HasFlag("CX"))
                originX = ctx.State.Columns / 2;

            if (HasFlag("TY"))
                originY = 0;
            else if (HasFlag("BY"))
                originY = ctx.State.Rows - 1;
            else if (HasFlag("CY"))
                originY = (ctx.State.Rows - 1) / 2;

            ctx.RenderPosition = (originX + columns) + ((originY + rows) * ctx.State.Columns);

            return new CommandResult(true);
        }

        public override string ToString()
        {
            return "TODO";
        }
    }
}
