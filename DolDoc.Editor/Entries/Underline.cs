using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    [Entry("UL")]
    public class Underline : ContextSwitchEntryBase
    {
        public Underline(IList<Flag> flags, IList<Argument> args)
            : base(flags, args)
        {
        }

        public override string ToString() => AsString("UL");

        protected override void Set(EntryRenderContext ctx, bool status) => ctx.Options.Underline = status;
    }
}
