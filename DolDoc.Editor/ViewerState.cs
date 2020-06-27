using DolDoc.Core.Parser;
using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using DolDoc.Editor.Input;
using DolDoc.Editor.Rendering;
using DolDoc.Interpreter.Commands;
using System;

namespace DolDoc.Editor
{
    /// <summary>
    /// This class represents the state of the viewer. The viewer is what we use to observe the DolDocument.
    /// </summary>
    public class ViewerState : IInputListener, ITickListener
    {
        private const EgaColor DefaultBackgroundColor = EgaColor.White;
        private const EgaColor DefaultForegroundColor = EgaColor.Black;

        private bool _cursorInverted;
        private EditorState _editor;
        private IFrameBuffer _frameBuffer;
        private byte[] _renderBuffer;
        private IDolDocParser _parser;

        public ViewerState(IDolDocParser parser, EditorState editor, IFrameBuffer frameBuffer, Document doc, int width, int height, int columns, int rows)
        {
            _editor = editor;
            _parser = parser;
            _frameBuffer = frameBuffer;
            _renderBuffer = new byte[width * height];
            Rows = rows;
            Width = width;
            Height = height;
            Columns = columns;
            ViewLine = 0;
            _cursorInverted = false;

            editor.OnUpdate += Editor_OnUpdate;

            Pages = new CharacterPageDirectory(columns, rows);
        }

        private void Editor_OnUpdate(string obj)
        {
            Console.WriteLine("UPDATE");
            var ctx = new CommandContext
            {
                ForegroundColor = DefaultForegroundColor,
                BackgroundColor = DefaultBackgroundColor,
                DefaultBackgroundColor = DefaultBackgroundColor,
                DefaultForegroundColor = DefaultForegroundColor,
                RenderPosition = 0,
                State = this
            };

            if (!RawMode)
            {
                var commands = _parser.Parse(obj);
                foreach (var command in commands)
                {
                    ctx.Flags = command.Flags;
                    ctx.Arguments = command.Arguments;
                    ctx.TextOffset = command.TextOffset;

                    var result = CommandHelper.Execute(command, ctx);
                    ctx.RenderPosition += result?.WrittenCharacters ?? 0;
                }
            }
            else
            {
                var cmd = Command.CreateTextCommand(0, new Flag[0], obj);
                ctx.Arguments = cmd.Arguments;
                ctx.Flags = cmd.Flags;
                CommandHelper.Execute(cmd, ctx);
            }

            Render();
        }

        public CharacterPageDirectory Pages { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Columns { get; private set; }

        public int Rows { get; private set; }

        public int CursorX { get; private set; }

        public int CursorY { get; private set; }

        public int CursorPosition
        {
            get => CursorX + (CursorY * Columns);
            set
            {
                RenderCursor(false);
                CursorX = value % Columns;
                CursorY = value / Columns;
            }
        }

        public int ViewLine { get; private set; }

        public bool RawMode { get; set; }

        public void KeyDown(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.PageUp:
                    PreviousPage();
                    break;

                case ConsoleKey.PageDown:
                    NextPage();
                    break;

                case ConsoleKey.RightArrow:
                    CursorPosition++;
                    break;

                case ConsoleKey.LeftArrow:
                    CursorPosition--;
                    if (CursorPosition < 0)
                        CursorPosition = 0;
                    break;

                case ConsoleKey.DownArrow:
                    CursorPosition += Columns;
                    break;

                case ConsoleKey.UpArrow:
                    CursorPosition -= Columns;
                    if (CursorPosition < 0)
                        CursorPosition = 0;
                    break;
            }
        }

        public void KeyPress(char key)
        {
            // throw new NotImplementedException();
        }

        public void KeyUp(ConsoleKey key)
        {
            throw new NotImplementedException();
        }

        public void MouseMove(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void MousePress(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void MouseRelease(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void PreviousPage()
        {
            if (ViewLine == 0)
                return;

            ViewLine -= Rows;
            if (ViewLine < 0)
                ViewLine = 0;

            Render();
        }

        public void NextPage()
        {
            /*if (ViewOffset == _pages.Count - 1)
                return;*/

            ViewLine += Rows;
            Render();
        }

        public void LastPage()
        {
            //_page = _pages.Count - 1;
            //ViewOffset = 

            //ViewLine = _maxY - Rows + 1;
            Render();
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

            if (x > Columns)
            {
                y += x / Columns;
                x %= Columns;
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
                Render();
        }

        public void Render()
        {
            Array.Clear(_renderBuffer, 0, _renderBuffer.Length);

            for (int y = 0; y < Rows; y++)
                for (int x = 0; x < Columns; x++)
                {
                    var ch = Pages[x, y + ViewLine];

                    RenderCharacter(x, y, ch);

                    if ((ch.Flags & CharacterFlags.Underline) == CharacterFlags.Underline)
                    {
                        for (int i = 0; i < 8; i++)
                            _renderBuffer[(((y * 8) + (8 - 1)) * Width) + (x * 8) + i] = (byte)((ch.Color >> 4) & 0x0F);
                    }
                }

            _frameBuffer.Render(_renderBuffer);
        }

        private void RenderCursor(bool inverted)
        {
            RenderCharacter(CursorX, CursorY, Pages[CursorX, ViewLine + CursorY], inverted);
            _frameBuffer.RenderPartial(CursorX * 8, CursorY * 8, 8, 8, _renderBuffer);
        }

        private void RenderCharacter(int column, int row, Character ch, bool inverted = false)
        {
            var bg = inverted ? (byte)((ch.Color >> 4) & 0x0F) : (byte)(ch.Color & 0x0F);
            var fg = inverted ? (byte)(ch.Color & 0x0F) : (byte)((ch.Color >> 4) & 0x0F);
            var character = SysFont.Font[ch.Char];
            for (int fx = 0; fx < 8; fx++)
                for (int fy = 0; fy < 8; fy++)
                {
                    bool draw = ((character >> ((fy * 8) + fx)) & 0x01) == 0x01;
                    _renderBuffer[(((row * 8) + fy) * Width) + (column * 8) + fx] = draw ? fg : bg;
                }
        }

        public void Tick()
        {
            RenderCursor(_cursorInverted);

            _cursorInverted = !_cursorInverted;
        }
    }
}
