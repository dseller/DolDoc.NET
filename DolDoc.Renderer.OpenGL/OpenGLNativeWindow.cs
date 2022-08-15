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
using System;
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
            var key = KeyMap.GetKey(e);
            if (key == null)
                return; 

            viewerState.KeyPress(key.Value);
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
