using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;
using System.Linq;

namespace DolDoc.Editor.Entries
{
    public class Tree : DocumentEntry
    {
        public Tree(IList<Flag> flags, IList<Argument> args)
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var options = ctx.NewOptions();
            options.Underline = true;
            options.ForegroundColor = EgaColor.Purple;

            bool collapsed = !HasFlag("C", false);
            var icon = collapsed ? "+" : "-";

            var writtenCharacters = WriteString(ctx, $"{icon}] {Tag}");

            ctx.PopOptions();

            // Find the tree's domain. It spans from the first $ID$ to the $ID$ that brings it to its original level.
            if (collapsed)
                ctx.Options.CollapsedTreeNodeIndentationLevel = ctx.Options.Indentation;

            return new CommandResult(true, writtenCharacters);
        }

        public override void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            if (key == Key.SPACE || key == Key.ENTER)
                Toggle(state);
        }

        public override void Click(ViewerState state) => Toggle(state);

        public override string ToString() => AsString("TR");

        public override bool Clickable => true;

        private void Toggle(ViewerState state)
        {
            if (HasFlag("C", false))
            {
                // It is expended, collapse it.
                Flags.Remove(Flags.FirstOrDefault(f => f.Value == "C" && !f.Status));
            }
            else
                Flags.Add(new Flag(false, "C"));

            state.Document.Reload();
        }
    }
}
