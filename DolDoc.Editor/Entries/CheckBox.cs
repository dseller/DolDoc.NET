using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class CheckBox : DocumentEntry
    {
        public CheckBox(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
        }

        private string Property => GetArgument("PROP");

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var @checked = ctx.Document.GetData(Property) as bool? ?? false;
            var options = ctx.NewOptions();
            if (Selected)
                options.Inverted = true;

            var writtenChars = WriteString(ctx, $"{Tag}: [{(@checked ? "X" : " ")}]");

            ctx.PopOptions();
            return new CommandResult(true, writtenChars);
        }

        public override void KeyPress(ViewerState state, Key key, char? character ,int relativeOffset)
        {
            if (key == Key.SPACE || key == Key.ENTER)
            {
                // Checked = !Checked;
                var @checked = state.Document.GetData(Property) as bool? ?? false;
                state.Document.FieldChanged(GetArgument("PROP"), !@checked);
            }
        }

        public override string ToString() => AsString("CB");
    }
}
