using DolDoc.Editor.Core;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace DolDoc.Editor.Sprites
{
    public class Color : SpriteElementBase
    {
        private readonly EgaColor color;
        
        public Color(BinaryReader reader)
        {
            color = (EgaColor)reader.ReadByte();
        }

        public Color(EgaColor c) => color = c;

        public override void Render(SpriteRenderContext ctx, int x, int y)
        {
            ctx.Color = color;
            var c = EgaColorRgbBitmap.Palette[(byte) color];
            GL.Color3(c.RD, c.GD, c.BD);
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)SpriteElementType.Color);
            writer.Write((byte)color);
        }

        public override string ToString() => $"Color ({color})";
    }
}
