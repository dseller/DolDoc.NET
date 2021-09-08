﻿using DolDoc.Core.Parser;
using DolDoc.Editor.Entries;
using DolDoc.Editor.Parser;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace DolDoc.Editor.Core
{
    public class Document
    {
        private IDolDocParser _parser;

        public event Action<Document> OnUpdate;
        public event Action<Button> OnButtonClick;
        public event Action<string, object> OnFieldChange;
        public event Action<Macro> OnMacro;
        public event Action<string> OnPromptEntered;

        public Document(string content, int columns = 80, int rows = 60, IList<BinaryChunk> binaryChunks = null)
            : this(columns, rows, binaryChunks)
        {
            Load(content);
        }

        public Document(int columns = 80, int rows = 60, IList<BinaryChunk> binaryChunks = null)
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

        public void ButtonClicked(Button btn) => OnButtonClick?.Invoke(btn);

        public void FieldChanged(string name, object value) => OnFieldChange?.Invoke(name, value);

        public void Macro(Macro macro) => OnMacro?.Invoke(macro);

        public void PromptEntered(string value) => OnPromptEntered?.Invoke(value);

        public void Load(string contents)
        {
            Entries = new LinkedList<DocumentEntry>(_parser.Parse(contents));
            OnUpdate?.Invoke(this);
        }

        public void Write(string content)
        {
            foreach (var entry in _parser.Parse(content))
                Entries.AddLast(entry);
            OnUpdate?.Invoke(this);
        }

        public void Refresh() => OnUpdate?.Invoke(this);

        public virtual object GetData(string key) => null;

        public string ToPlainText() => Entries.Aggregate(string.Empty, (acc, entry) =>
        {
            acc += entry.ToString();
            return acc;
        });
    }
}
