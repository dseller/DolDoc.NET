using DolDoc.Editor.Core;
using DolDoc.Editor.Rendering;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace DolDoc.BlazorHost
{
    public class Canvas2DFrameBuffer : IFrameBufferWindow
    {
        private IJSRuntime _runtime;

        public Canvas2DFrameBuffer(IJSRuntime runtime)
        {
            _runtime = runtime;
        }

        public string Title { set => throw new NotImplementedException(); }

        public void Clear()
        {
            throw new NotImplementedException();
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

        public void Show(string title, int width, int heigth, Document document = null)
        {
            throw new NotImplementedException();
        }
    }
}
