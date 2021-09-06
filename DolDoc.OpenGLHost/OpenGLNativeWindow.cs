using DolDoc.Editor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Fonts;
using DolDoc.Editor.Rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DolDoc.OpenGLHost
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
                    WindowBorder = WindowBorder.Hidden,
                    IsEventDriven = false
                });


                unsafe
                {
                    graphicsContext = new GLFWGraphicsContext(nativeWindow.WindowPtr);
                }

                nativeWindow.CenterWindow();
                this.document = document ?? new Document(width / 8, height / 8);
                viewerState = new ViewerState(this, this.document, width, height, new YaffFontProvider(), "Courier_8_2");

                viewerState.Pages.Clear();
                document.Refresh();
                timer = new Timer(_ => viewerState.Tick(), null, 0, 200);
                
                OnLoad();
                nativeWindow.KeyDown += NativeWindow_KeyDown;

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

        private void NativeWindow_KeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Escape)
                Environment.Exit(0);

            var keyDownTranslation = new Dictionary<Keys, ConsoleKey>
            {
                { Keys.Down, ConsoleKey.DownArrow},
                { Keys.Right, ConsoleKey.RightArrow },
                { Keys.Left, ConsoleKey.LeftArrow },
                { Keys.Up, ConsoleKey.UpArrow },
                //{ KeyCode., ConsoleKey.Backspace },
                { Keys.Delete, ConsoleKey.Delete },
                { Keys.Home, ConsoleKey.Home },
                //{ KeyCode., ConsoleKey.PageUp },
                { Keys.PageDown, ConsoleKey.PageDown },
                { Keys.Space, ConsoleKey.Spacebar },
            };

            if (keyDownTranslation.TryGetValue(e.Key, out var key))
                viewerState.KeyDown(key);
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
                GL.DrawPixels(1024, 768, PixelFormat.Rgb, PixelType.UnsignedByte, _framebuffer);

            graphicsContext.SwapBuffers();
        }

        public void Dispose() => nativeWindow.Close();
    }
}
