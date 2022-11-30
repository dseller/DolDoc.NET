using DolDoc.Core.Parser;
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
        public event Action<string> OnSave;

        public Document(string content, IList<BinaryChunk> binaryChunks = null, string path = null)
            : this(binaryChunks)
        {
            Load(content);
            Path = path;
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

        /// <summary>
        /// The parent document, i.e. the document that opened this document.
        /// </summary>
        public Document Parent { get; internal set; }

        /// <summary>
        /// The path in the filesystem to the document. Only set to a value if the document originated from a document on disk.
        /// </summary>
        public string Path { get; }

        public void Load(string contents)
        {
            if (string.IsNullOrEmpty(contents))
                return;

            Entries = new LinkedList<DocumentEntry>(_parser.Parse(contents));
            OnUpdate?.Invoke(this, true);
        }

        public virtual void Reload() => OnUpdate?.Invoke(this, true);

        public void Write(string content)
        {
            if (string.IsNullOrEmpty(content))
                return;

            foreach (var entry in _parser.Parse(content))
                Entries.AddLast(entry);
            OnUpdate?.Invoke(this, false);
        }

        public void Refresh() => OnUpdate?.Invoke(this, false);

        public virtual object GetData(string key) => null;

        public virtual void Save() => OnSave?.Invoke(ToPlainText());

        public string ToPlainText() => Entries.Aggregate(string.Empty, (acc, entry) =>
        {
            acc += entry.ToString();
            return acc;
        });
    }
}
