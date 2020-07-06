using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Entries
{
    public class Char : DocumentEntry
    {
        public Char(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx) => new CommandResult(true, WriteString(ctx, new string((char)int.Parse(Tag), 1)));

        public override string ToString() => $"$CH,{Tag}$";
    }
}
