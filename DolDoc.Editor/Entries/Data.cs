using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Entries
{
    public class Data : DocumentEntry
    {
        public Data(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var charsWritten = WriteString(ctx, $"{Aux}: ");

            return new CommandResult(true, charsWritten);
        }

        public override string ToString() => AsString("DA"); //$"$DA,A=\"{Aux}\"$";
    }
}
