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
            var fgColor = ctx.ForegroundColor;
            var ul = ctx.Underline;
            ctx.Underline = true;
            ctx.ForegroundColor = EgaColor.Purple;

            bool collapsed = !HasFlag("C", false);
            var icon = collapsed ? "+" : "-";

            var writtenCharacters = WriteString(ctx, $"{icon}] {Tag}");

            ctx.ForegroundColor = fgColor;
            ctx.Underline = ul;

            // Find the tree's domain. It spans from the first $ID$ to the $ID$ that brings it to its original level.

            if (collapsed)
                ctx.CollapsedTreeNodeIndentationLevel = ctx.Indentation;

            return new CommandResult(true, writtenCharacters);
        }

        public override void KeyPress(ViewerState state, Key key, int relativeOffset)
        {
            if (key == Key.SPACE || key == Key.ENTER)
                Toggle();
        }
        
        public override void Click()
        {
            Toggle();
        }

        public override string ToString() => $"$TR,\"{Tag}\"$";

        public override bool Clickable => true;

        private void Toggle()
        {
            if (HasFlag("C", false))
            {
                // It is expended, collapse it.
                Flags.Remove(Flags.FirstOrDefault(f => f.Value == "C" && !f.Status));
            }
            else
                Flags.Add(new Flag(false, "C"));
        }
    }
}
