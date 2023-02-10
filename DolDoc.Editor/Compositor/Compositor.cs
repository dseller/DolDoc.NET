using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DolDoc.Editor.Core;
using DolDoc.Editor.Fonts;
using DolDoc.Editor.Input;
using DolDoc.Editor.Logging;
using DolDoc.Editor.Rendering;

namespace DolDoc.Editor.Compositor
{
    public class Compositor : IInputListener, ITickListener
    {
        private bool isMovingWindow;
        private float windowMoveStart, windowMoveEnd;
        
        private readonly Document logDocument;
        private readonly IFrameBufferWindow frameBuffer;
        private readonly int width;
        private readonly int height;
        public IFont Font;
        public readonly byte[] FilledBitmap, UnderlineBitmap;
        private readonly object syncRoot = new object();
        
        public static Compositor Instance { get; private set; }

        public static Compositor Initialize(IFrameBufferWindow frameBuffer, int width, int height, Document root = null, IFontProvider fontProvider = null, string font = null)
        {
            Instance = new Compositor(frameBuffer, width, height, root, fontProvider, font);
            return Instance;
        }
        
        private Compositor(IFrameBufferWindow frameBuffer, int width, int height, Document root = null, IFontProvider fontProvider = null, string font = null)
        {
            logDocument = new Document();
            Logger = new DocumentLogger(logDocument);
            this.frameBuffer = frameBuffer;
            this.width = width;
            this.height = height;
            this.frameBuffer.Compositor = this;
            Font = (fontProvider ?? new YaffFontProvider(Logger)).Get(font);
            FilledBitmap = Enumerable.Repeat((byte) 0xFF, (Font.Width * Font.Height) / 8).ToArray();
            UnderlineBitmap = new byte[(Font.Width * Font.Height) / 8];
            UnderlineBitmap[(UnderlineBitmap.Length - 1) / 8] = 0xFF;
            
            Windows = new List<Window>();
            Columns = width / Font.Width;
            Rows = height / Font.Height;
            WindowIndexBitmap = new byte[Columns * Rows];
            Root = NewWindow("ROOT", Columns, Rows, 0, 0, root, WindowFlags.IsRoot);
            FocusedWindow = Root;
            Logger.Info("Compositor initialized");
        }
        
        public ILogger Logger { get; }
        
        public Window FocusedWindow { get; private set; }
        
        public List<Window> Windows { get; }
        
        /// <summary>
        /// Contains a list of window indices, one for each character.
        /// </summary>
        public byte[] WindowIndexBitmap { get; }
        
        public int Columns { get; }
        public int Rows  { get; }

        public Window Root { get; }

        public (Character, Window) Get(int x, int y)
        {
            Window window;
            lock (syncRoot)
            {
                var index = WindowIndexBitmap[x + (y * Columns)];
                window = Windows[index];
            }

            var ch = window.GetCharacter(x - window.X, y - window.Y);
            return (ch, window);
        }

        public Window NewWindow(string title, int columns, int rows, int x = 10, int y = 10, Document doc = null, WindowFlags? flags = null)
        {
            var w = new Window(this, title, columns, rows, x, y, doc, flags);
            Windows.Add(w);
            doc?.Refresh();

            lock (syncRoot)
            {
                for (int row = y; row < rows + y; row++)
                    Array.Fill(WindowIndexBitmap, (byte) (Windows.Count - 1), (row * Columns) + x, columns);
            }
            
            if (FocusedWindow != null)
                FocusedWindow.Flags &= ~WindowFlags.HasFocus;
            FocusedWindow = w;
            FocusedWindow.Flags |= WindowFlags.HasFocus;

            return w;
        }

        public void Start()
        {
            frameBuffer.Show("TempleTodo", width, height);
        }

        public void CloseWindow(Window window)
        {
            if (window.IsRoot)
                Environment.Exit(0);
            Windows.Remove(window);
            RecalculateIndices();
        }

        public void CloseDocument(bool save) => FocusedWindow?.State.CloseDocument(save);

        public void KeyPress(Key key)
        {
            if (key == Key.F11)
            {
                NewWindow("Log", Columns - 20, Rows - 20, 10, 10, logDocument);
                
            }
            
            FocusedWindow?.State.KeyPress(key);  
        }

