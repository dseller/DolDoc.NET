using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using DolDoc.Editor.Input;
using DolDoc.Editor.Rendering;
using System;
using System.IO;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Cursor = DolDoc.Editor.Core.Cursor;
using Window = DolDoc.Editor.Compositor.Window;

namespace DolDoc.Editor
{
    /// <summary>
    /// This class represents the state of the viewer. The viewer is what we use to observe the DolDocument.
    /// </summary>
    public class ViewerState : IInputListener, ITickListener
    {
        private readonly Window window;
        public EgaColor DefaultBackgroundColor { get; set; } = EgaColor.White;
        public EgaColor DefaultForegroundColor { get; set; } = EgaColor.Black;
        
        public Document Document { get; private set; }

        public Cursor Cursor { get; }

        private string title;
        private bool cursorInverted;

        public ViewerState(Window window, Document doc, int columns, int rows)
        {
            this.window = window;
            cursorInverted = false;
            Cursor = new Cursor(this);
            Document = doc;

            Width = columns * window.Compositor.Font.Width;
            Height = rows * window.Compositor.Font.Height;
            Rows = rows;
            Columns = columns;

            RawMode = false;

            if (doc != null)
                doc.OnUpdate += Document_OnUpdate;

            Pages = new CharacterPageDirectory(this, Columns, Rows);
        }

        public void LoadDocument(Document document, bool child = false)
        {
            if (child)
                document.Parent = Document;

            Pages.Clear(DefaultBackgroundColor);
            Document = document;
            Document.OnUpdate += Document_OnUpdate;
            Cursor.SetPosition(0);
            Document_OnUpdate(Document, false);
        }

        private void Document_OnUpdate(Document document, bool clear)
        {
            if (clear)
                Pages.Clear(DefaultBackgroundColor);

            var renderOptions = new RenderOptions(DefaultForegroundColor, DefaultBackgroundColor);
            var ctx = new EntryRenderContext(document, this, renderOptions);
            var startPosition = ctx.RenderPosition;

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
            {
                var result = Text.Create(new Flag[0], document.ToPlainText()).Evaluate(ctx);
                ctx.RenderPosition += result?.WrittenCharacters ?? 0;
            }
        }

        public CharacterPageDirectory Pages { get; }

        public int Width { get; }

        public int Height { get; }

        public int Columns { get; }

        public int Rows { get; }

        public int CursorPosition => Cursor.DocumentPosition;

        public CharacterPage CurrentPage => Pages.GetOrCreatePage(CursorPosition);

        public bool RawMode { get; set; }

        public string Title
        {
            get => title;

            set
            {
                title = value;
                window.Title = value;
                // frameBuffer.SetTitle(value);
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
                    Cursor.PageUp();
                    break;

                case Key.PAGE_DOWN:
                    Cursor.PageDown();
                    break;

                case Key.ARROW_RIGHT:
                    Cursor.Right();
                    break;

                case Key.ARROW_LEFT:
                    Cursor.Left();
                    break;

                case Key.ARROW_DOWN:
                    Cursor.Down();
                    break;

                case Key.ARROW_UP:
                    Cursor.Up();
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

        public CursorType MouseMove(float x, float y)
        {
            var entry = FindEntry(x, y);
            if (entry == null)
                return CursorType.Pointer;

            if (entry.IsInput)
                return CursorType.IBeam;
            if (entry.Clickable)
                return CursorType.Hand;

            return CursorType.Pointer;
        }

        public void MouseDown(float x, float y)
        {
            var entry = FindEntry(x, y);
            if (entry == null || !entry.Clickable)
            {
                Cursor.SetPosition((int)((x / window.Compositor.Font.Width)) + (((int)(y / window.Compositor.Font.Height)) * Columns) + (Cursor.ViewLine * Columns));
            }
            else
            {
                // Perform the click.
                entry.Click(this);
            }

            Document.Refresh();
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
            if (save)
                Document.Save();

            // If there is no parent document, close the window.
            if (Document.Parent == null)
                window.Compositor.CloseWindow(window);
            else
            {
                Document = Document.Parent;
                Pages.Clear(DefaultBackgroundColor);
                Cursor.SetPosition(0);
                Document_OnUpdate(Document, false);
            }
        }

        private void DoBlink()
        {
            for (int row = 0; row < Rows; row++)
                for (int column = 0; column < Columns; column++)
                {
                    var ch = Pages[column, row];
                    if ((ch.Flags & CharacterFlags.Blink) == CharacterFlags.Blink)
                        ch.Flags ^= CharacterFlags.Inverted;
                }
        }
        
        private void RenderCursor() => Pages[Cursor.DocumentPosition].Flags ^= CharacterFlags.Inverted;

        public void Tick(ulong ticks)
        {
            // Blink every 200ms, one frame is 33ms, so every 6 frames
            if (ticks % 15 == 0)
            {
                DoBlink();
                RenderCursor();
                cursorInverted = !cursorInverted;
                GLFW.PostEmptyEvent();
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
            var column = (int)Math.Floor(x / window.Compositor.Font.Width);
            var row = (int)(Math.Floor(y / window.Compositor.Font.Height)) + Cursor.ViewLine;

            // Retrieve the entry belonging to the clicked character.
            var ch = Pages[column, row];
            if (!ch.HasEntry)
                return null;

            return ch.Entry;
        }
    }
}
