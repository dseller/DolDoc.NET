using DolDoc.Editor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Fonts;
using DolDoc.Editor.Rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DolDoc.Renderer.OpenGL
{
    public class OpenGLNativeWindow : IFrameBufferWindow, IDisposable
    {
        private Timer timer;
        private Thread thread;
        private Document document;
        private ViewerState viewerState;
        private EgaColor[] _framebuffer;
        private object framebufferLock = new object();

        private NativeWindow nativeWindow;
        private IGraphicsContext graphicsContext;

        public ViewerState State => viewerState;

        public void Clear() => Array.Fill(_framebuffer, EgaColor.Palette[15]);

        public void Render(byte[] data)
        {
            lock (framebufferLock)
                for (int i = 0; i < data.Length; i++)
                    _framebuffer[i] = EgaColor.Palette[data[i]];
        }

        public void RenderPartial(int x, int y, int width, int height, byte[] data)
        {
        }

        public void Show(string title, int width, int height, Document document = null)
        {
            thread = new Thread(() =>
            {
                Log.Verbose("Starting OpenGLNativeWindow thread...");
                _framebuffer = new EgaColor[width * height];

                nativeWindow = new NativeWindow(new NativeWindowSettings
                {
                    Size = new Vector2i(width, height),
                    Profile = ContextProfile.Compatability,
                    WindowBorder = WindowBorder.Fixed,
                    IsEventDriven = false,
                    Title = title
                });

                unsafe
                {
                    graphicsContext = new GLFWGraphicsContext(nativeWindow.WindowPtr);
                }

                nativeWindow.CenterWindow();
                this.document = document ?? new Document();
                viewerState = new ViewerState(this, this.document, width, height, new YaffFontProvider(), "Terminal_VGA_cp861");

                viewerState.Pages.Clear();
                document.Refresh();

                ulong ticks = 0;
                timer = new Timer(_ => viewerState.Tick(ticks++), null, 0, 1000/30);

                OnLoad();
                nativeWindow.KeyDown += NativeWindow_KeyDown;
                nativeWindow.MouseMove += NativeWindow_MouseMove;
                nativeWindow.MouseUp += NativeWindow_MouseUp;
                nativeWindow.MouseWheel += NativeWindow_MouseWheel;

                while (nativeWindow.Exists)
                {
                    nativeWindow.ProcessEvents();
                    // TODO: fps restrict
                    OnRenderFrame();
                    Thread.Sleep(1);
                }
            });

            thread.Start();
        }

        private void NativeWindow_MouseMove(MouseMoveEventArgs obj) => viewerState.MouseMove(nativeWindow.MousePosition.X, nativeWindow.MousePosition.Y);

        private void NativeWindow_MouseWheel(MouseWheelEventArgs obj)
        {
            
        }

        private void NativeWindow_MouseUp(MouseButtonEventArgs obj) => viewerState.MouseClick(nativeWindow.MousePosition.X, nativeWindow.MousePosition.Y);

        private void NativeWindow_KeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Escape)
                Environment.Exit(0);

            var keys = new Dictionary<Keys, Key>
            {
                { Keys.Up, Key.ARROW_UP },
                { Keys.Left, Key.ARROW_LEFT },
                { Keys.Down, Key.ARROW_DOWN },
                { Keys.Right, Key.ARROW_RIGHT },
                { Keys.Backspace, Key.BACKSPACE },
                { Keys.Delete, Key.DEL },
                { Keys.Home, Key.HOME },
                { Keys.PageUp, Key.PAGE_UP },
                { Keys.PageDown, Key.PAGE_DOWN },
                { Keys.Space, Key.SPACE },
                { Keys.Period, Key.DOT },
                { Keys.A, e.Shift ? Key.A_UPPER : Key.A_LOWER },
                { Keys.B, e.Shift ? Key.B_UPPER : Key.B_LOWER },
                { Keys.C, e.Shift ? Key.C_UPPER : Key.C_LOWER },
                { Keys.D, e.Shift ? Key.D_UPPER : Key.D_LOWER },
                { Keys.E, e.Shift ? Key.E_UPPER : Key.E_LOWER },
                { Keys.F, e.Shift ? Key.F_UPPER : Key.F_LOWER },
                { Keys.G, e.Shift ? Key.G_UPPER : Key.G_LOWER },
                { Keys.H, e.Shift ? Key.H_UPPER : Key.H_LOWER },
                { Keys.I, e.Shift ? Key.I_UPPER : Key.I_LOWER },
                { Keys.J, e.Shift ? Key.J_UPPER : Key.J_LOWER },
                { Keys.K, e.Shift ? Key.K_UPPER : Key.K_LOWER },
                { Keys.L, e.Shift ? Key.L_UPPER : Key.L_LOWER },
                { Keys.M, e.Shift ? Key.M_UPPER : Key.M_LOWER },
                { Keys.N, e.Shift ? Key.N_UPPER : Key.N_LOWER },
                { Keys.O, e.Shift ? Key.O_UPPER : Key.O_LOWER },
                { Keys.P, e.Shift ? Key.P_UPPER : Key.P_LOWER },
                { Keys.Q, e.Shift ? Key.Q_UPPER : Key.Q_LOWER },
                { Keys.R, e.Shift ? Key.R_UPPER : Key.R_LOWER },
                { Keys.S, e.Shift ? Key.S_UPPER : Key.S_LOWER },
                { Keys.T, e.Shift ? Key.T_UPPER : Key.T_LOWER },
                { Keys.U, e.Shift ? Key.U_UPPER : Key.U_LOWER },
                { Keys.V, e.Shift ? Key.V_UPPER : Key.V_LOWER },
                { Keys.W, e.Shift ? Key.W_UPPER : Key.W_LOWER },
                { Keys.X, e.Shift ? Key.X_UPPER : Key.X_LOWER },
                { Keys.Y, e.Shift ? Key.Y_UPPER : Key.Y_LOWER },
                { Keys.Z, e.Shift ? Key.Z_UPPER : Key.Z_LOWER },
                { Keys.Slash, Key.SLASH_FOWARD },
                { Keys.Backslash, Key.SLASH_BACKWARD },
                { Keys.D1, Key.NUMBER_1 },
                { Keys.D2, Key.NUMBER_2 },
                { Keys.D3, Key.NUMBER_3 },
                { Keys.D4, Key.NUMBER_4 },
                { Keys.D5, Key.NUMBER_5 },
                { Keys.D6, Key.NUMBER_6 },
                { Keys.D7, Key.NUMBER_7 },
                { Keys.D8, Key.NUMBER_8 },
                { Keys.D9, Key.NUMBER_9 },
                { Keys.D0, Key.NUMBER_0 },
                { Keys.Enter, Key.ENTER }
            };

            if (keys.TryGetValue(e.Key, out var key))
                viewerState.KeyPress(key);
        }

        private void OnLoad()
        {
            GL.ReadBuffer(ReadBufferMode.Back);
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.LineWidth(2.5f);
        }

        private void OnRenderFrame()
        {
            GL.Viewport(0, 0, nativeWindow.Size.X, nativeWindow.Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.RasterPos2(-1, 1);
            GL.PixelZoom(1, -1);

            lock (framebufferLock)
                GL.DrawPixels(nativeWindow.Size.X, nativeWindow.Size.Y, PixelFormat.Rgb, PixelType.UnsignedByte, _framebuffer);

            graphicsContext.SwapBuffers();
        }

        public void Dispose() => nativeWindow.Close();

        public void SetTitle(string title)
        {
            nativeWindow.Title = title;
        }

        public void SetCursorType(CursorType cursorType)
        {
            nativeWindow.Cursor = cursorType switch
            {
                CursorType.Hand => MouseCursor.Hand,
                _ => MouseCursor.Default
            };
        }
    }
}
