using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

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
            var options = ctx.NewOptions();
            options.ForegroundColor = EgaColor.LtBlue;

            var lm = GetArgument("LM");
            var text = Tag ?? lm;

            if (!HasFlag("UL", false))
                options.Underline = true;
            var charsWritten = WriteString(ctx, text);

            ctx.PopOptions();
            return new CommandResult(true, charsWritten);
        }

        public override string ToString() => $"$MA$";
    }
}
