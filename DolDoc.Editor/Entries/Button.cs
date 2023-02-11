using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    [Entry("BT")]
    public class Button : DocumentEntry
    {
        public Button(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var options = ctx.NewOptions();
            if (Selected)
                options.Inverted = true;

            // Add one character for the last border.
            var charsWritten = WriteString(ctx, Tag) + 1;
            WriteBorder(ctx, Tag.Length);

            ctx.PopOptions();
            return new CommandResult(true, charsWritten);
        }

        public override void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            if (key == Key.SPACE || key == Key.ENTER)
                state.Document.ButtonClicked(this);
        }

        public override void Click(ViewerState state) => state.Document.ButtonClicked(this);

        public override bool Clickable => true;

        public override string ToString() => AsString("BT");
    }
}