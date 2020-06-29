using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Entries
{
    public static class EntryFactory
    {
        public static DocumentEntry Create(string cmd, int offset, IList<Flag> flags, IList<Argument> args)
        {
            switch (cmd)
            {
                case "BG": return new Background(offset, flags, args);
                case "BK": return new Blink(offset, flags, args);
                case "CL": return new Clear(offset, flags, args);
                case "CM": return new CursorMove(offset, flags, args);
                case "ER": return new Error(offset, null);
                case "FG": return new Foreground(offset, flags, args);
                case "ID": return new Indent(offset, flags, args);
                case "IV": return new Invert(offset, flags, args);
                case "LK": return new Link(offset, flags, args);
                case "TX": return new Text(offset, flags, args);
                case "TR": return new Tree(offset, flags, args);
                case "UL": return new Underline(offset, flags, args);
                case "WW": return new WordWrap(offset, flags, args);
                default: return new Error(offset, $"Unrecognized cmd '{cmd}'.");
            }
        }
    }
}
