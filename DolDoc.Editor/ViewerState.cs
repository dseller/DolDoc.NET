using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using DolDoc.Editor.Fonts;
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

        public Document Document { get; private set; }

        public IFont Font { get; private set; }

        public Cursor Cursor { get; }

        private string _title;
        private bool _cursorInverted;
        private IFrameBufferWindow _frameBuffer;
        private byte[] _renderBuffer;
        private readonly IFontProvider _fontProvider;

        public ViewerState(IFrameBufferWindow frameBuffer, Document doc, int width, int height, IFontProvider fontProvider = null, string font = null)
        {
            Cursor = new Cursor(this);
            Document = doc;
            _frameBuffer = frameBuffer;
            _renderBuffer = new byte[width * height];
            Rows = doc.Rows;
            Width = width;
            Height = height;
            Columns = doc.Columns;
            _cursorInverted = false;
            _fontProvider = fontProvider ?? new TempleOSFontProvider();
            Font = _fontProvider.Get(font);

            RawMode = false;

            doc.OnUpdate += Document_OnUpdate;

            Pages = new CharacterPageDirectory(Columns, Rows);
        }

        public void LoadDocument(Document document)
        {
            _frameBuffer?.Clear();
            Pages.Clear();
            Document = document;
            Document.OnUpdate += Document_OnUpdate;
            Rows = Document.Rows;
            Columns = Document.Columns;
            Cursor.DocumentPosition = 0;
            Document_OnUpdate(Document);
            Render();
        }

        private void Document_OnUpdate(Document document)
        {
            var renderOptions = new RenderOptions(DefaultForegroundColor, DefaultBackgroundColor);
            var ctx = new EntryRenderContext(document, this, renderOptions);

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
                DocumentEntry.CreateTextCommand(new Flag[0], document.ToPlainText()).Evaluate(ctx);

            Render();
        }

        public CharacterPageDirectory Pages { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Columns { get; private set; }

        public int Rows { get; private set; }

        public int CursorPosition => Cursor.DocumentPosition;

        public bool RawMode { get; set; }

        public string Title
        {
            get => _title;

            set
            {
                _title = value;
                _frameBuffer.SetTitle(value);
            }
        }

        public void KeyPress(Key key)
        {
            switch (key)
            {
                case Key.PAGE_UP:
                    PreviousPage();
                    break;

                case Key.PAGE_DOWN:
                    NextPage();
                    break;

                case Key.ARROW_RIGHT:
                    Cursor.Right();
                    RenderCursor();
                    break;

                case Key.ARROW_LEFT:
                    Cursor.Left();
                    RenderCursor();
                    break;

                case Key.ARROW_DOWN:
                    Cursor.Down();
                    RenderCursor();
                    break;

                case Key.ARROW_UP:
                    Cursor.Up();
                    RenderCursor();
                    break;
            }

            // SPACE to TILDE
            char? character = null;
            if (key >= Key.SPACE && key <= Key.TILDE)
                character = (char)key;

            var ch = Pages[Cursor.DocumentPosition];
            if (ch.HasEntry)
                ch.Entry.KeyPress(this, key, character, ch.RelativeTextOffset);
            Document.Refresh();
        }

        public void MouseMove(int x, int y)
        {

        }

        public void MousePress(int x, int y)
        {
            //CursorX = x / 8;
            //CursorY = y / 8;

            //if (Pages[CursorPositionWithViewlineOffset].HasEntry)
            //    Pages[CursorPositionWithViewlineOffset].Entry.Click();

            //Document.Refresh();
        }

        public void MouseRelease(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void PreviousPage()
        {
            //if (ViewLine == 0)
            //    return;

            //ViewLine -= Rows;
            //if (ViewLine < 0)
            //    ViewLine = 0;

            //Render();
        }

        public void NextPage()
        {
            /*if (ViewOffset == _pages.Count - 1)
                return;*/

            //ViewLine += Rows;
            //Render();
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
                {
                    if (!Pages.HasPageForPosition(x, y + Cursor.ViewLine))
                        Pages.GetOrCreatePage(x, y + Cursor.ViewLine);

                    RenderCharacter(x, y, Pages[x, y + Cursor.ViewLine]);
                }

            // render sprites. this is hacky, but for now it will do.
            foreach (var entry in Document.Entries)
                if (entry is Sprite spriteEntry && spriteEntry.SpriteObj != null)
                    if ((spriteEntry.SpriteOffset * Font.Width * Font.Height) < Width * Height)
                        spriteEntry.SpriteObj.WriteToFrameBuffer(this, _renderBuffer, (spriteEntry.SpriteOffset * Font.Width * Font.Height) - (Cursor.ViewLine * Columns * 8 * 8));

            _frameBuffer?.Render(_renderBuffer);
        }

        private void RenderCursor()
        {
            for (int fx = 0; fx < Font.Width; fx++)
                for (int fy = 0; fy < Font.Height; fy++)
                    _renderBuffer[((((Cursor.WindowY * Font.Height) + fy) * Width) + (Cursor.WindowX * Font.Width) + fx)] ^= 0x0F;

            _frameBuffer.RenderPartial(Cursor.WindowX * Font.Width, Cursor.WindowY * Font.Height, Font.Width, Font.Height, _renderBuffer);
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

            byte[] character = Font[ch.Char];
            const int byteSize = 8;

            for (int fy = 0; fy < Font.Height; fy++)
                for (int fx = 0; fx < Font.Width; fx++)
                {
                    var fontRow = character[(fy * Font.Width) / byteSize];
                    bool draw = ((fontRow >> (fx % byteSize)) & 0x01) == 0x01;
                    _renderBuffer[(((row * Font.Height) + fy) * Width) + (column * Font.Width) + fx + ch.ShiftX] = draw ? (byte)fg : (byte)bg;
                }

            if ((ch.Flags & CharacterFlags.Underline) == CharacterFlags.Underline)
            {
                for (int i = 0; i < Font.Width; i++)
                    _renderBuffer[(((row * Font.Height) + (Font.Height - 1)) * Width) + (column * Font.Width) + i] = (byte)fg;
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
