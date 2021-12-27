using DolDoc.Editor.Core;
using System.IO;

namespace DolDoc.Editor.Sprites
{
    public class Color : SpriteElementBase
    {
        public Color(BinaryReader reader)
        {
            _color = (EgaColor)reader.ReadByte();
        }

        public Color(EgaColor c) => _color = c;

        private EgaColor _color;

        public override void Render(SpriteRenderContext ctx, byte[] frameBuffer, int pixelOffset)
        {
            ctx.Color = _color;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)SpriteElementType.Color);
            writer.Write((byte)_color);
        }

        public override string ToString() => $"Color ({_color})";
    }
}
