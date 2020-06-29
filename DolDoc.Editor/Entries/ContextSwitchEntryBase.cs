using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public abstract class ContextSwitchEntryBase : DocumentEntry
    {
        protected ContextSwitchEntryBase(int textOffset, IList<Flag> flags, IList<Argument> args) 
            : base(textOffset, flags, args)
        {
        }

        protected abstract void Set(EntryRenderContext ctx, bool status);

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            Set(ctx, Arguments[0].Value == "1");
            
            return new CommandResult(true);
        }
    }
}
