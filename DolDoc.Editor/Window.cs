using DolDoc.Editor.Core;
using DolDoc.Editor.Rendering;
using System.Diagnostics;

namespace DolDoc.Editor
{
    public class Window<TFrameBuffer>
        where TFrameBuffer : IFrameBufferWindow, new()
    {
        private readonly Compositor<TFrameBuffer> compositor;
        private readonly IFrameBufferWindow frameBufferWindow;

        public Window(Compositor<TFrameBuffer> compositor, IFrameBufferWindow frameBufferWindow)
        {
            this.compositor = compositor;
            this.frameBufferWindow = frameBufferWindow;
        }

        public void Show(string title, int width, int height, Document document = null)
        {
            Debug.WriteLine($"Showing window, width={width}, height={height}");
            frameBufferWindow.Show(title, width, height, document);
        }

        public ViewerState State => frameBufferWindow.State;
    }
}
