using System.IO;

namespace DolDoc.Editor.Sprites
{
    public abstract class SpriteElementBase
    {
        public abstract void Render(SpriteRenderContext ctx, int x, int y);

        public abstract void Serialize(BinaryWriter writer);
    }
}
