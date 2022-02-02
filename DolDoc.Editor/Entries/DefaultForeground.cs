using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolDoc.Editor.Entries
{
    [Entry("FD")]
    public class DefaultForeground : DocumentEntry
    {
        public DefaultForeground(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            ctx.Options.DefaultForegroundColor = (EgaColor)Enum.Parse(typeof(EgaColor), Tag, true);
            return new CommandResult(true);
        }

        public override string ToString() => AsString("FD");
    }
}
