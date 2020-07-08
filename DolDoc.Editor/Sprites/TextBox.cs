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
            for (int i = -2; i < (Value.Length * 8) + Padding; i++)
                frameBuffer[i + pixelOffset + ((Y - Padding) * 640) + X] = 0x00;

            // Bottom
            for (int i = -2; i < (Value.Length * 8) + Padding + 1; i++)
                frameBuffer[i + pixelOffset + ((Y + Padding) * 640) + X + (8 * 640)] = 0x00;

            // Left 
            for (int i = 0; i < 8 + (2 * Padding); i++)
                frameBuffer[(i * 640) + pixelOffset + ((Y - 2) * 640) + X - Padding] = 0x00;

            // Right
            for (int i = 0; i < 8 + (2 * 2); i++)
                frameBuffer[(i * 640) + (Value.Length * 8) + pixelOffset + ((Y - Padding) * 640) + X + Padding] = 0x00;
        }

        public override string ToString() => $"TextBox ({X}, {Y}) [{Value}]";
    }
}
