using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    [Entry("BD")]
    public class DefaultBackground : DocumentEntry
    {
        public DefaultBackground(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            ctx.Options.DefaultBackgroundColor = (EgaColor)Enum.Parse(typeof(EgaColor), Tag, true);
            return new CommandResult(true);
        }

        public override string ToString() => AsString("BD");
    }
}
