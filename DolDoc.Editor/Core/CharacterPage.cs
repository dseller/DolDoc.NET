﻿using System;

namespace DolDoc.Editor.Core
{
    public class CharacterPage
    {
        private Character[] _characters;

        public int PageNumber { get; set; }

        public int Columns { get; }

        public int Rows { get; }

        public CharacterPage(int pageNo, int columns, int rows)
        {
            Rows = rows;
            Columns = columns;
            PageNumber = pageNo;
            _characters = new Character[rows * columns];

            // TODO: get this from the state...
            Clear(EgaColor.White);
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
                _characters[i] = new Character(0x00, (byte)color, null, CharacterFlags.None);
        }
    }
}
