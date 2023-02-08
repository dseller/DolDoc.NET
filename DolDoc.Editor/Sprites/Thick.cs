using System.IO;

namespace DolDoc.Editor.Sprites
{
    public class Thick : SpriteElementBase
    {
        public Thick(BinaryReader reader)
        {
            Thickness = reader.ReadInt32();
        }

        public Thick(int thickness)
        {
            Thickness = thickness;
        }

        public int Thickness { get; }

        public override void Render(SpriteRenderContext ctx, int x, int y)
        {
            ctx.Thickness = Thickness;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)SpriteElementType.Thick);
            writer.Write(Thickness);
        }
    }
}
