using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Button : DocumentEntry
    {
        public Button(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var inverted = ctx.Inverted;
            if (Selected)
                ctx.Inverted = true;

            var charsWritten = WriteString(ctx, Tag);
            WriteBorder(ctx, Tag.Length);

            ctx.Inverted = inverted;
            return new CommandResult(true, charsWritten);
        }

        public override string ToString() => AsString("BT");
    }
}
