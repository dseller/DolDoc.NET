using DolDoc.Editor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Rendering;
using OpenGL;
using OpenGL.CoreUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace DolDoc.OpenGLHost
{
    public static class Colors
    {
        public static EgaColor[] ColorsD = new[]
        {
            new EgaColor(0x00, 0x00, 0x00),
            new EgaColor(0x00, 0x00, 0xAA),
            new EgaColor(0x00, 0xAA, 0x00),
            new EgaColor(0x00, 0xAA, 0xAA),
            new EgaColor(0xAA, 0x00, 0x00),
            new EgaColor(0xAA, 0x00, 0xAA),
            new EgaColor(0xAA, 0x55, 0x00),
            new EgaColor(0xAA, 0xAA, 0xAA),
            new EgaColor(0x55, 0x55, 0x55),
            new EgaColor(0x55, 0x55, 0xFF),
            new EgaColor(0x55, 0xFF, 0x55),
            new EgaColor(0x55, 0xFF, 0xFF),
            new EgaColor(0xFF, 0x55, 0x55),
            new EgaColor(0xFF, 0x55, 0xFF),
            new EgaColor(0xFF, 0xFF, 0x55),
            new EgaColor(0xFF, 0xFF, 0xFF)
        };
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EgaColor
    {
        public EgaColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte R;
        public byte G;
        public byte B;
    }

    public class Program : IFrameBuffer
    {
        private readonly EgaColor[] _framebuffer = new EgaColor[1024 * 768];
        private Document _document;
        private ViewerState _viewerState;
        private Timer timer;

        static void Main(string[] args)
        {
            Program p = new Program();
            p.RUn();
        }

        public void RUn()
        {
            using (NativeWindow nativeWindow = NativeWindow.Create())
            {
                nativeWindow.ContextCreated += NativeWindow_ContextCreated;
                nativeWindow.Render += NativeWindow_Render;
                nativeWindow.Create(0, 0, 1024, 768, NativeWindowStyle.None);
                nativeWindow.Show();
                nativeWindow.KeyDown += NativeWindow_KeyDown;
                nativeWindow.Resize += NativeWindow_Resize;

                var fs = File.Exists("Main.DD") ? File.Open("Main.DD", FileMode.Open) : null;
                LoadFile(fs);

                //_document = new Document(128, 96);
                //_viewerState = new ViewerState(this, _document, 1024, 768);

                //Shell shell = new Shell(_document);
                //shell.Start();

                timer = new Timer(_ => _viewerState.Tick(), null, 0, 200);

                nativeWindow.Run();
            }
        }

        private void NativeWindow_Resize(object sender, EventArgs e)
        {
            NativeWindow nativeWindow = (NativeWindow)sender;
            Gl.Viewport(0, 0, (int)nativeWindow.Width, (int)nativeWindow.Height);
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
                _viewerState.KeyDown(key);
        }

        private void LoadFile(Stream stream)
        {
            if (stream == null)
                stream = new MemoryStream(Encoding.ASCII.GetBytes($"\n\n$FG,RED$$TX+B+RX+CY,\"Could not open file!\"$"));

            _document = DocumentLoader.Load(stream, 128, 96);
            _viewerState = new ViewerState(this, _document, 1024, 768);
            _viewerState.Pages.Clear();
            _document.Refresh();
        }

        private void NativeWindow_Render(object sender, NativeWindowEventArgs e)
        {
            NativeWindow nativeWindow = (NativeWindow)sender;
            Gl.Viewport(0, 0, (int)nativeWindow.Width, (int)nativeWindow.Height);
            Gl.Clear(ClearBufferMask.ColorBufferBit);
            Gl.RasterPos2(-1, 1);
            Gl.PixelZoom(1, -1);
            Gl.DrawPixels(1024, 768, PixelFormat.Rgb, PixelType.UnsignedByte, _framebuffer);
        }

        private static void NativeWindow_ContextCreated(object sender, NativeWindowEventArgs e)
        {
            Gl.ReadBuffer(ReadBufferMode.Back);
            Gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            Gl.Enable(EnableCap.Blend);
            Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            Gl.LineWidth(2.5f);
        }

        public void Render(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
                _framebuffer[i] = Colors.ColorsD[data[i]];
        }

        public void RenderPartial(int x, int y, int width, int height, byte[] data)
        {
            //for (int dstY = 0; dstY < height; dstY++)
            //    Array.Copy(data, x + ((dstY + y) * _viewerState.Width), _framebuffer, (y*_viewerState.Width), width);
            //    //Marshal.Copy(data, x + ((dstY + y) * 640), bmpData.Scan0 + (dstY * bmpData.Stride), width);
        }

        public void Clear()
        {
            Array.Fill(_framebuffer, Colors.ColorsD[15]);
        }
    }
}
