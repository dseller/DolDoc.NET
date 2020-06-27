using DolDoc.Editor.Core;
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
        private int _columns, _rows;

        public event Action<string> OnUpdate;

        public EditorState(Document doc, int columns, int rows, string content = null)
        {
            Document = doc;
            _rows = rows;
            _columns = columns;
            _pieceTable = new PieceTable(content);

            //Console.SetWindowSize(_columns, _rows);
            //Console.SetBufferSize(_columns, _rows);

            OnUpdate += OutputTableToConsole;
            OnUpdate(content);
        }

        public int CursorPosition { get; private set; }

        public Document Document { get; private set; }

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
                    if (CursorPosition == 0)
                        break;

                    CursorPosition--;
                    _pieceTable.Remove(CursorPosition, 1);
                    update = true;
                    break;

                case ConsoleKey.Delete:
                    _pieceTable.Remove(CursorPosition, 1);
                    update = true;
                    break;
            }

            int maxLength = _pieceTable.ToString().Length;
            if (CursorPosition < 0)
                CursorPosition = 0;
            if (CursorPosition > maxLength)
                CursorPosition = maxLength;
            

            // Console.SetCursorPosition(CursorPosition % Console.WindowWidth, CursorPosition / Console.WindowWidth);

            if (update)
                OnUpdate(_pieceTable.ToString());
        }

        // public int CursorX => _cursorPosition % Console.

        public void KeyPress(char key)
        {
            if (!char.IsControl(key))
                _pieceTable.Insert(new string(key, 1), CursorPosition++);
            else if (key == '\r')
                _pieceTable.Insert(new string('\n', 1), CursorPosition++);
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
            CursorPosition = position;
            Console.SetCursorPosition(CursorPosition % Console.WindowWidth, CursorPosition / Console.WindowWidth);
        }

        private void OutputTableToConsole(string data)
        {
            //Console.Clear();
            //Console.Write(data);
            //Console.SetCursorPosition(CursorPosition % Console.WindowWidth, CursorPosition / Console.WindowWidth);
        }

        public void Kick()
        {
            OnUpdate(_pieceTable.ToString());
        }
    }
}
