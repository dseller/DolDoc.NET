using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace DolDoc.Editor.Sprites
{
    public class Circle : SpriteElementBase
    {
        private readonly int x, y;
        private readonly int radius;
        
        public Circle(BinaryReader reader)
        {
            x = reader.ReadInt32();
            y = reader.ReadInt32();
            radius = reader.ReadInt32();
        }

        public Circle(int x, int y, int radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
        }
        
        public override void Render(SpriteRenderContext ctx, int x, int y)
        {
            GL.Begin(BeginMode.Polygon);
            for (int i = 0; i <= 300; i++) {
                double angle = 2 * Math.PI * i / 300;
                double xTemp = Math.Cos(angle) * radius;
                double yTemp = Math.Sin(angle) * radius;
                GL.Vertex2(this.x + x + xTemp, this.y + y + yTemp);
            }
            GL.End();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)SpriteElementType.Circle);
            writer.Write(x);
            writer.Write(y);
            writer.Write(radius);
        }
    }
}