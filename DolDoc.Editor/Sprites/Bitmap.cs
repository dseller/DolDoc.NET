using System.IO;

namespace DolDoc.Editor.Sprites
{
    public class Bitmap : SpriteElementBase
    {
        private int _x, _y, _w, _h;
        private byte[] _data;

        public Bitmap(BinaryReader reader)
        {
            _x = reader.ReadInt32();
            _y = reader.ReadInt32();
            _w = reader.ReadInt32();
            _h = reader.ReadInt32();
            _data = reader.ReadBytes(_w * _h);
        }

        public override void Render(SpriteRenderContext ctx, int x, int y)
        {
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)SpriteElementType.Bitmap);
            writer.Write(_x);
            writer.Write(_y);
            writer.Write(_w);
            writer.Write(_h);
            writer.Write(_data);
        }
    }
}
