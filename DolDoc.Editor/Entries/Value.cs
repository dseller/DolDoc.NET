using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Value : DocumentEntry
    {
        public Value(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var charsWritten = 0;
            if (!string.IsNullOrWhiteSpace(Aux))
            {
                charsWritten = WriteString(ctx, $"{Aux}: ");
                ctx.RenderPosition += charsWritten;
            }

            var value = ctx.Document.GetData(Property)?.ToString();
            if (string.IsNullOrEmpty(value))
                return new CommandResult(true);

            charsWritten += WriteString(ctx, value);
            return new CommandResult(true, charsWritten);
        }

        private string Property => GetArgument("PROP");

        public override string ToString() => AsString("VA");
    }
}
