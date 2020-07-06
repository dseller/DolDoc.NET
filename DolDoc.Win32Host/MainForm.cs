using DolDoc.Editor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DolDoc.Win32Host
{
    public partial class MainForm : Form, IFrameBuffer
    {
        private Bitmap _bmp;
        private Document _document;
        private string _providedFileName;
        private ViewerState _viewerState;

        public MainForm()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
                _providedFileName = args[1];

            InitializeComponent();
        }

        private static ColorPalette PatchPalette(ColorPalette palette)
        {
            palette.Entries[0] = Color.FromArgb(0x00, 0x00, 0x00);
            palette.Entries[1] = Color.FromArgb(0x00, 0x00, 0xAA);
            palette.Entries[2] = Color.FromArgb(0x00, 0xAA, 0x00);
            palette.Entries[3] = Color.FromArgb(0x00, 0xAA, 0xAA);
            palette.Entries[4] = Color.FromArgb(0xAA, 0x00, 0x00);
            palette.Entries[5] = Color.FromArgb(0xAA, 0x00, 0xAA);
            palette.Entries[6] = Color.FromArgb(0xAA, 0x55, 0x00);
            palette.Entries[7] = Color.FromArgb(0xAA, 0xAA, 0xAA);
            palette.Entries[8] = Color.FromArgb(0x55, 0x55, 0x55);
            palette.Entries[9] = Color.FromArgb(0x55, 0x55, 0xFF);
            palette.Entries[10] = Color.FromArgb(0x55, 0xFF, 0x55);
            palette.Entries[11] = Color.FromArgb(0x55, 0xFF, 0xFF);
            palette.Entries[12] = Color.FromArgb(0xFF, 0x55, 0x55);
            palette.Entries[13] = Color.FromArgb(0xFF, 0x55, 0xFF);
            palette.Entries[14] = Color.FromArgb(0xFF, 0xFF, 0x55);
            palette.Entries[15] = Color.FromArgb(0xFF, 0xFF, 0xFF);
            return palette;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _bmp = new Bitmap(640, 480, PixelFormat.Format8bppIndexed);
            _bmp.Palette = PatchPalette(_bmp.Palette);
            uxImage.Width = 640;
            uxImage.Height = 480;
            uxImage.Image = _bmp;
            WriteBackground(_bmp);

            timer.Tick += Tick;

            Focus();
            ActiveControl = null;

            if (_providedFileName != null)
                LoadFile(File.Open(_providedFileName, FileMode.Open));
        }

        private void Tick(object sender, EventArgs e)
        {
            _viewerState.Tick();
        }

        public void Render(byte[] data)
        {
            timer.Enabled = false;

            var bmpData = _bmp.LockBits(new Rectangle(0, 0, _bmp.Width, _bmp.Height), ImageLockMode.ReadOnly, _bmp.PixelFormat);
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            _bmp.UnlockBits(bmpData);

            uxImage.Refresh();

            Application.DoEvents();
            timer.Enabled = true;

            // debug stuff
            DebugDump();
        }

        public void RenderPartial(int x, int y, int width, int height, byte[] data)
        {
            var bmpData = _bmp.LockBits(new Rectangle(x, y, width, height), ImageLockMode.ReadOnly, _bmp.PixelFormat);
            
            for (int dstY = 0; dstY < height; dstY++)
                Marshal.Copy(data, x + ((dstY + y) * 640), bmpData.Scan0 + (dstY * bmpData.Stride), width);
            
            _bmp.UnlockBits(bmpData);
            uxImage.Refresh();
            Application.DoEvents();
        }

        private void DebugDump()
        {
            uxDebugView.Clear();
            uxDebugView.Text = $@"Cursor:
DocumentCursor: {_viewerState.Cursor.DocumentX},{_viewerState.Cursor.DocumentY} ({_viewerState.CursorPosition})
WindowX: {_viewerState.Cursor.WindowX}
WindowY: {_viewerState.Cursor.WindowY}
ViewLine: {_viewerState.Cursor.ViewLine}
CurrentEntry: {_viewerState.Cursor.SelectedEntry?.ToString()}

Char info:
  Char: {_viewerState.Pages[_viewerState.CursorPosition].Char}
  Page: {_viewerState.Pages.GetOrCreatePage(_viewerState.CursorPosition).PageNumber}
";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.InitialDirectory = Environment.CurrentDirectory;
            fd.Filter = "DolDoc Documents|*.DD";
            var result = fd.ShowDialog();
            if (result.HasFlag(DialogResult.Cancel))
                return;

            var stream = fd.OpenFile();
            if (stream == null)
                return;

            Text = Path.GetFileName(fd.FileName);

            LoadFile(stream);            
        }

        private void LoadFile(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();
                _document = new Document(content, defaultFgColor: EgaColor.White);
                _viewerState = new ViewerState(this, _document, 640, 480);
                _viewerState.Pages.Clear();
                _document.Refresh();
            }
        }

        private void WriteBackground(Bitmap g)
        {
            var data = g.LockBits(new Rectangle(0, 0, g.Width, g.Height), ImageLockMode.ReadOnly, g.PixelFormat);

            var ptr = data.Scan0;
            byte[] frameBuffer = new byte[data.Width * data.Height];

            frameBuffer[0] = (byte)ConsoleColor.DarkGray;
            for (int i = 1; i < data.Width * data.Height; i++)
            {
                if ((i % data.Width) == 0)
                    frameBuffer[i] = frameBuffer[i - 1];
                else
                    frameBuffer[i] = frameBuffer[i - 1] == (byte)ConsoleColor.DarkGray ? (byte)ConsoleColor.Gray : (byte)ConsoleColor.DarkGray;
            }

            Marshal.Copy(frameBuffer, 0, ptr, frameBuffer.Length);
            g.UnlockBits(data);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            timer.Enabled = false;

            var translation = new Dictionary<Keys, ConsoleKey>
            {
                { Keys.Down, ConsoleKey.DownArrow},
                { Keys.Right, ConsoleKey.RightArrow },
                { Keys.Left, ConsoleKey.LeftArrow },
                { Keys.Up, ConsoleKey.UpArrow },
                { Keys.Back, ConsoleKey.Backspace },
                { Keys.Delete, ConsoleKey.Delete },
                { Keys.Home, ConsoleKey.Home },
                { Keys.PageUp, ConsoleKey.PageUp },
                { Keys.PageDown, ConsoleKey.PageDown }
            };

            if (translation.TryGetValue(e.KeyCode, out var key))
            {
                // _editorState.KeyDown(key);
                _viewerState.KeyDown(key);
            }

            timer.Enabled = true;

            DebugDump();
        }

        private void toggleRawModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewerState.RawMode = !_viewerState.RawMode;
            _viewerState.Pages.Clear();
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            timer.Enabled = false;
            _viewerState.KeyPress(e.KeyChar);
            timer.Enabled = true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _document = new Document();
            _viewerState = new ViewerState(this, _document, 640, 480);
        }

        private void uxImage_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Click on {0}x{1}", e.X, e.Y);
            _viewerState.MousePress(e.X, e.Y);
        }

        private void redrawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _document?.Refresh();
        }

        private void uxImage_MouseMove(object sender, MouseEventArgs e)
        {
            _viewerState.MouseMove(e.X, e.Y);

            var pos = ((e.X / 8) + (((e.Y / 8) + _viewerState.Cursor.ViewLine) * _viewerState.Columns));
            if (_viewerState.Pages[pos].HasEntry &&
                _viewerState.Pages[pos].Entry.Clickable &&
                System.Windows.Forms.Cursor.Current != Cursors.Hand)
                System.Windows.Forms.Cursor.Current = Cursors.Hand;
            else
                System.Windows.Forms.Cursor.Current = Cursors.Default;
        }
    }
}
