using Antlr.Runtime;
using DolDoc.Core.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DolDoc.Core.Editor
{
    public class Document
    {
        private Character[] _data;
        public int Columns, Rows, CursorX, CursorY;
        private int _maxY;
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
            _data = new Character[Rows * Columns];
            Clear(backgroundColor);
        }

        public void Load(Stream stream)
        {
            Clear(_defaultBgColor);

            var lexer = new doldocLexer(new ANTLRInputStream(stream));
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new doldocParser(tokenStream);
            var nodes = parser.document();

            var nodesFixed = FixStrings(nodes);

            //var interpreter = new DolDocInterpreter(this, stream);
            //interpreter.OnClear += () => Clear(_bgColor);
            //interpreter.OnWriteCharacter += ch => OnWriteCharacter(ch, CharacterFlags.None);
            //interpreter.OnWriteString += OnWriteString;
            //interpreter.OnWriteLink += OnWriteLink;
            //interpreter.OnForegroundColor += color => _fgColor = color ?? _defaultFgColor;
            //interpreter.OnBackgroundColor += color => _bgColor = color ?? _defaultBgColor;
            //interpreter.Run();
            CursorX = 0;
            CursorY = 0;
            OnUpdate();
        }

        private static IEnumerable<DocumentNode> FixStrings(IEnumerable<DocumentNode> nodes)
        {
            StringBuilder builder = new StringBuilder();
            List<DocumentNode> result = new List<DocumentNode>();
            
            foreach (var node in nodes)
            {
                if (node is StringNode chNode)
                    builder.Append(chNode.Value);
                else if (node is Command cmd)
                {
                    if (builder.Length > 0)
                    {
                        var str = new StringNode(builder.ToString());
                        builder.Clear();
                        result.Add(str);
                    }

                    result.Add(node);
                }
            }

            if (builder.Length > 0)
            {
                var str = new StringNode(builder.ToString());
                builder.Clear();
                result.Add(str);
            }

            return result;
        }

        private void OnWriteLink(DolDocInstruction<string> obj)
        {
            var oldColor = _fgColor;

            _fgColor = EgaColor.Red;
            obj.Flags |= CharacterFlags.Underline;
            OnWriteString(obj);
            _fgColor = oldColor;
        }

        //public void Write(int x, int y, CharacterFlags flags, char ch, EgaColor fgColor, EgaColor bgColor)
        //{
        //    var color = ((byte)fgColor << 4) | (byte)bgColor;
        //    _data[(y * Columns) + x] = new Character((byte)ch, (byte)color, flags);
        //}

        public Character Read(int x, int y) => _data[(y * Columns) + x];

        public void Clear(EgaColor color)
        {
            // Y = X * Columns
            var rows = _data.Length / Columns;

            for (int column = 0; column < Columns; column++)
                for (int row = 0; row < rows; row++)
                {
                    var ch = Read(column, row);
                    if ((ch.Flags & CharacterFlags.Hold) == CharacterFlags.Hold)
                        continue;

                    //Write(column, row, CharacterFlags.None, (char)0, _fgColor, color);
                }
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
            bool update = false;

            if (x < 0)
                x = 0;

            if (y < 0)
            {
                if (ViewLine > 0)
                {
                    ViewLine--;
                    update = true;
                }

                y = 0;
            }


            if (y >= Rows)
            {
                ViewLine += (Rows - y) + 1;
                y = Rows - 1;
                update = true;
            }

            CursorX = x;
            CursorY = y;
            if (update)
                OnUpdate();
        }

        private void OnWriteString(DolDocInstruction<string> data)
        {
            if ((data.Flags & CharacterFlags.Center) == CharacterFlags.Center)
                CursorX = (Columns / 2) - (data.Data.Length / 2);
            else if ((data.Flags & CharacterFlags.Right) == CharacterFlags.Right)
                CursorX = Columns - data.Data.Length;

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
                //    OnWriteCharacter(ch, data.Flags);
        }

        public void WriteCharacter(Character c, int x, int y)
        {
            if (!char.IsControl(c.Char))
            {
                //Write(CursorX, CursorY, flags, obj, _fgColor, _bgColor);
                //var color = ((byte)fgColor << 4) | (byte)bgColor;

                _data[(y * Columns) + x] = c;

                CursorX++;
            }
            else
            {
                //if (obj == '\n')
                //{
                //    CursorY++;
                //    CursorX = 0;
                //}
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

                //for (int i = 0; i < Columns * Rows; i++)
                //    _data[oldLength + i] = new Character(0x00, (byte)_bgColor, CharacterFlags.None);
            }

            _maxY = Math.Max(_maxY, CursorY);
        }
    }
}
