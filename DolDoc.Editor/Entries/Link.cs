using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace DolDoc.Editor.Entries
{
    public class Link : DocumentEntry
    {
        public Link(IList<Flag> flags, IList<Argument> args) 
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var oldFg = ctx.ForegroundColor;
            var oldUl = ctx.Underline;

            ctx.ForegroundColor = EgaColor.Red;

            if (!HasFlag("UL", false))
                ctx.Underline = true;
            var text = GetArgument("A") ?? Tag;

            //var result = base.Evaluate(ctx);
            var charsWritten = WriteString(ctx, text);

            ctx.ForegroundColor = oldFg;
            if (!HasFlag("UL", false))
                ctx.Underline = oldUl;

            return new CommandResult(true, charsWritten);
        }

        public override void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            if (key == Key.SPACE)
            {
                // Follow the link.
                if (!File.Exists(Tag))
                    return;

                using (var fs = File.Open(Tag, FileMode.Open))
                {
                    var document = DocumentLoader.Load(fs, state.Columns, state.Rows);
                    state.LoadDocument(document);
                }
            }
        }

        public override string ToString() => AsString("LK");
    }
}
