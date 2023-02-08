using DolDoc.Editor.Core;
using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace DolDoc.Editor.Sprites
{
    public class Line : SpriteElementBase
    {
        public Line(BinaryReader reader)
        {
            X1 = reader.ReadInt32();
            Y1 = reader.ReadInt32();
            X2 = reader.ReadInt32();
            Y2 = reader.ReadInt32();
        }

        public Line(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        protected int X1 { get; }

        protected int Y1 { get; }

        protected int X2 { get; }

        protected int Y2 { get; }

		public override void Render(SpriteRenderContext ctx, int x, int y) => RenderLine(ctx, x + X1, y + Y1, x + X2, y + Y2, ctx.Color);

		protected void RenderLine(SpriteRenderContext renderContext, int x1, int y1, int x2, int y2, EgaColor color = EgaColor.Black)
        {
            GL.Begin(BeginMode.Lines);
            GL.Vertex2(x1, renderContext.State.Height - (y1 + 1));
            GL.Vertex2(x2, renderContext.State.Height - (y2 + 1));
            GL.End();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)SpriteElementType.Line);
            writer.Write(X1);
            writer.Write(Y1);
            writer.Write(X2);
            writer.Write(Y2);
        }

		public override string ToString() => $"Line ({X1}, {Y1}) => ({X2}, {Y2})";

    }
}
