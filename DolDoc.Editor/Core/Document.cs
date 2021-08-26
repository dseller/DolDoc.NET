using DolDoc.Core.Parser;
using DolDoc.Editor.Parser;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DolDoc.Editor.Core
{
    public class Document
    {
        private IDolDocParser _parser;

        public event Action<Document> OnUpdate;

        public Document(string content, int columns = 80, int rows = 60, EgaColor defaultBgColor = EgaColor.White, EgaColor defaultFgColor = EgaColor.Black, IList<BinaryChunk> binaryChunks = null)
            : this(columns, rows, defaultBgColor, defaultFgColor, binaryChunks)
        {
            Load(content);
        }

        public Document(int columns = 80, int rows = 60, EgaColor defaultBgColor = EgaColor.White, EgaColor defaultFgColor = EgaColor.Black, IList<BinaryChunk> binaryChunks = null)
        {
            Rows = rows;
            Columns = columns;
            BinaryChunks = binaryChunks;

            _parser = new AntlrParser();
            Entries = new LinkedList<DocumentEntry>();
        }

        public int Columns { get; }

        public int Rows { get; }

        public LinkedList<DocumentEntry> Entries { get; private set; }
        
        public IList<BinaryChunk> BinaryChunks { get; private set; }

        public void Load(string contents)
        {
            Entries = new LinkedList<DocumentEntry>(_parser.Parse(contents));
            OnUpdate?.Invoke(this);
        }

        public void Refresh() => OnUpdate?.Invoke(this);

        public string ToPlainText()
        {
            return "TODO";
        }
    }
}
