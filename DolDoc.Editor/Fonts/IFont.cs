namespace DolDoc.Editor.Fonts
{
    public interface IFont
    {
        int Width { get; }

        int Height { get; }

        byte[] this[int ch] { get; }
    }
}
