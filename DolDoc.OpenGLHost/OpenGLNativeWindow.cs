using DolDoc.Editor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Rendering;
using OpenGL;
using OpenGL.CoreUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DolDoc.OpenGLHost
{
    public class OpenGLNativeWindow : IFrameBufferWindow, IDisposable
    {
        private readonly Thread thread;
        private Timer timer;
        private readonly NativeWindow nativeWindow;
        private Document document;
        private ViewerState viewerState;
        private EgaColor[] _framebuffer;// = new EgaColor[1024 * 768];
        private object framebufferLock = new object();
        private int width, height;

        public OpenGLNativeWindow()
        {
            nativeWindow = NativeWindow.Create();
            thread = new Thread(Run);
            // 
        }

        public void Clear() => Array.Fill(_framebuffer, EgaColor.Palette[15]);

        private void Run()
        {
            Log.Verbose("Starting OpenGLNativeWindow thread...");

            nativeWindow.ContextCreated += NativeWindow_ContextCreated;
            nativeWindow.Render += NativeWindow_Render;
            nativeWindow.KeyDown += NativeWindow_KeyDown;
            nativeWindow.Create(0, 0, (uint)width, (uint)height, NativeWindowStyle.None);
            nativeWindow.Show();

            viewerState.Pages.Clear();
            document.Refresh();

            timer = new Timer(_ => viewerState.Tick(), null, 0, 200);
            nativeWindow.Run();
        }

        public void Dispose() => nativeWindow.Dispose();

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
            this.width = width;
            this.height = height;
            this.document = document ?? new Document(width / 8, height / 8);
            viewerState = new ViewerState(this, this.document, width, height);
            Log.Verbose("Document content: {0}", this.document.ToPlainText());

            _framebuffer = new EgaColor[width * height];
            thread.Start();
        }

        private void NativeWindow_Render(object sender, NativeWindowEventArgs e)
        {
            NativeWindow nativeWindow = (NativeWindow)sender;
            Gl.Viewport(0, 0, (int)nativeWindow.Width, (int)nativeWindow.Height);
            Gl.Clear(ClearBufferMask.ColorBufferBit);
            Gl.RasterPos2(-1, 1);
            Gl.PixelZoom(1, -1);

            lock (framebufferLock)
                Gl.DrawPixels(width, height, PixelFormat.Rgb, PixelType.UnsignedByte, _framebuffer);
        }

        private static void NativeWindow_ContextCreated(object sender, NativeWindowEventArgs e)
        {
            Gl.ReadBuffer(ReadBufferMode.Back);
            Gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            Gl.Enable(EnableCap.Blend);
            Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            Gl.LineWidth(2.5f);
        }

        private void NativeWindow_KeyDown(object sender, NativeWindowKeyEventArgs e)
        {
            if (e.Key == KeyCode.Escape)
                Environment.Exit(0);

            var keyDownTranslation = new Dictionary<KeyCode, ConsoleKey>
            {
                { KeyCode.Down, ConsoleKey.DownArrow},
                { KeyCode.Right, ConsoleKey.RightArrow },
                { KeyCode.Left, ConsoleKey.LeftArrow },
                { KeyCode.Up, ConsoleKey.UpArrow },
                //{ KeyCode., ConsoleKey.Backspace },
                { KeyCode.Delete, ConsoleKey.Delete },
                { KeyCode.Home, ConsoleKey.Home },
                //{ KeyCode., ConsoleKey.PageUp },
                //{ KeyCode.PageDown, ConsoleKey.PageDown }
                { KeyCode.Space, ConsoleKey.Spacebar }
            };

            if (keyDownTranslation.TryGetValue(e.Key, out var key))
                viewerState.KeyDown(key);
        }
    }
}
