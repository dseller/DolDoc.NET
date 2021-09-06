using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Title : DocumentEntry
    {
        public Title(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            ctx.State.Title = Tag;
            return new CommandResult(true);
        }

        public override string ToString() => AsString("TI");
    }
}
