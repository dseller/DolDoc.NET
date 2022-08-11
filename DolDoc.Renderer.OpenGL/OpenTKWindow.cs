using System;
using System.Drawing;
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

        public Wnd(ViewerState state, GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            this.state = state;
        }

        protected override void OnLoad()
        {
            GL.ReadBuffer(ReadBufferMode.Back);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            GL.ClearColor(1, 1, 1, 1);
            
            // GL.ClearColor(0, 0, 0, 0);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.LineWidth(2.5f);
            
            base.OnLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Color3(Color.Black);
            GL.RasterPos2(-1, .98);
            GL.PixelZoom(1, -1);
            GL.Disable(EnableCap.Lighting);

            var page = state.CurrentPage;
            var chCounter = 1;
            foreach (var ch in page)
            {
                var fg = EgaColor.Palette[(byte)ch.Color.Foreground];
                GL.Color3((sbyte)fg.R, (sbyte)fg.G, (sbyte)fg.B);
                GL.Bitmap(state.Font.Width, state.Font.Height, 
                    0, 0,  
                    (chCounter % state.Columns == 0 ? -((state.Columns - 1) * state.Font.Width) : state.Font.Width), 
                    (chCounter % state.Columns == 0) ? -state.Font.Height : 0, 
                    state.Font[ch.Char]);

                if ((ch.Flags & CharacterFlags.Underline) == CharacterFlags.Underline)
                {
                    // GL.Begin(BeginMode.Lines);
                    // GL.Vertex2(new Vector2d((chCounter % state.Columns) * .02, (chCounter / state.Columns) * .02));
                    // GL.Vertex2(new Vector2d(((chCounter % state.Columns) + state.Font.Width) * .02), (chCou));
                    // GL.End();
                }
                
                chCounter++;
            }

            //Code goes here.
            GL.Flush();
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            var ch = KeyMap.Map[e.Key];
            state.KeyPress(ch);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e) => state.MouseMove(e.X, e.Y);

        protected override void OnMouseDown(MouseButtonEventArgs e) => state.MouseClick(MousePosition.X, MousePosition.Y);
    }
    
    public class OpenTKWindow : IFrameBufferWindow
    {
        private Wnd window;
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
                var nativeSettings = new NativeWindowSettings
                {
                    Size = new Vector2i(width, heigth),
                    Profile = ContextProfile.Compatability,
                    WindowBorder = WindowBorder.Fixed,
                    IsEventDriven = false,
                    Title = title
                };
                
                ulong ticks = 0;
                var timer = new Timer(_ => State.Tick(ticks++), null, 0, 1000 / 30);

                using (window = new Wnd(State, settings, nativeSettings))
                {
                    window.Run();
                }
            });
            thread.Start();
        }

        public void Clear()
        {
        }

        public void Render(byte[] data)
        {
        }

        public void RenderPartial(int x, int y, int width, int height, byte[] data)
        {
        }

        public void SetTitle(string title)
        {
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