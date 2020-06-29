using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Commands
{
    public class Link : WriteString
    {
        public override CommandResult Execute(DocumentEntry entry, CommandContext ctx)
        {
            var oldFg = ctx.ForegroundColor;
            var oldUl = ctx.Underline;

            ctx.ForegroundColor = EgaColor.Red;
            ctx.Underline = true;

            var result = base.Execute(entry, ctx);

            ctx.ForegroundColor = oldFg;
            ctx.Underline = oldUl;

            return result;
        }
    }
}
