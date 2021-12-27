using DolDoc.Editor.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DolDoc.Editor.Sprites
{
    public class TextBox : Text
    {
        private const int Padding = 2;

        public TextBox(BinaryReader reader)
            : base(reader)
        {
        }

        public override void Render(SpriteRenderContext ctx, byte[] frameBuffer, int pixelOffset)
        {
            // Draw the text first.
            base.Render(ctx, frameBuffer, pixelOffset);

            // Then, draw a border around it.
            for (int i = -2; i < (Value.Length * ctx.State.Font.Width) + Padding; i++)
                frameBuffer[i + pixelOffset + ((Y - Padding) * ctx.State.Width) + X] = 0x00;

            // Bottom
            for (int i = -2; i < (Value.Length * ctx.State.Font.Width) + Padding + 1; i++)
                frameBuffer[i + pixelOffset + ((Y + Padding) * ctx.State.Width) + X + (ctx.State.Font.Width * ctx.State.Width)] = 0x00;

            // Left 
            for (int i = 0; i < ctx.State.Font.Width + (2 * Padding); i++)
                frameBuffer[(i * ctx.State.Width) + pixelOffset + ((Y - 2) * ctx.State.Width) + X - Padding] = 0x00;

            // Right
            for (int i = 0; i < ctx.State.Font.Width + (2 * 2); i++)
                frameBuffer[(i * ctx.State.Width) + (Value.Length * ctx.State.Font.Width) + pixelOffset + ((Y - Padding) * ctx.State.Width) + X + Padding] = 0x00;
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
