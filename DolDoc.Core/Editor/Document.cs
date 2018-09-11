using System;
using System.IO;
using System.Linq;

namespace DolDoc.Core.Editor
{
    public class Document
    {
        private ushort[] _data;
        public int Columns, Rows, CursorX, CursorY;
        private int _maxY;
        private bool _underline;
        private EgaColor _bgColor, _fgColor;
        private EgaColor _defaultBgColor, _defaultFgColor;

        public int ViewLine;
        public event Action OnUpdate;

        public Document(int columns, int rows, EgaColor backgroundColor, EgaColor foregroundColor)
        {
            Rows = rows;
            Columns = columns;
            _fgColor = foregroundColor;
            _bgColor = backgroundColor;
            _defaultFgColor = _fgColor;
            _defaultBgColor = _bgColor;
            _data = new ushort[Rows * Columns];
            Clear(backgroundColor);
        }

        public void Load(Stream stream)
        {
            var interpreter = new DolDocInterpreter(stream);
            interpreter.OnClear += () => Clear(_bgColor);
            interpreter.OnWriteCharacter += ch => OnWriteCharacter(ch, true);
            interpreter.OnWriteString += OnWriteString;
            interpreter.OnWriteLink += OnWriteLink;
            interpreter.OnForegroundColor += color => _fgColor = color ?? _defaultFgColor;
            interpreter.OnBackgroundColor += color => _bgColor = color ?? _defaultBgColor;
            interpreter.OnUnderline += value => _underline = value;
            interpreter.Run();
            CursorX = 0;
            CursorY = 0;
            OnUpdate();
        }

        private void OnWriteLink(DolDocInstruction<string> obj)
        {
            var oldColor = _fgColor;
            _fgColor = EgaColor.Red;
            OnWriteString(obj);
            _fgColor = oldColor;
        }

        public void Write(int x, int y, char ch, EgaColor fgColor, EgaColor bgColor)
        {
            var color = ((byte)fgColor << 4) | (byte)bgColor;
            _data[(y * Columns) + x] = (ushort)((((byte)color) << 8) | (byte)ch);
        }

        public ushort Read(int x, int y) => _data[(y * Columns) + x];

        public void Clear(EgaColor color)
        {
            for (int column = 0; column < Columns; column++)
                for (int row = 0; row < Rows; row++)
                    Write(column, row, (char)0, _fgColor, color);
        }

        public void PreviousPage()
        {
            if (ViewLine == 0)
                return;

            ViewLine -= Rows;
            if (ViewLine < 0)
                ViewLine = 0;

            OnUpdate();
        }

        public void NextPage()
        {
            /*if (ViewOffset == _pages.Count - 1)
                return;*/

            ViewLine += Rows;
            OnUpdate();
        }

        public void LastPage()
        {
            //_page = _pages.Count - 1;
            //ViewOffset = 

            ViewLine = _maxY - Rows + 1;
            OnUpdate();
        }

        public void SetCursor(int x, int y)
        {
            if (CursorX < 0)
                CursorX = 0;
            if (CursorY < 0)
                CursorY = 0;

            CursorX = x;
            CursorY = y;

        }

        private void OnWriteString(DolDocInstruction<string> data)
        {
            if (data.Flags.Contains("+CX"))
                CursorX = (Columns / 2) - (data.Data.Length / 2);

            foreach (var ch in data.Data)
                OnWriteCharacter(ch, false);
        }

        private void OnWriteCharacter(char obj, bool refresh)
        {
            if (!char.IsControl(obj))
            {
                Write(CursorX, CursorY, obj, _fgColor, _bgColor);
                CursorX++;
            }
            else
            {
                if (obj == '\n')
                {
                    CursorY++;
                    CursorX = 0;
                }
            }

            if (CursorX >= Columns)
            {
                CursorX = 0;
                CursorY++;
            }

            if ((CursorY % Rows) == 0)
            {
                int oldLength = _data.Length;
                Array.Resize(ref _data, _data.Length + (Columns * Rows));

                for (int i = 0; i < Columns * Rows; i++)
                    _data[oldLength + i] = (ushort)((byte)_bgColor << 8);
            }

            _maxY = Math.Max(_maxY, CursorY);
        }
    }
}
