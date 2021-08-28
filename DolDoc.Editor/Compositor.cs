using DolDoc.Editor.Rendering;
using Serilog;
using System.Collections.Generic;

namespace DolDoc.Editor
{
    /// <summary>
    /// Manages windows.
    /// </summary>
    public class Compositor<TFrameBuffer>
        where TFrameBuffer : IFrameBufferWindow, new()
    {
        public Compositor()
        {
            Windows = new List<Window<TFrameBuffer>>();
        }

        public List<Window<TFrameBuffer>> Windows { get; }

        public Window<TFrameBuffer> NewWindow()
        {
            Log.Information("Creating new window...");

            var fb = new TFrameBuffer();
            var window = new Window<TFrameBuffer>(this, fb);
            Windows.Add(window);
            return window;
        }
    }
}
