using DolDoc.Editor.Rendering;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace DolDoc.BlazorHost
{
    public class Canvas2DFrameBuffer : IFrameBuffer
    {
        private IJSRuntime _runtime;

        public Canvas2DFrameBuffer(IJSRuntime runtime)
        {
            _runtime = runtime;
        }

        public void Render(byte[] data)
        {
            Task.Run(async () =>
            {
                await _runtime.InvokeVoidAsync("renderDolDoc", new object[] { data });
            });
        }

        public void RenderPartial(int x, int y, int width, int height, byte[] data)
        {
            
        }
    }
}
