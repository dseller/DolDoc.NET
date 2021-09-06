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

        public bool Checked { get; private set; }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var options = ctx.NewOptions();
            options.Inverted = true;

            var writtenChars = WriteString(ctx, $"{Tag}: [{(Checked ? "X" : " ")}]");

            ctx.PopOptions();
            return new CommandResult(true, writtenChars);
        }

        public override void KeyPress(ViewerState state, Key key, char? character ,int relativeOffset)
        {
            if (key == Key.SPACE || key == Key.ENTER)
            {
                Checked = !Checked;
                state.Document.FieldChanged(GetArgument("PROP"), Checked);
            }
        }

        public override string ToString() => AsString("CB");
    }
}
