using DolDoc.Core.Editor;
using DolDoc.Editor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DolDoc.Win32Host
{
    public partial class MainForm : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private bool _tock;
        private Bitmap _bmp;
        private string[] _args;
        private Document _document;

        private EditorState _editorState;

        public MainForm(string[] args)
        {
            _args = args;
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
            AllocConsole();

            _editorState = new EditorState(80, 60);

            _bmp = new Bitmap(640, 480, PixelFormat.Format8bppIndexed);
            _bmp.Palette = PatchPalette(_bmp.Palette);
            uxImage.Width = 640;
            uxImage.Height = 480;
            uxImage.Image = _bmp;
            WriteBackground(_bmp);

            _document = new Document(80, 60, EgaColor.White, EgaColor.Black);
            _document.OnUpdate += RenderDocument;

            timer.Tick += Tick;

            Focus();
            ActiveControl = null;

            if (_args != null && _args.Length > 0)
                LoadFile(File.Open(_args[0], FileMode.Open));
        }

        private void Tick(object sender, EventArgs e)
        {
            InvertCursor(_tock);

            _tock = !_tock;
        }

        private void InvertCursor(bool tock)
        {
            var data = _bmp.LockBits(new Rectangle(0, 0, _bmp.Width, _bmp.Height), ImageLockMode.ReadOnly, _bmp.PixelFormat);
            var bitmap = new byte[_bmp.Width * _bmp.Height];
            Marshal.Copy(data.Scan0, bitmap, 0, bitmap.Length);

            var ch = _document.Read(_document.CursorX, _document.ViewLine + _document.CursorY);
            var character = SysFont.Font[ch.Char & 0xFF];

            var fg = tock ? (byte)((ch.Color >> 4) & 0x0F) : (byte)(ch.Color & 0x0F);
            var bg = tock ? (byte)(ch.Color & 0x0F) : (byte)((ch.Color >> 4) & 0x0F);

            for (int fx = 0; fx < 8; fx++)
                for (int fy = 0; fy < 8; fy++)
                {
                    bool draw = ((character >> ((fy * 8) + fx)) & 0x01) == 0x01;
                    bitmap[(((_document.CursorY * 8) + fy) * _bmp.Width) + (_document.CursorX * 8) + fx] = draw ? fg : bg;
                }

            Marshal.Copy(bitmap, 0, data.Scan0, bitmap.Length);
            _bmp.UnlockBits(data);
            uxImage.Refresh();
        }

        private void RenderDocument()
        {
            timer.Enabled = false;
            byte[] fb = new byte[_bmp.Width * _bmp.Height];

            for (int y = 0; y < _document.Rows; y++)
                for (int x = 0; x < _document.Columns; x++)
                {
                    var ch = _document.Read(x, y + _document.ViewLine);
                    var character = SysFont.Font[ch.Char];
                    //var color = (byte)((ch >> 8) & 0xFF);

                    for (int fx = 0; fx < 8; fx++)
                        for (int fy = 0; fy < 8; fy++)
                        {
                            bool draw = ((character >> ((fy * 8) + fx)) & 0x01) == 0x01;
                            fb[(((y * 8) + fy) * _bmp.Width) + (x * 8) + fx] = draw ? (byte)((ch.Color >> 4) & 0x0F) : (byte)(ch.Color & 0x0F);
                        }

                    if ((ch.Flags & CharacterFlags.Underline) == CharacterFlags.Underline)
                    {
                        for (int i = 0; i < 8; i++)
                            fb[(((y * 8) + (8 - 1)) * _bmp.Width) + (x * 8) + i] = (byte)((ch.Color >> 4) & 0x0F);
                    }
                }

            var data = _bmp.LockBits(new Rectangle(0, 0, _bmp.Width, _bmp.Height), ImageLockMode.ReadOnly, _bmp.PixelFormat);
            Marshal.Copy(fb, 0, data.Scan0, fb.Length);
            _bmp.UnlockBits(data);

            uxImage.Refresh();

            Application.DoEvents();
            timer.Enabled = true;

            // debug stuff
            uxDebugView.Clear();
            uxDebugView.Text = $@"Cursor {_document.CursorX},{_document.CursorY}

Character info:
  textOffset: {_document.Read(_document.CursorX, _document.CursorY).TextOffset}
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
                _document.Load(content);
                _editorState = new EditorState(80, 60, content);
                _editorState.OnUpdate += data => _document.Load(data, true);
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
            var translation = new Dictionary<Keys, ConsoleKey>
            {
                { Keys.Down, ConsoleKey.DownArrow},
                { Keys.Right, ConsoleKey.RightArrow },
                { Keys.Left, ConsoleKey.LeftArrow },
                { Keys.Up, ConsoleKey.UpArrow },
                { Keys.Back, ConsoleKey.Backspace },
                { Keys.Delete, ConsoleKey.Delete },
                { Keys.Home, ConsoleKey.Home }
            };

            switch (e.KeyCode)
            {
                case Keys.PageUp:
                    _document.PreviousPage();
                    break;

                case Keys.PageDown:
                    _document.NextPage();
                    break;

                case Keys.End:
                    //_document.LastPage();
                    for (int i = _document.CursorX; i < _document.Columns; i++)
                        if (_document.Read(i, _document.CursorY).TextOffset == null)
                        {
                            _document.SetCursor(i, _document.CursorY);
                            _editorState.SetCursorPosition(_document.Read(i - 1, _document.CursorY).TextOffset.Value + 1);
                            break;
                        }
                    break;

                case Keys.Home:
                    RenderDocument();
                    _document.SetCursor(0, _document.CursorY);
                    _editorState.SetCursorPosition((_document.Read(0, _document.CursorY).TextOffset ?? 0));
                    InvertCursor(false);
                    break;

                case Keys.Left:
                    RenderDocument();
                    _document.SetCursor(_document.CursorX - 1, _document.CursorY);
                    _editorState.SetCursorPosition(_document.Read(_document.CursorX, _document.CursorY).TextOffset.Value);
                    InvertCursor(false);
                    break;

                case Keys.Right:
                    RenderDocument();
                    _document.SetCursor(_document.CursorX + 1, _document.CursorY);
                    _editorState.SetCursorPosition(_document.Read(_document.CursorX, _document.CursorY).TextOffset.Value);
                    InvertCursor(false);
                    break;

                case Keys.Up:
                    RenderDocument();
                    _document.SetCursor(_document.CursorX, _document.CursorY - 1);
                    _editorState.SetCursorPosition(_document.Read(_document.CursorX, _document.CursorY).TextOffset.Value);
                    InvertCursor(false);
                    break;

                case Keys.Down:
                    RenderDocument();
                    _document.SetCursor(_document.CursorX, _document.CursorY + 1);
                    _editorState.SetCursorPosition(_document.Read(_document.CursorX, _document.CursorY).TextOffset.Value);
                    InvertCursor(false);
                    break;
            }

            if (translation.TryGetValue(e.KeyCode, out var key))
                _editorState.KeyDown(key);
        }

        private void toggleRawModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _document.EnableInterpreter = !_document.EnableInterpreter;
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            _editorState.KeyPress(e.KeyChar);
            _document.SetCursor(_document.CursorX + 1, _document.CursorY);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _editorState = new EditorState(80, 60);
            _editorState.OnUpdate += data => _document.Load(data, true);
            _document.Load(string.Empty, false);
        }
    }
}
