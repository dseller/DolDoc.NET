using DolDoc.Editor.Extensions;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace DolDoc.Editor.Sprites
{
    public class TextBox : Text
    {
        private const int Padding = 2;

        public TextBox(BinaryReader reader)
            : base(reader)
        {
        }

        public TextBox(int x, int y, string value)
            : base(x, y, value)
        {
        }

        public override void Render(SpriteRenderContext ctx, int x, int y)
        {
            // Draw the text first.
            base.Render(ctx, x, y);
            
            GL.Begin(BeginMode.Lines);
            // Top
            GL.Vertex2((x + X) - 2 - Padding, ctx.State.Height - (y + Y) + 2 + Padding + ctx.Compositor.Font.Height);
            GL.Vertex2((x + X) + 2 + Padding + (Value.Length * ctx.Compositor.Font.Width), ctx.State.Height - (y + Y) + 2 + Padding + ctx.Compositor.Font.Height);
            // Bottom
            GL.Vertex2((x + X) - 2 - Padding, ctx.State.Height - (y + Y) - 2 - Padding);
            GL.Vertex2((x + X) + 2 + Padding + (Value.Length * ctx.Compositor.Font.Width), ctx.State.Height - (y + Y) - 2 - Padding);
            // Left
            GL.Vertex2((x + X) - 2 - Padding, ctx.State.Height - (y + Y) + 2 + Padding + ctx.Compositor.Font.Height);
            GL.Vertex2((x + X) - 2 - Padding, ctx.State.Height - (y + Y) - 2 - Padding);
            // Right
            GL.Vertex2((x + X) + 2 + Padding + (Value.Length * ctx.Compositor.Font.Width), ctx.State.Height - (y + Y) + 2 + Padding + ctx.Compositor.Font.Height);
            GL.Vertex2((x + X) + 2 + Padding + (Value.Length * ctx.Compositor.Font.Width), ctx.State.Height - (y + Y) - 2 - Padding);
            GL.End();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)SpriteElementType.TextBox);
            writer.Write(X);
            writer.Write(Y);
            writer.WriteNullTerminatedString(Value);
        }

        public override string ToString() => $"TextBox ({X}, {Y}) [{Value}]";
    }
}
