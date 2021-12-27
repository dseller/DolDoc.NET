using DolDoc.Editor.Core;
using DolDoc.Editor.Rendering;
using Microsoft.Extensions.Logging;

namespace DolDoc.Editor
{
    public class Window<TFrameBuffer>
        where TFrameBuffer : IFrameBufferWindow, new()
    {
        private readonly ILogger logger;
        private readonly Compositor<TFrameBuffer> compositor;
        private readonly IFrameBufferWindow frameBufferWindow;

        public Window(Compositor<TFrameBuffer> compositor, IFrameBufferWindow frameBufferWindow)
        {
            this.compositor = compositor;
            this.frameBufferWindow = frameBufferWindow;
            logger = LogSingleton.Instance.CreateLogger<Window<TFrameBuffer>>();
        }

        public void Show(string title, int width, int height, Document document = null)
        {
            logger.LogInformation("Showing window, width={0}, height={1}", width, height);
            frameBufferWindow.Show(title, width, height, document);
        }

        public ViewerState State => frameBufferWindow.State;
    }
}
