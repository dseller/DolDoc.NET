using System.IO;

namespace DolDoc.Editor.Sprites
{
    public class Bitmap : SpriteElementBase
    {
        private readonly int x;
        private readonly int y;
        private readonly int w;
        private readonly int h;
        private readonly byte[] data;

        public Bitmap(BinaryReader reader)
        {
            x = reader.ReadInt32();
            y = reader.ReadInt32();
            w = reader.ReadInt32();
            h = reader.ReadInt32();
            data = reader.ReadBytes(w * h);
        }

        public override void Render(SpriteRenderContext ctx, int x, int y)
        {
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)SpriteElementType.Bitmap);
            writer.Write(x);
            writer.Write(y);
            writer.Write(w);
            writer.Write(h);
            writer.Write(data);
        }
    }
}
