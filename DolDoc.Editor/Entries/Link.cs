using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Link : Text
    {
        public Link(int textOffset, IList<Flag> flags, IList<Argument> args) : base(textOffset, flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var oldFg = ctx.ForegroundColor;
            var oldUl = ctx.Underline;

            ctx.ForegroundColor = EgaColor.Red;
            ctx.Underline = true;

            var result = base.Evaluate(ctx);

            ctx.ForegroundColor = oldFg;
            ctx.Underline = oldUl;

            return result;
        }

        public override string ToString() => $"$LK,\"{Tag}\"$";
    }
}
