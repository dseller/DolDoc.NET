using DolDoc.Editor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Input;
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
        private string _providedFileName;

        private EditorState _editorState;
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
            /*InvertCursor(_tock);
            _tock = !_tock;*/

            /*foreach (var listener in _tickListeners)
                listener.Tick();*/
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
            Marshal.Copy(data, (y * Width) + x, bmpData.Scan0, data.Length);
            _bmp.UnlockBits(bmpData);
            uxImage.Refresh();
            Application.DoEvents();
        }

        private void DebugDump()
        {
            uxDebugView.Clear();
            uxDebugView.Text = $@"ViewerState:
Cursor {_viewerState.CursorX},{_viewerState.CursorY}

EditorState:
Cursor {_editorState.CursorPosition}
";
            // TODO: ABOVE IS WEIRD fix it (using viewerstate cursorX/Y to read from document character matrix)
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
                var document = new Document(80, 60, EgaColor.White, EgaColor.Black);
                document.Load(content);

                _editorState = new EditorState(document, 80, 60, content);
                //_editorState.OnUpdate += data => document.Load(data, true);

                _viewerState = new ViewerState(new Core.Parser.LegacyParser(), _editorState, this, document, 640, 480, 80, 60);
                //_viewerState.Render();
                _editorState.Kick();
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
                { Keys.Home, ConsoleKey.Home },
                { Keys.PageUp, ConsoleKey.PageUp },
                { Keys.PageDown, ConsoleKey.PageDown }
            };

            if (translation.TryGetValue(e.KeyCode, out var key))
            {
                _editorState.KeyDown(key);
                _viewerState.KeyDown(key);
            }


                //_editorState.KeyDown(key);

            /*switch (e.KeyCode)
            {
                

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
                    var cell = _document.Read(_document.CursorX, _document.CursorY);
                    if (cell.TextOffset == null)
                    {
                        // Undo the cursor move if the destination character does not have a text offset.
                        _document.SetCursor(_document.CursorX - 1, _document.CursorY);
                        SystemSounds.Exclamation.Play();
                        return;
                    }

                    _editorState.SetCursorPosition(_document.Read(_document.CursorX, _document.CursorY).TextOffset.Value);
                    InvertCursor(false);
                    break;

                case Keys.Up:
                    RenderDocument();
                    _document.SetCursor(_document.CursorX, _document.CursorY - 1);
                    var upCell = _document.Read(_document.CursorX, _document.CursorY);
                    if (upCell.TextOffset == null)
                    {
                        // Find a correct one
                        var idx = _document.GetLastCharacterOnLine(_document.CursorY);
                        if (!idx.HasValue)
                            throw new Exception("yeahhh...");

                        _document.SetCursor(idx.Value, _document.CursorY);
                    }


                    _editorState.SetCursorPosition(_document.Read(_document.CursorX, _document.CursorY).TextOffset.Value);
                    InvertCursor(false);
                    break;

                case Keys.Down:
                    RenderDocument();
                    _document.SetCursor(_document.CursorX, _document.CursorY + 1);
                    _editorState.SetCursorPosition(_document.Read(_document.CursorX, _document.CursorY).TextOffset.Value);
                    InvertCursor(false);
                    break;

                case Keys.Enter:
                    _document.SetCursor(0, _document.CursorY + 1);
                    break;
            }*/

            

            DebugDump();
        }

        private void toggleRawModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewerState.RawMode = !_viewerState.RawMode;
            _viewerState.Pages.Clear();
            _editorState.Kick();
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            _editorState.KeyPress(e.KeyChar);
            _viewerState.KeyPress(e.KeyChar);

            /*_editorState.KeyPress(e.KeyChar);

            if (!char.IsControl(e.KeyChar))
                _document.SetCursor(_document.CursorX + 1, _document.CursorY);*/
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var document = new Document(80, 60, EgaColor.White, EgaColor.Black);
            document.Load(string.Empty, false);

            _editorState = new EditorState(document, 80, 60);
            _editorState.OnUpdate += data => document.Load(data, true);
            _viewerState = new ViewerState(new Core.Parser.LegacyParser(), _editorState, this, document, 640, 480, 80, 60);
            //_document.Load(string.Empty, false);
        }
    }
}
