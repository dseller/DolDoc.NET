using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using DolDoc.Editor.Input;
using DolDoc.Editor.Rendering;
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

        private Document _document;

        private bool _cursorInverted;
        private IFrameBuffer _frameBuffer;
        private byte[] _renderBuffer;

        public ViewerState(IFrameBuffer frameBuffer, Document doc, int width, int height)
        {
            _document = doc;
            _frameBuffer = frameBuffer;
            _renderBuffer = new byte[width * height];
            Rows = doc.Rows;
            Width = width;
            Height = height;
            Columns = doc.Columns;
            ViewLine = 0;
            _cursorInverted = false;

            RawMode = false;

            doc.OnUpdate += Document_OnUpdate;

            Pages = new CharacterPageDirectory(Columns, Rows);
        }

        private void Document_OnUpdate(Document document)
        {
            var ctx = new EntryRenderContext
            {
                Document = document,
                ForegroundColor = DefaultForegroundColor,
                BackgroundColor = DefaultBackgroundColor,
                DefaultBackgroundColor = DefaultBackgroundColor,
                DefaultForegroundColor = DefaultForegroundColor,
                RenderPosition = 0,
                State = this
            };

            if (!RawMode)
            {
                foreach (var entry in document.Entries)
                {
                    var result = entry.Evaluate(ctx);
                    if (result.RefreshRequested)
                    {
                        document.Refresh();
                        return;
                    }

                    ctx.RenderPosition += result?.WrittenCharacters ?? 0;
                }
            }
            else
                DocumentEntry.CreateTextCommand(0, new Flag[0], document.ToPlainText()).Evaluate(ctx);

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
                    if (!Pages[CursorPosition + 1].HasEntry)
                    {
                        do
                        {
                            // TODO: endoffile is not the correct position in the character buffer, it's the raw text position EOF.
                            /*if (CursorPosition >= _endOfFile)
                                break;*/

                            CursorPosition++;
                        } while (!Pages[CursorPosition].HasEntry);
                    }
                    else
                        CursorPosition++;

                    RenderCursor();
                    break;

                case ConsoleKey.LeftArrow:
                    if (CursorPosition == 0)
                        break;

                    if (!Pages[CursorPosition - 1].HasEntry && CursorY > 0)
                    {
                        do
                        {
                            if (CursorPosition <= 0)
                                break;

                            CursorPosition--;
                        } while (!Pages[CursorPosition].HasEntry);
                    }
                    else
                        CursorPosition--;

                    if (CursorPosition < 0)
                        CursorPosition = 0;

                    RenderCursor();
                    break;

                case ConsoleKey.DownArrow:
                    CursorPosition += Columns;

                    if (!Pages[CursorPosition].HasEntry)
                    {

                        do
                        {
                            // TODO: endoffile is not the correct position in the character buffer, it's the raw text position EOF.
                            //if (CursorPosition >= _endOfFile)
                            //{
                            //    CursorPosition = _endOfFile;
                            //    break;
                            //}

                            CursorPosition--;
                        } while (!Pages[CursorPosition].HasEntry);
                    }

                    RenderCursor();
                    break;

                case ConsoleKey.UpArrow:
                    if (CursorPosition - Columns < 0)
                        break;

                    CursorPosition -= Columns;

                    if (!Pages[CursorPosition].HasEntry)
                    {

                        do
                        {
                            if (CursorPosition <= 0)
                                break;

                            // TODO: endoffile is not the correct position in the character buffer, it's the raw text position EOF.
                            //if (CursorPosition >= _endOfFile)
                            //{
                            //    CursorPosition = _endOfFile;
                            //    break;
                            //}

                            CursorPosition--;
                        } while (!Pages[CursorPosition].HasEntry);
                    }

                    if (CursorPosition < 0)
                        CursorPosition = 0;

                    RenderCursor();
                    break;
            }

            var ch = Pages[CursorPosition];
            if (ch.Entry != null)
                ch.Entry.KeyPress(this, key, ch.RelativeTextOffset);
            _document.Refresh();
        }

        public void KeyPress(char key)
        {
            /*if (!char.IsControl(key) || key == '\r')
            {*/
            var ch = Pages[CursorPosition];
            // Get the entry of the current cursor position.
            ch.Entry.CharKeyPress(this, key, ch.RelativeTextOffset);
            _document.Refresh();
            //}
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
            CursorX = x / 8;
            CursorY = y / 8;

            if (Pages[CursorPosition].HasEntry)
                Pages[CursorPosition].Entry.Click();

            _document.Refresh();
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

        public void Render()
        {
            Array.Clear(_renderBuffer, 0, _renderBuffer.Length);

            for (int y = 0; y < Rows; y++)
                for (int x = 0; x < Columns; x++)
                    RenderCharacter(x, y, Pages[x, y + ViewLine]);

            _frameBuffer?.Render(_renderBuffer);
        }

        private void RenderCursor()
        {
            for (int fx = 0; fx < 8; fx++)
                for (int fy = 0; fy < 8; fy++)
                    _renderBuffer[((((CursorY * 8) + fy) * Width) + (CursorX * 8) + fx)] ^= 0x0F;

            _frameBuffer.RenderPartial(CursorX * 8, CursorY * 8, 8, 8, _renderBuffer);
        }

        private void DoBlink(bool inverted)
        {
            for (int row = 0; row < Rows; row++)
                for (int column = 0; column < Columns; column++)
                {
                    var ch = Pages[column, row];
                    if ((ch.Flags & CharacterFlags.Blink) == CharacterFlags.Blink)
                        RenderCharacter(column, row, ch, inverted);
                }

            _frameBuffer.Render(_renderBuffer);
        }

        private void RenderCharacter(int column, int row, Character ch, bool inverted = false)
        {
            if ((ch.Flags & CharacterFlags.Inverted) == CharacterFlags.Inverted)
                inverted = true;

            var bg = inverted ? ch.Color.Foreground : ch.Color.Background;
            var fg = inverted ? ch.Color.Background : ch.Color.Foreground;
            var character = SysFont.Font[ch.Char];
            for (int fx = 0; fx < 8; fx++)
                for (int fy = 0; fy < 8; fy++)
                {
                    bool draw = ((character >> ((fy * 8) + fx)) & 0x01) == 0x01;
                    _renderBuffer[(((row * 8) + fy) * Width) + (column * 8) + fx + ch.ShiftX] = draw ? (byte)fg : (byte)bg;
                }

            if ((ch.Flags & CharacterFlags.Underline) == CharacterFlags.Underline)
            {
                for (int i = 0; i < 8; i++)
                    _renderBuffer[(((row * 8) + (8 - 1)) * Width) + (column * 8) + i] = (byte)fg;
            }
        }

        public void Tick()
        {
            DoBlink(!_cursorInverted);
            RenderCursor();

            _cursorInverted = !_cursorInverted;
        }
    }
}
