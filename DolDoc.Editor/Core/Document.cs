﻿using System;
using System.IO;

namespace DolDoc.Editor.Core
{
    public class Document
    {
        public int Columns, Rows;
        private int _maxY;


        private string _content;
        public event Action OnUpdate;

        private bool _enableInterpreter;
        public bool EnableInterpreter
        {
            get { return _enableInterpreter; }
            set { _enableInterpreter = value; Load(_content, true); }
        }

        public Document(int columns, int rows, EgaColor backgroundColor, EgaColor foregroundColor)
        {
            //Rows = rows;
            //Columns = columns;
            //_fgColor = foregroundColor;
            //_bgColor = backgroundColor;
            //_defaultFgColor = _originalFgColor = _fgColor;
            //_defaultBgColor = _originalBgColor = _bgColor;
            //_data = new Character[Rows * Columns];


            //_enableInterpreter = true;

            //Clear(backgroundColor);
        }

        public void Load(string contents, bool restoreCursor = false)
        {
            /*var cX = CursorX;
            var cY = CursorY;
            CursorX = CursorY = 0;

            Clear(_defaultBgColor);

            _content = contents;
            if (EnableInterpreter)
                _interpreter.Render(contents);
            else
                OnWriteString(new DolDocInstruction<string>(CharacterFlags.None, contents, 0));

            if (restoreCursor)
                SetCursor(cX, cY);
            else
            {
                CursorX = 0;
                CursorY = 0;
            }

            OnUpdate();*/
        }

        //private void OnWriteLink(DolDocInstruction<string> obj)
        //{
        //    var oldColor = _fgColor;

        //    _fgColor = EgaColor.Red;
        //    obj.Flags |= CharacterFlags.Underline;
        //    OnWriteString(obj);
        //    _fgColor = oldColor;
        //}

        //public void Write(int x, int y, CharacterFlags flags, char ch, EgaColor fgColor, EgaColor bgColor, int? textOffset)
        //{
        //    var color = ((byte)fgColor << 4) | (byte)bgColor;
        //    _data[(y * Columns) + x] = new Character((byte)ch, (byte)color, textOffset, flags);
        //}

        //public Character Read(int x, int y) => _data[(y * Columns) + x];

        //public int CursorPosition => CursorX + ((CursorY + ViewLine) * Columns);

        //public void Clear(EgaColor color)
        //{
        //    _defaultBgColor = _bgColor = _originalBgColor;
        //    _defaultFgColor = _fgColor = _originalFgColor;

        //    // Y = X * Columns
        //    // CursorX = CursorY = 0;
        //    var rows = _data.Length / Columns;

        //    for (int column = 0; column < Columns; column++)
        //        for (int row = 0; row < rows; row++)
        //        {
        //            var ch = Read(column, row);
        //            if ((ch.Flags & CharacterFlags.Hold) == CharacterFlags.Hold)
        //                continue;

        //            Write(column, row, CharacterFlags.None, (char)0, _fgColor, color, null);
        //        }
        //}

        //public int? GetLastCharacterOnLine(int line)
        //{
        //    for (int i = (line * Columns) + Columns; i > line * Columns; i--)
        //    {
        //        if (_data[i].TextOffset == null)
        //            continue;

        //        return i - (line * Columns);
        //    }

        //    return null;
        //}

        //private void OnWriteString(DolDocInstruction<string> data)
        //{
            //if ((data.Flags & CharacterFlags.Center) == CharacterFlags.Center)
            //    CursorX = (Columns / 2) - (data.Data.Length / 2);
            //else if ((data.Flags & CharacterFlags.Right) == CharacterFlags.Right)
            //    CursorX = Columns - data.Data.Length;

            //if ((data.Flags & CharacterFlags.WordWrap) == CharacterFlags.WordWrap)
            //{
            //    // Get length of first word.
            //    var words = data.Data.Split(null);
            //    foreach (var word in words)
            //    {
            //        if ((CursorX + word.Length) >= Columns)
            //        {
            //            SetCursor(0, CursorY + 1);
            //        }

            //        foreach (var ch in data.Data)
            //            OnWriteCharacter(ch, data.Flags);
            //    }
            //}
            //else
            //foreach (var ch in data.Data)
            //OnWriteCharacter(ch, data.TextOffset, data.Flags);

            /*for (int i = 0; i < data.Data.Length; i++)
                OnWriteCharacter(data.Data[i], data.TextOffset + i, data.Flags);
        }*/

        private void OnWriteCharacter(char obj, int textOffset, CharacterFlags flags)
        {
            /*if (!char.IsControl(obj))
            {
                Write(CursorX, CursorY, flags, obj, _fgColor, _bgColor, textOffset);
                CursorX++;
            }
            else
            {
                if (obj == '\n')
                {
                    Write(CursorX, CursorY, CharacterFlags.None, '^', _fgColor, _bgColor, textOffset);
                    CursorY++;
                    CursorX = 0;
                }
            }

            if (CursorX >= Columns)
            {
                CursorX = 0;
                CursorY++;
            }

            if (CursorPosition >= _data.Length)
            {
                Console.WriteLine("Allocating new data page");

                int oldLength = _data.Length;
                Array.Resize(ref _data, _data.Length + (Columns * Rows));

                for (int i = 0; i < Columns * Rows; i++)
                    _data[oldLength + i] = new Character(0x00, (byte)_bgColor, textOffset, CharacterFlags.None);

            }

            _maxY = Math.Max(_maxY, CursorY);*/
        }
    }
}
