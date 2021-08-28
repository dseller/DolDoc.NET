namespace DolDoc.Editor.Rendering
{
    public interface IFrameBuffer
    {
        void Clear();

        void Render(byte[] data);

        void RenderPartial(int x, int y, int width, int height, byte[] data);
    }
}
