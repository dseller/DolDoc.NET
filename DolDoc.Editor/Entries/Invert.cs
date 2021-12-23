using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    [Entry("IV")]
    public class Invert : ContextSwitchEntryBase
    {
        public Invert(IList<Flag> flags, IList<Argument> args) 
            : base(flags, args)
        {
        }

        public override string ToString() => AsString("IV");

        protected override void Set(EntryRenderContext ctx, bool status) => ctx.Options.Inverted = status;
    }
}
