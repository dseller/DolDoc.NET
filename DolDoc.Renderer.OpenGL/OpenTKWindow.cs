using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using DolDoc.Editor.Compositor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using DolDoc.Editor.Rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;

namespace DolDoc.Renderer.OpenGL
{
    public class Wnd : GameWindow
    {
        private readonly Compositor compositor;
        private readonly IFrameBufferWindow frameBuffer;

        public Wnd(Compositor compositor, IFrameBufferWindow frameBuffer, GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            this.compositor = compositor;
            this.frameBuffer = frameBuffer;
        }

        protected override void OnLoad()
        {
            GL.ReadBuffer(ReadBufferMode.Back);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            GL.ClearColor(Color.White);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.LineWidth(2.5f);

            GL.MatrixMode(MatrixMode.Projection);
            GL.Ortho(0, Size.X, 0, Size.Y, -1, 1);
            GL.Viewport(0, 0, Size.X, Size.Y);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            var spritesToRender = new List<(Sprite, Window, int, int)>();

            for (int y = 0; y < compositor.Rows; y++)
            for (int x = 0; x < compositor.Columns; x++)
            {
                var (ch, window) = compositor.Get(x, y);
                EgaColorRgbBitmap fg, bg;
                if ((ch.Flags & CharacterFlags.Inverted) == CharacterFlags.Inverted)
                {
                    fg = EgaColorRgbBitmap.Palette[(byte) ((byte) ch.Color.Foreground ^ 0x0F)];
                    bg = EgaColorRgbBitmap.Palette[(byte) ((byte) ch.Color.Background ^ 0x0F)];
                }
                else
                {
                    fg = EgaColorRgbBitmap.Palette[(byte) ch.Color.Foreground];
                    bg = EgaColorRgbBitmap.Palette[(byte) ch.Color.Background];
                }

                GL.Color3(bg.RD, bg.GD, bg.BD);
                GL.WindowPos2(x * compositor.Font.Width, Size.Y - ((y + 1) * compositor.Font.Height));
                GL.Bitmap(compositor.Font.Width, compositor.Font.Height, 0, 0, 0, 0, compositor.FilledBitmap);

                GL.Color3(fg.RD, fg.GD, fg.BD);
                GL.WindowPos2(x * compositor.Font.Width, Size.Y - ((y + 1) * compositor.Font.Height));

                GL.Bitmap(compositor.Font.Width, compositor.Font.Height,
                    0, 0,
                    0, //(chCounter % state.Columns == 0 ? -((state.Columns - 1) * state.Font.Width) : state.Font.Width), 
                    0, //(chCounter % state.Columns == 0) ? -state.Font.Height : 0, 
                    compositor.Font[ch.Char]);

                if ((ch.Flags & CharacterFlags.Underline) == CharacterFlags.Underline)
                    GL.Bitmap(compositor.Font.Width, compositor.Font.Height, 0, 0, 0, 0, compositor.UnderlineBitmap);
                if (ch.Entry is Sprite s)
                    spritesToRender.Add((s, window, x, y));
            }

            foreach (var s in spritesToRender)
                s.Item1.SpriteObj.Render(compositor, s.Item2.State, s.Item3 * compositor.Font.Width, s.Item4 * compositor.Font.Height);

            Context.SwapBuffers();
            base.OnRenderFrame(args);
            Thread.Sleep(1);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            compositor.CloseDocument(false);
            e.Cancel = true;
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            var ch = KeyMap.GetKey(e);
            if (!ch.HasValue)
                return;
            compositor.KeyPress(ch.Value);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            var cursor = compositor.MouseMove(e.X, e.Y);
            frameBuffer.SetCursorType(cursor);
        } 
        protected override void OnMouseDown(MouseButtonEventArgs e) => compositor.MouseDown(MousePosition.X, MousePosition.Y);

        protected override void OnMouseUp(MouseButtonEventArgs e) => compositor.MouseUp();
    }

    public class OpenTKWindow : IFrameBufferWindow
    {
        private Wnd window;
        private Timer timer;
        private Thread thread;

        public void Show(string title, int width, int height)
        {
            Width = width;
            Height = height;

            thread = new Thread(() =>
            {
                Debug.WriteLine("Opening OpenTKWindow!");

                GLFWProvider.CheckForMainThread = false;
                var settings = new GameWindowSettings();
                settings.RenderFrequency = 1;
                var nativeSettings = new NativeWindowSettings
                {
                    Size = new Vector2i(width, height),
                    Profile = ContextProfile.Compatability,
                    WindowBorder = WindowBorder.Fixed,
                    IsEventDriven = true,
                    Title = title
                };

                using (window = new Wnd(Compositor, this, settings, nativeSettings))
                {
                    ulong ticks = 0;
                    timer = new Timer(_ => Compositor.Tick(ticks++, false), null, 0, 1000 / 30);

                    window.VSync = VSyncMode.On;
                    window.RenderFrequency = 30;
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
                CursorType.Move => MouseCursor.Crosshair,
                CursorType.IBeam => MouseCursor.IBeam, 
                _ => MouseCursor.Default
            };
        }

        public int? Width { get; private set; }
        public int? Height { get; private set; }

        public Compositor Compositor { get; set; }
    }
}