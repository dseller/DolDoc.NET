// <copyright file="Document.cs" company="Dennis Seller">
// Copyright (c) Dennis Seller. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using DolDoc.Core.Parser;
using DolDoc.Editor.Entries;
using DolDoc.Editor.Parser;

namespace DolDoc.Editor.Core
{
    public class Document
    {
        private readonly IDolDocParser parser;

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

            parser = new AntlrParser();
            Entries = new LinkedList<DocumentEntry>();
        }

        public event Action<Document> OnUpdate;

        public event Action<Button> OnButtonClick;

        public event Action<string, object> OnFieldChange;

        public event Action<Macro> OnMacro;

        public event Action<string> OnPromptEntered;

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
            Entries = new LinkedList<DocumentEntry>(parser.Parse(contents));
            OnUpdate?.Invoke(this);
        }

        public void Write(string content)
        {
            foreach (var entry in parser.Parse(content))
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
