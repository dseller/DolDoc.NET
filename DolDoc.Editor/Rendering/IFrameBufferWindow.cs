namespace DolDoc.Editor.Rendering
{
    public interface IFrameBufferWindow
    {
        void Show(string title, int width, int height);

        void Clear();

        void SetTitle(string title);

        void SetCursorType(CursorType cursorType);
        
        int? Width { get; }
        
        int? Height { get; }

        Compositor.Compositor Compositor { get; set; }
    }
}
