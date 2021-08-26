namespace DolDoc.Editor.Fonts
{
    public class TempleOSFontProvider : IFontProvider
    {
        public IFont Get(string name) => new TempleOSFont();
    }
}
