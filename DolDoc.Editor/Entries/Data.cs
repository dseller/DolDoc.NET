using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Data : DocumentEntry
    {
        public Data(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
        }

        private int InputLineLength => int.Parse(GetArgument("ILL", "16"));

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var inputLine = new string(' ', InputLineLength);
            var charsWritten = WriteString(ctx, $"{Aux}: ");
            ctx.RenderPosition += charsWritten;

            var underLine = ctx.Underline;
            ctx.Underline = true;
            charsWritten += WriteString(ctx, inputLine);
            ctx.Underline = underLine;

            return new CommandResult(true, charsWritten);
        }

        public override string ToString() => AsString("DA"); //$"$DA,A=\"{Aux}\"$";
    }
}
