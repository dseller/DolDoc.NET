using DolDoc.Editor.Core;
using DolDoc.Editor.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DolDoc.Editor
{
    /// <summary>
    /// Manages windows.
    /// </summary>
    public class Compositor<TFrameBuffer>
        where TFrameBuffer : IFrameBufferWindow, new()
    {
        public Compositor(ILoggerFactory logFactory)
        {
            Windows = new List<Window<TFrameBuffer>>();
            LogSingleton.Initialize(logFactory);
        }

        public List<Window<TFrameBuffer>> Windows { get; }

        public Window<TFrameBuffer> NewWindow()
        {
            var logger = LogSingleton.Instance.CreateLogger<Compositor<TFrameBuffer>>();

            logger.LogInformation("Creating new window...");

            var fb = new TFrameBuffer();
            var window = new Window<TFrameBuffer>(this, fb);
            Windows.Add(window);
            return window;
        }
    }
}
