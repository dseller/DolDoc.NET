﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DolDoc.Editor.Core
{
    public class CharacterPage : IEnumerable<Character>
    {
        private Character[] _characters;

        public int PageNumber { get; set; }

        public int Columns { get; }

        public int Rows { get; }

        public CharacterPage(ViewerState viewerState, int pageNo, int columns, int rows)
        {
            Rows = rows;
            Columns = columns;
            PageNumber = pageNo;
            _characters = new Character[rows * columns];
            for (int i = 0; i < _characters.Length; i++)
                _characters[i] = new Character((pageNo * columns * rows) + i);

            Clear(viewerState.DefaultBackgroundColor);
        }

        public Character this[int pos]
        {
            get => _characters[pos];
            set => _characters[pos] = value;
        }

        public Character this[int x, int y]
        {
            get => _characters[(y * Columns) + x];
            set => _characters[(y * Columns) + x] = value;
        }

        public void Clear(EgaColor color)
        {
            for (int i = 0; i < _characters.Length; i++)
                if ((_characters[i].Flags & CharacterFlags.Hold) == 0)
                    _characters[i].Write(null, i, 0x00, new CombinedColor(color, color), CharacterFlags.None);
        }

        public IEnumerator<Character> GetEnumerator() => _characters.AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _characters.GetEnumerator();
    }
}
