using System;
using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;
using System.IO;

namespace DolDoc.Editor.Entries
{
    [Entry("LK")]
    public class Link : DocumentEntry
    {
        public Link(IList<Flag> flags, IList<Argument> args) 
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var options = ctx.NewOptions();
            options.ForegroundColor = EgaColor.Red;

            if (!HasFlag("UL", false))
                options.Underline = true;

            var charsWritten = WriteString(ctx, Tag);

            ctx.PopOptions();
            return new CommandResult(true, charsWritten);
        }

        public override void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            if (key == Key.SPACE)
                OpenDocument(state);
        }

        public override void Click(ViewerState state) => OpenDocument(state);

        public override bool Clickable => true;

        public override string ToString() => AsString("LK");

        private void OpenDocument(ViewerState state)
        {
            var file = string.IsNullOrEmpty(Aux) ? Tag : Aux;

            // Follow the link.
            if (!File.Exists(file))
                return;

            using (var fs = File.Open(file, FileMode.Open))
            {
                try
                {
                    var document = DocumentLoader.Load(fs, file);
                    state.LoadDocument(document, true);
                }
                catch (Exception ex)
                {
                    state.Document.Entries.AddLast(new Error(ex.ToString()));
                }
            }
        }
    }
}
