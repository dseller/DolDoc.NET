using DolDoc.Editor.Core;

namespace DolDoc.Editor.Rendering
{
    public interface IFrameBufferWindow
    {
        void Show(string title, int width, int heigth, Document document = null);

        void Clear();

        void Render(byte[] data);

        void RenderPartial(int x, int y, int width, int height, byte[] data);

        void SetTitle(string title);

        void SetCursorType(CursorType cursorType);

        ViewerState State { get; }
    }
}
