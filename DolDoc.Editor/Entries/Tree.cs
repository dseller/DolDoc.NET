using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Tree : Text
    {
        public Tree(int textOffset, IList<Flag> flags, IList<Argument> args)
            : base(textOffset, flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var fgColor = ctx.ForegroundColor;
            var ul = ctx.Underline;
            ctx.Underline = true;
            ctx.ForegroundColor = EgaColor.Purple;

            var writtenCharacters = WriteString(ctx, $"-] {Tag}");

            ctx.ForegroundColor = fgColor;
            ctx.Underline = ul;

            return new CommandResult(true, writtenCharacters);
        }

        public override string ToString() => $"$TR,\"{Tag}\"$";
    }
}
