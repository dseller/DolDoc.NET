using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Commands
{
    public class Link : WriteString
    {
        public override CommandResult Execute(CommandContext ctx)
        {
            var oldFg = ctx.ForegroundColor;
            ctx.ForegroundColor = EgaColor.Red;

            var result = base.Execute(ctx);

            ctx.ForegroundColor = oldFg;

            return result;
        }
    }
}
