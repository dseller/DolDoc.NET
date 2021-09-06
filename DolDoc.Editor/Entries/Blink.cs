using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Blink : ContextSwitchEntryBase
    {
        public Blink(IList<Flag> flags, IList<Argument> args) 
            : base(flags, args)
        {
        }

        public override string ToString() => $"$BK,{Tag}$";

        protected override void Set(EntryRenderContext ctx, bool status) => ctx.Options.Blink = status;
    }
}
