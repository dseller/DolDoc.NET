using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using DolDoc.Editor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Fonts;
using DolDoc.Editor.Rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using Serilog;

namespace DolDoc.Renderer.OpenGL
{
    public class Wnd : GameWindow
    {
        private readonly ViewerState state;
        private readonly byte[] filledBitmap, underlineBitmap;

        public Wnd(ViewerState state, GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            this.state = state;
            filledBitmap = Enumerable.Repeat((byte) 0xFF, (state.Font.Width * state.Font.Height) / 8).ToArray();
            underlineBitmap = new byte[(state.Font.Width * state.Font.Height) / 8];
            underlineBitmap[(underlineBitmap.Length - 1)/8] = 0xFF;
        }

        protected override void OnLoad()
        {
            GL.ReadBuffer(ReadBufferMode.Back);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            GL.ClearColor(Color.White);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.LineWidth(2.5f);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.Ortho(0, Size.X, 0, Size.Y, -1, 1);
            GL.Viewport(0, 0, Size.X, Size.Y);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Disable(EnableCap.Lighting);
            
            for (int y = 0; y < state.Rows; y++)
                for (int x = 0; x < state.Columns; x++)
                {
                    if (!state.Pages.HasPageForPosition(x, y + state.Cursor.ViewLine))
                        state.Pages.GetOrCreatePage(x, y + state.Cursor.ViewLine);
            
                    var ch = state.Pages[x, y + state.Cursor.ViewLine]; 
                    EgaColor fg, bg;
                    if ((ch.Flags & CharacterFlags.Inverted) == CharacterFlags.Inverted)
                    {
                        fg = EgaColor.Palette[(byte) ((byte)ch.Color.Foreground ^ 0x0F)];
                        bg = EgaColor.Palette[(byte) ((byte)ch.Color.Background ^ 0x0F)];
                    }
                    else
                    {
                        fg = EgaColor.Palette[(byte) ch.Color.Foreground];
                        bg = EgaColor.Palette[(byte) ch.Color.Background];
                    }

                    GL.Color3(bg.RD, bg.GD, bg.BD);
                    GL.WindowPos2(x * state.Font.Width, Size.Y - ((y + 1) * state.Font.Height));
                    GL.Bitmap(state.Font.Width, state.Font.Height, 0, 0, 0, 0, filledBitmap);

                    GL.Color3(fg.RD, fg.GD, fg.BD);
                    GL.WindowPos2(x * state.Font.Width, Size.Y - ((y + 1) * state.Font.Height));

                    GL.Bitmap(state.Font.Width, state.Font.Height,
                        0, 0,
                        0, //(chCounter % state.Columns == 0 ? -((state.Columns - 1) * state.Font.Width) : state.Font.Width), 
                        0, //(chCounter % state.Columns == 0) ? -state.Font.Height : 0, 
                        state.Font[ch.Char]);

                    if ((ch.Flags & CharacterFlags.Underline) == CharacterFlags.Underline)
                    {
                        GL.Bitmap(state.Font.Width, state.Font.Height, 0, 0, 0, 0, underlineBitmap);
                    }
                }
            
            // GL.Flush();
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            state.CloseDocument(false);
            e.Cancel = true;
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            var ch = KeyMap.GetKey(e);
            if (!ch.HasValue)
                return;

            state.KeyPress(ch.Value);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e) => state.MouseMove(e.X, e.Y);

        protected override void OnMouseDown(MouseButtonEventArgs e) => state.MouseClick(MousePosition.X, MousePosition.Y);
    }

    public class OpenTKWindow : IFrameBufferWindow
    {
        private Wnd window;
        private Timer timer;
        private Thread thread;
        private Document document;

        public void Show(string title, int width, int heigth, Document document = null)
        {
            thread = new Thread(() =>
            {
                Log.Verbose("Opening OpenTKWindow!");

                this.document = document ?? new Document();

                State = new ViewerState(this, this.document, width, heigth, new YaffFontProvider(), "Terminal_VGA_cp861");
                document.Refresh();

                var settings = new GameWindowSettings();
                settings.RenderFrequency = 1;
                var nativeSettings = new NativeWindowSettings
                {
                    Size = new Vector2i(width, heigth),
                    Profile = ContextProfile.Compatability,
                    WindowBorder = WindowBorder.Fixed,
                    IsEventDriven = false,
                    Title = title
                };

                ulong ticks = 0;
                timer = new Timer(_ => State.Tick(ticks++), null, 0, 1000 / 30);

                using (window = new Wnd(State, settings, nativeSettings))
                {
                    window.VSync = VSyncMode.On;
                    window.Run();
                }
            });
            thread.Start();
        }

        public void Clear()
        {
        }

        public void SetTitle(string title)
        {
            if (window == null)
                return;
            window.Title = title;
        }

        public void SetCursorType(CursorType cursorType)
        {
            window.Cursor = cursorType switch
            {
                CursorType.Hand => MouseCursor.Hand,
                _ => MouseCursor.Default
            };
        }

        public ViewerState State { get; private set; }
    }
}