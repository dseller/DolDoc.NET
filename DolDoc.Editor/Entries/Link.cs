using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Link : DocumentEntry
    {
        public Link(IList<Flag> flags, IList<Argument> args) 
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var oldFg = ctx.ForegroundColor;
            var oldUl = ctx.Underline;

            ctx.ForegroundColor = EgaColor.Red;

            if (!HasFlag("UL", false))
                ctx.Underline = true;

            //var result = base.Evaluate(ctx);
            var charsWritten = WriteString(ctx, Tag);
            if (HasFlag("B", true))
                WriteBorder(ctx, charsWritten);

            ctx.ForegroundColor = oldFg;
            if (!HasFlag("UL", false))
                ctx.Underline = oldUl;

            

            return new CommandResult(true, charsWritten);
        }

        public override string ToString() => $"$LK,\"{Tag}\"$";
    }
}
