using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DolDoc.Editor.Sprites
{
    public class Arrow : Line
    {
        public Arrow(BinaryReader reader) 
            : base(reader)
        {
        }

        public override void Render(SpriteRenderContext ctx, byte[] frameBuffer, int pixelOffset)
        {
            // First, render the line.
            base.Render(ctx, frameBuffer, pixelOffset);
        }

        public override string ToString() => $"Arrow ({X1}, {Y1}) => ({X2}, {Y2})";
    }
}