        public void MouseUp()
        {
            isMovingWindow = false;
            windowMoveStart = windowMoveEnd = 0f;
        }

        public void MouseDown(float x, float y)
        {
            var column = (int)Math.Floor(x / Font.Width);
            var row = (int)(Math.Floor(y / Font.Height));

            Window window;
            lock (syncRoot)
            {
                var index = WindowIndexBitmap[column + (row * Columns)];
                window = Windows[index];

                if (!window.IsRoot)
                {
                    Windows.Remove(window);
                    Windows.Add(window);
                    RecalculateIndices();
                }

                FocusedWindow.Flags &= ~WindowFlags.HasFocus;
                FocusedWindow = window;
                FocusedWindow.Flags |= WindowFlags.HasFocus;
            }

            var normalizedX = window.HasBorder ? x - ((window.X + 1) * Font.Width) : x - (window.X * Font.Width);
            var normalizedY = window.HasBorder ? y - ((window.Y + 1) * Font.Height) : y - (window.Y * Font.Height);
            if (normalizedX < 0 || normalizedY < 0 || (normalizedX / Font.Width) > window.Columns || (normalizedY / Font.Height) > window.Rows)
            {
                isMovingWindow = true;
                windowMoveStart = x;
                windowMoveEnd = y;
            }
            else
                window.State.MouseDown(normalizedX, normalizedY);
        }

        public void MouseMove(float x, float y)
        {
            if (isMovingWindow && !FocusedWindow.IsRoot)
            {
                var deltaX = x - windowMoveStart;
                var deltaY = y - windowMoveEnd;
                lock (syncRoot)
                {
                    FocusedWindow.X = (int) ((deltaX + windowMoveStart) / Font.Width);
                    FocusedWindow.Y = (int) ((deltaY + windowMoveEnd) / Font.Height);
                    RecalculateIndices();
                }
            }
            else
            {
                var column = (int)Math.Floor(x / Font.Width);
                var row = (int) Math.Floor(y / Font.Height);
                var index = WindowIndexBitmap[column + (row * Columns)];
                var window = Windows[index];
                if (!window.IsRoot)
                {
                    var asdasdx = "";
                }

                var normalizedX = window.HasBorder ? (x / Font.Width) - ((window.X + 1)) : x - (window.X);
                var normalizedY = window.HasBorder ? (y / Font.Width) - ((window.Y + 1)) : y - (window.Y);
                
                var normalizedColumn = (int)Math.Floor(normalizedX / window.Compositor.Font.Width);
                var normalizedRow = (int)(Math.Floor(normalizedY / window.Compositor.Font.Height)) + window.State.Cursor.ViewLine;
                
                // Retrieve the entry belonging to the clicked character.
                var ch = window.State.Pages[normalizedColumn, normalizedRow];
                if (!ch.HasEntry)
                    frameBuffer.SetCursorType(CursorType.Pointer);
                else if (ch.Entry.Clickable)
                    frameBuffer.SetCursorType(CursorType.Hand);
                else
                    frameBuffer.SetCursorType(CursorType.Pointer);
            }
        }
        
        private void RecalculateIndices()
        {
            lock (syncRoot)
            {
                var rects = new Queue<Rectangle>();
                foreach (var wnd in Windows)
                    rects.Enqueue(new Rectangle(wnd.X, wnd.Y, wnd.Columns, wnd.Rows));

                for (byte counter = 0; rects.Count > 0; counter++)
                {
                    var rect = rects.Dequeue();
                    for (int row = rect.Y; row < rect.Height + rect.Y; row++)
                    {
                        var fillWidth = rect.Width;
                        if (rect.X + fillWidth > Columns)
                            fillWidth -= Columns - rect.X;
                        
                        Array.Fill(WindowIndexBitmap, counter, (row * Columns) + rect.X, fillWidth);
                    }
                }
            }
        }

        public void Tick(ulong ticks)
        {
            List<Window> windows;
            lock (syncRoot)
                windows = new List<Window>(Windows);
            
            foreach (var window in windows)
                window.State.Tick(ticks);
        }
    }
}