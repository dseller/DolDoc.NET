using DolDoc.Core.Parser;
using System.Collections.Generic;

namespace DolDoc.Editor.Core
{
    public class Document
    {
        private IDolDocParser _parser;
        private LinkedList<DocumentEntry> _entries;

        public Document(string content)
            : this()
        {
            Load(content);
        }

        public Document(int columns = 80, int rows = 60, EgaColor defaultBgColor = EgaColor.White, EgaColor defaultFgColor = EgaColor.Black)
        {
            Rows = rows;
            Columns = columns;

            _parser = new LegacyParser();
            _entries = new LinkedList<DocumentEntry>();
        }

        public int Columns { get; }

        public int Rows { get; }

        public void Load(string contents)
        {
            _entries = new LinkedList<DocumentEntry>(_parser.Parse(contents));
        }
    }
}
