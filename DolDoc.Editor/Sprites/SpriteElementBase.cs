using System.IO;

namespace DolDoc.Editor.Sprites
{
    public abstract class SpriteElementBase
    {
        public abstract void Render(SpriteRenderContext ctx, byte[] frameBuffer, int pixelOffset);

        public abstract void Serialize(BinaryWriter writer);
    }
}
