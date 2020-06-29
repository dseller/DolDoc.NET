using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Commands
{
    public class Clear : IDolDocCommand
    {
        public CommandResult Execute(DocumentEntry entry, CommandContext ctx)
        {
            var toRemove = new List<DocumentEntry>();

            foreach (var e in ctx.Document.Entries)
            {
                if (!e.HasFlag("H"))
                    toRemove.Add(e);

                if (e == entry)
                    break;
            }

            foreach (var item in toRemove)
                ctx.Document.Entries.Remove(item);

            return new CommandResult(true, refreshRequested: true);
        }
    }
}
