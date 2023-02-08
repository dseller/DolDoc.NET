using System.IO;
using OpenTK.Graphics.OpenGL;

namespace DolDoc.Editor.Sprites
{
    public class Thick : SpriteElementBase
    {
        private int thickness;
        
        public Thick(BinaryReader reader)
        {
            thickness = reader.ReadInt32();
        }

        public Thick(int thickness)
        {
            this.thickness = thickness;
        }

        public override void Render(SpriteRenderContext ctx, int x, int y)
        {
            ctx.Thickness = thickness;
            GL.LineWidth(thickness);
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)SpriteElementType.Thick);
            writer.Write(thickness);
        }
    }
}
