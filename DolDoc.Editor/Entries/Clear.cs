using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Clear : DocumentEntry
    {
        public Clear(IList<Flag> flags, IList<Argument> args) 
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var toRemove = new List<DocumentEntry>();

            foreach (var e in ctx.Document.Entries)
            {
                if (!e.HasFlag("H"))
                    toRemove.Add(e);

                if (e == this)
                    break;
            }

            foreach (var item in toRemove)
                ctx.Document.Entries.Remove(item);

            return new CommandResult(true, refreshRequested: true);
        }

        public override string ToString() => "$CL$";
    }
}
