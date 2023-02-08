using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace DolDoc.Editor.Sprites
{
    public class Arrow : Line
    {
        public Arrow(BinaryReader reader)
            : base(reader)
        {
        }

        public Arrow(int x1, int y1, int x2, int y2)
            : base(x1, y1, x2, y2)
        {
        }

        public override void Render(SpriteRenderContext ctx, int x, int y)
        {
            // First, render the line.
            base.Render(ctx, x, y);

            // Then, render the arrow head on top.
            RenderArrowHead(ctx, x + X1, y + Y1, x + X2, y + Y2);
        }

        /// <summary>
        /// Renders an arrow head at the end of the line.
        /// </summary>
        /// <param name="ctx">Render ctx</param>
        /// <param name="frameBuffer">Framebuffer</param>
        /// <param name="pixelOffset">Offset in framebuffer</param>
        /// <param name="x1">Source X</param>
        /// <param name="y1">Source Y</param>
        /// <param name="x2">Dest X</param>
        /// <param name="y2">Dest Y</param>
        /// <param name="size">Length in pixels of the arrow heads</param>
        protected void RenderArrowHead(SpriteRenderContext renderContext, int x1, int y1, int x2, int y2, int size = 6)
        {
            var angle = Math.Atan2(y2 - y1, x2 - x1);
            GL.Begin(BeginMode.Lines);
            GL.Vertex2(x2, renderContext.State.Height - y2);
            GL.Vertex2((int) (x2 - size * Math.Cos(angle - Math.PI / 6)), (int) (renderContext.State.Height - y2 - size * Math.Sin(angle - Math.PI / 6)));
            GL.Vertex2(x2, renderContext.State.Height - y2);
            GL.Vertex2((int) (x2 - size * Math.Cos(angle + Math.PI / 6)), (int) (renderContext.State.Height - y2 - size * Math.Sin(angle + Math.PI / 6)));
            GL.End();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte) SpriteElementType.Arrow);
            writer.Write(X1);
            writer.Write(Y1);
            writer.Write(X2);
            writer.Write(Y2);
        }

        public override string ToString() => $"Arrow ({X1}, {Y1}) => ({X2}, {Y2})";
    }
}