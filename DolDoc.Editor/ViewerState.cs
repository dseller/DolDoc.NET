using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using DolDoc.Editor.Fonts;
using DolDoc.Editor.Input;
using DolDoc.Editor.Rendering;
using System;
using System.IO;

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
        private byte[] _renderBuffer;
        private bool _cursorInverted;
        private IFrameBufferWindow _frameBuffer;
        private readonly IFontProvider _fontProvider;

        public ViewerState(IFrameBufferWindow frameBuffer, Document doc, int width, int height, IFontProvider fontProvider = null, string font = null)
        {
            Cursor = new Cursor(this);
            Document = doc;
            _cursorInverted = false;
            _frameBuffer = frameBuffer;
            _renderBuffer = new byte[width * height];
            _fontProvider = fontProvider ?? new TempleOSFontProvider();
            Font = _fontProvider.Get(font);

            Width = width;
            Height = height;
            Rows = height / Font.Height;
            Columns = width / Font.Width;

            RawMode = false;

            doc.OnUpdate += Document_OnUpdate;

            Pages = new CharacterPageDirectory(Columns, Rows);
        }

        public void LoadDocument(Document document, bool child = false)
        {
            if (child)
                document.Parent = Document;

            _frameBuffer?.Clear();
            Pages.Clear();
            Document = document;
            Document.OnUpdate += Document_OnUpdate;
            Cursor.SetPosition(0);
            Document_OnUpdate(Document, false);
            Render();
        }

        private void Document_OnUpdate(Document document, bool clear)
        {
            if (clear)
            {
                _frameBuffer?.Clear();
                Pages.Clear();
            }

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
                Text.Create(new Flag[0], document.ToPlainText()).Evaluate(ctx);

            Render();
        }

        public CharacterPageDirectory Pages { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Columns { get; }

        public int Rows { get; }

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
            if (key == Key.F12)
            {
                // Write to file.
                var output = Document.ToPlainText();
                var path = $"dump_{Guid.NewGuid().ToString("n")}.dd";
                using (var fs = File.Create(path))
                using (var writer = new StreamWriter(fs))
                    writer.Write(output);

                Console.WriteLine($"Dumped document to {output}");
                return;
            }

            switch (key)
            {
                case Key.ESC:
                    CloseDocument(true);
                    break;

                case Key.SHIFT_ESC:
                    CloseDocument(false);
                    break;

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

        public void MouseMove(float x, float y)
        {
            var entry = FindEntry(x, y);
            if (entry == null)
            {
                _frameBuffer.SetCursorType(CursorType.Pointer);
                return;
            }

            if (entry.Clickable)
                _frameBuffer.SetCursorType(CursorType.Hand);
            else
                _frameBuffer.SetCursorType(CursorType.Pointer);
        }

        public void MouseClick(float x, float y)
        {
            var entry = FindEntry(x, y);
            if (entry == null || !entry.Clickable)
            {
                Cursor.SetPosition((int)((x / Font.Width)) + (((int)(y / Font.Height)) * Columns) + (Cursor.ViewLine * Columns));
            }
            else
            {
                // Perform the click.
                entry.Click(this);
            }

            Document.Refresh();
        }

        public void PreviousPage()
        {
            Cursor.PageUp();
            RenderCursor();
        }

        public void NextPage()
        {
            Cursor.PageDown();
            RenderCursor();
        }

        public void LastPage()
        {
            //_page = _pages.Count - 1;
            //ViewOffset = 

            //ViewLine = _maxY - Rows + 1;
            // Render();
        }

        public void CloseDocument(bool save)
        {
            /*if (save && Document.Path != null)
            {
                using (var fs = File.Open(Document.Path, FileMode.Create))
                using (var writer = new StreamWriter(fs))
                    writer.Write(Document.ToPlainText());
            }*/

            // If there is no parent document, close the whole application.
            if (Document.Parent == null)
                Environment.Exit(0);

            Document = Document.Parent;
            _frameBuffer?.Clear();
            Pages.Clear();
            Cursor.SetPosition(0);
            Document_OnUpdate(Document, false);
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
                    _renderBuffer[(((row * Font.Height) + fy + ch.ShiftY) * Width) + (column * Font.Width) + fx + ch.ShiftX] = draw ? (byte)fg : (byte)bg;
                }

            if ((ch.Flags & CharacterFlags.Underline) == CharacterFlags.Underline)
            {
                for (int i = 0; i < Font.Width; i++)
                    _renderBuffer[(((row * Font.Height) + (Font.Height - 1)) * Width) + (column * Font.Width) + i] = (byte)fg;
            }
        }

        public void Tick(ulong ticks)
        {
            // Blink every 200ms, one frame is 33ms, so every 6 frames
            if (ticks % 6 == 0)
            {
                DoBlink(!_cursorInverted);
                RenderCursor();
                _cursorInverted = !_cursorInverted;
            }
            else if (ticks % 15 == 0)
            {
                Document.Refresh();
                RenderCursor();
            }
        }

        /// <summary>
        /// Find an entry at the specified coordinates.
        /// </summary>
        /// <param name="x">X coordinate (not column, but pixels)</param>
        /// <param name="y">Y coordinate (not row, but pixels)</param>
        /// <returns>The entry, or null if no entry at the specified coordinates.</returns>
        public DocumentEntry FindEntry(float x, float y)
        {
            // Find the entry that is being clicked.
            var column = (int)Math.Floor(x / Font.Width);
            var row = (int)(Math.Floor(y / Font.Height)) + Cursor.ViewLine;

            // Retrieve the entry belonging to the clicked character.
            var ch = Pages[column, row];
            if (!ch.HasEntry)
                return null;

            return ch.Entry;
        }
    }
}
