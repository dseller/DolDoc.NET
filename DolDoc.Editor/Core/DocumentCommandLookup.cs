using System.Collections.Generic;

namespace DolDoc.Editor.Core
{
    public static class DocumentCommandLookup
    {
        private static readonly IDictionary<string, DocumentCommand> values =
            new Dictionary<string, DocumentCommand>()
            {
                { "BG", DocumentCommand.Background },
                { "BL", DocumentCommand.Blink },
                { "CL", DocumentCommand.Clear },
                { "CM", DocumentCommand.MoveCursor },
                { "FG", DocumentCommand.Foreground },
                { "ID", DocumentCommand.Indent },
                { "IV", DocumentCommand.Invert },
                { "LK", DocumentCommand.Link },
                { "TR", DocumentCommand.Tree },
                { "TX", DocumentCommand.Text },
                { "UL", DocumentCommand.Underline },
                { "WW", DocumentCommand.WordWrap },
            };

        public static DocumentCommand Get(string cmd)
        {
            if (!values.TryGetValue(cmd, out var command))
                return DocumentCommand.Error;

            return command;
        }
    }
}
