using DolDoc.Editor.Core;
using DolDoc.Editor.Rendering;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

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
            Log.Information("Showing window, width={0}, height={1}", width, height);
            frameBufferWindow.Show(title, width, height, document);
        }
    }
}
