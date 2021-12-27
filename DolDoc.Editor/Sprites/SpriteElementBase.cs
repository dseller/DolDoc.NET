namespace DolDoc.Editor.Sprites
{
    public abstract class SpriteElementBase
    {
        public abstract void Render(SpriteRenderContext ctx, byte[] frameBuffer, int pixelOffset);
    }
}
