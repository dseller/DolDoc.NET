using DolDoc.Core.Editor;
using DolDoc.Editor.Input;
using DolDoc.Editor.Rendering;
using DolDoc.Editor.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor
{
    public class EditorState : IInputListener
    {
        private readonly PieceTable _pieceTable;
        private readonly Composer _composer;
        private readonly CharacterMatrix _visualRepresentation;
        private int _cursorPosition, _columns, _rows;

        public event Action<string> OnUpdate;

        public EditorState(int columns, int rows, string content = null)
        {
            _rows = rows;
            _columns = columns;
            _pieceTable = new PieceTable(content);

            Console.SetWindowSize(_columns, _rows);
            Console.SetBufferSize(_columns, _rows);

            OnUpdate += OutputTableToConsole;
            OnUpdate(content);
        }

        public void KeyDown(ConsoleKey key)
        {
            bool update = false;
            switch (key)
            {
                /*case ConsoleKey.LeftArrow:
                    _cursorPosition--;
                    
                    break;

                case ConsoleKey.RightArrow:
                    _cursorPosition++;
                    break;

                case ConsoleKey.DownArrow:
                    _cursorPosition += _columns;
                    break;

                case ConsoleKey.UpArrow:
                    _cursorPosition -= _columns;
                    break;*/

                case ConsoleKey.Backspace:
                    if (_cursorPosition == 0)
                        break;

                    _cursorPosition--;
                    _pieceTable.Remove(_cursorPosition, 1);
                    update = true;
                    break;

                case ConsoleKey.Delete:
                    _pieceTable.Remove(_cursorPosition, 1);
                    update = true;
                    break;
            }

            int maxLength = _pieceTable.ToString().Length;
            if (_cursorPosition < 0)
                _cursorPosition = 0;
            if (_cursorPosition > maxLength)
                _cursorPosition = maxLength;
            

            Console.SetCursorPosition(_cursorPosition % Console.WindowWidth, _cursorPosition / Console.WindowWidth);

            if (update)
                OnUpdate(_pieceTable.ToString());
        }

        // public int CursorX => _cursorPosition % Console.

        public void KeyPress(char key)
        {
            if (!char.IsControl(key))
                _pieceTable.Insert(new string(key, 1), _cursorPosition++);
            else if (key == '\r')
                _pieceTable.Insert(new string('\n', 1), _cursorPosition++);
            else
                return;

            OnUpdate(_pieceTable.ToString());
        }

        public void KeyUp(ConsoleKey key)
        {
            
        }

        public void MousePress(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void MouseMove(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void MouseRelease(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void SetCursorPosition(int position)
        {
            _cursorPosition = position;
            Console.SetCursorPosition(_cursorPosition % Console.WindowWidth, _cursorPosition / Console.WindowWidth);
        }

        private void OutputTableToConsole(string data)
        {
            Console.Clear();
            Console.Write(data);
            Console.SetCursorPosition(_cursorPosition % Console.WindowWidth, _cursorPosition / Console.WindowWidth);
        }
    }
}
