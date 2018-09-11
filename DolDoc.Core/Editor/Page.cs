using System;

namespace DolDoc.Core.Editor
{
    public class Page
    {
        private ushort[] _data;
        private int _columns, _rows;

        public Page(int columns, int rows)
        {
            _rows = rows;
            _columns = columns;
            _data = new ushort[columns * rows];
        }

        public void Write(int x, int y, char ch, EgaColor fgColor, EgaColor bgColor)
        {
            var color = ((byte)fgColor << 4) | (byte)bgColor;
            _data[(y * _columns) + x] = (ushort)((((byte)color) << 8) | (byte)ch);
        }

        public ushort Read(int x, int y) => _data[(y * _columns) + x];

        public void Clear(EgaColor color)
        {
            for (int column = 0; column < _columns; column++)
                for (int row = 0; row < _rows; row++)
                    Write(column, row, (char)0, color, color);
        }
    }
}
