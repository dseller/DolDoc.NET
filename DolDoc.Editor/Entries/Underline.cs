using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Underline : ContextSwitchEntryBase
    {
        public Underline(IList<Flag> flags, IList<Argument> args)
            : base(flags, args)
        {
        }

        public override string ToString() => $"UL,{Tag}$";

        protected override void Set(EntryRenderContext ctx, bool status) => ctx.Underline = status;
    }
}
