using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using Serilog;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Entries
{
    public class Data : DocumentEntry
    {
        private StringBuilder stringBuilder;

        public Data(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
            stringBuilder = new StringBuilder();
        }

        private int InputLineLength => int.Parse(GetArgument("ILL", "16"));

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var inverted = ctx.Inverted;
            if (Selected)
                ctx.Inverted = true;

            var inputLine = new string(' ', InputLineLength);
            var charsWritten = WriteString(ctx, $"{Aux}: ");
            ctx.RenderPosition += charsWritten;

            var underLine = ctx.Underline;
            ctx.Underline = true;
            charsWritten += WriteString(ctx, inputLine);
            ctx.Underline = underLine;
            ctx.Inverted = inverted;

            return new CommandResult(true, charsWritten);
        }

        public override void CharKeyPress(ViewerState state, char key, int relativeOffset)
        {
            stringBuilder.Append(key);
            Log.Information("Value for {0} is now: {1}", GetArgument("PROP"), stringBuilder.ToString());
        }

        public override string ToString() => AsString("DA"); //$"$DA,A=\"{Aux}\"$";
    }
}
