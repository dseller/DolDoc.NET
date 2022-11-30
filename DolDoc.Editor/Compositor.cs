using DolDoc.Editor.Rendering;
using System.Collections.Generic;
using System.Diagnostics;

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
            Debug.WriteLine("Creating new window...");

            var fb = new TFrameBuffer();
            var window = new Window<TFrameBuffer>(this, fb);
            Windows.Add(window);
            return window;
        }
    }
}
