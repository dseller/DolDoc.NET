using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Commands
{
    public class Tree : WriteString
    {
        public override CommandResult Execute(CommandContext ctx)
        {
            var fgColor = ctx.ForegroundColor;
            var ul = ctx.Underline;
            ctx.Underline = true;
            ctx.ForegroundColor = EgaColor.Purple;

            ctx.Arguments[0] = new Argument(null, $"-] {ctx.Arguments[0].Value}");
            var result = base.Execute(ctx);

            ctx.ForegroundColor = fgColor;
            ctx.Underline = ul;

            return result;
        }
    }
}
