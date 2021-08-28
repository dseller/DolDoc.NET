using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Text;

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
            var writtenChars = WriteString(ctx, $"[{(Checked ? "X" : " ")}] {Tag}");

            return new CommandResult(true, writtenChars);
        }

        public override void CharKeyPress(ViewerState state, char key, int relativeOffset)
        {
            if (key == ' ' || key == '\r')
                Checked = !Checked;
        }

        public override string ToString() => $"$CB,\"{Tag}\"$";
    }
}
