using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Rendering
{
    public class CharacterMatrix
    {
        private readonly int _width, _height;
        private readonly Character[] _buffer;

        public CharacterMatrix(int width, int height)
        {
            _width = width;
            _height = height;
            _buffer = new Character[width * height];
        }

        public Character this[int x, int y]
        {
            get => _buffer[(x * _width) + y];
            set => _buffer[(x * _width) + y] = value;
        }

        /// <summary>
        /// Fill the whole matrix with the specified character.
        /// </summary>
        /// <param name="ch">The character.</param>
        public void Fill(Character ch)
        {
            for (int i = 0; i < (_width * _height); i++)
                _buffer[i] = ch;
        }
    }
}
