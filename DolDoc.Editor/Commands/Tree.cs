using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Commands
{
    public class Tree : WriteString
    {
        public override CommandResult Execute(DocumentEntry entry, CommandContext ctx)
        {
            var fgColor = ctx.ForegroundColor;
            var ul = ctx.Underline;
            ctx.Underline = true;
            ctx.ForegroundColor = EgaColor.Purple;

            //entry.Arguments[0] = new Argument(null, $"-] {entry.Arguments[0].Value}");
            var result = base.Execute(entry, ctx);

            ctx.ForegroundColor = fgColor;
            ctx.Underline = ul;

            return result;
        }
    }
}
