using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class WordWrap : ContextSwitchEntryBase
    {
        public WordWrap(IList<Flag> flags, IList<Argument> args)
            : base(flags, args)
        {
        }

        public override string ToString() => $"WW,{Tag}$";

        protected override void Set(EntryRenderContext ctx, bool status) => ctx.WordWrap = status;
    }
}
