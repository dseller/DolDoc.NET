using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Entries
{
    public class Macro : DocumentEntry
    {
        public Macro(IList<Flag> flags, IList<Argument> args) 
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var lm = GetArgument("LM");
            //if (lm == null)
            //    return new CommandResult(false);

            var text = Tag ?? lm;

            var oldFg = ctx.ForegroundColor;
            var oldUl = ctx.Underline;

            ctx.ForegroundColor = EgaColor.LtBlue;
            if (!HasFlag("UL", false))
                ctx.Underline = true;

            //var result = base.Evaluate(ctx);
            var charsWritten = WriteString(ctx, text);
            if (HasFlag("B", true))
                WriteBorder(ctx, charsWritten);

            ctx.ForegroundColor = oldFg;
            if (!HasFlag("UL", false))
                ctx.Underline = oldUl;

            return new CommandResult(true, charsWritten);
        }

        public override string ToString() => $"$MA$";
    }
}
