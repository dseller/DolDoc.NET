using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public static class EntryFactory
    {
        public static DocumentEntry Create(string cmd, IList<Flag> flags, IList<Argument> args)
        {
            switch (cmd)
            {
                case "BG": return new Background(flags, args);
                case "BK": return new Blink(flags, args);
                case "CH": return new Char(flags, args);
                case "CL": return new Clear(flags, args);
                case "CM": return new CursorMove(flags, args);
                case "ER": return new Error(null);
                case "FG": return new Foreground(flags, args);
                case "ID": return new Indent(flags, args);
                case "IV": return new Invert(flags, args);
                case "LK": return new Link(flags, args);
                case "SP": return new Sprite(flags, args);
                case "TX": return new Text(flags, args);
                case "TR": return new Tree(flags, args);
                case "UL": return new Underline(flags, args);
                case "WW": return new WordWrap(flags, args);
                default: return new Error($"Unrecognized cmd '{cmd}'.");
            }
        }
    }
}
