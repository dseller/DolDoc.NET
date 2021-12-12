﻿using DolDoc.Core.Parser;
using DolDoc.Editor.Entries;
using DolDoc.Editor.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DolDoc.Editor.Core
{
    public class Document
    {
        private IDolDocParser _parser;

        public event Action<Document, bool> OnUpdate;
        public event Action<Button> OnButtonClick;
        public event Action<string, object> OnFieldChange;
        public event Action<DocumentEntry> OnMacro;
        public event Action<string> OnPromptEntered;

        public Document(string content, IList<BinaryChunk> binaryChunks = null)
            : this(binaryChunks)
        {
            Load(content);
        }

        public Document(IList<BinaryChunk> binaryChunks = null)
        {
            BinaryChunks = binaryChunks;

            _parser = new AntlrParser();
            Entries = new LinkedList<DocumentEntry>();
        }

        public LinkedList<DocumentEntry> Entries { get; private set; }
        
        public IList<BinaryChunk> BinaryChunks { get; private set; }

        public void ButtonClicked(Button btn) => OnButtonClick?.Invoke(btn);

        public void FieldChanged(string name, object value) => OnFieldChange?.Invoke(name, value);

        public virtual void Macro(DocumentEntry entry) => OnMacro?.Invoke(entry);

        public void PromptEntered(string value) => OnPromptEntered?.Invoke(value);

        public void Load(string contents)
        {
            Entries = new LinkedList<DocumentEntry>(_parser.Parse(contents));
            OnUpdate?.Invoke(this, true);
        }

        public virtual void Reload() => OnUpdate?.Invoke(this, true);

        public void Write(string content)
        {
            foreach (var entry in _parser.Parse(content))
                Entries.AddLast(entry);
            OnUpdate?.Invoke(this, false);
        }

        public void Refresh() => OnUpdate?.Invoke(this, false);

        public virtual object GetData(string key) => null;

        public string ToPlainText() => Entries.Aggregate(string.Empty, (acc, entry) =>
        {
            acc += entry.ToString();
            return acc;
        });
    }
}
