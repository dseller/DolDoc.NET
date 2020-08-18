using DolDoc.Editor.Core;
using System;
using System.IO;

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

        public int X1 { get; }

        public int Y1 { get; }

        public int X2 { get; }

        public int Y2 { get; }

		public override void Render(SpriteRenderContext ctx, byte[] frameBuffer, int pixelOffset) => RenderLine(frameBuffer, pixelOffset, X1, Y1, X2, Y2, ctx.Color);

		protected void RenderLine(byte[] frameBuffer, int pixelOffset, int x1, int y1, int x2, int y2, EgaColor color = EgaColor.Black)
        {
            int w = x2 - x1;
            int h = y2 - y1;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                // putpixel(x, y, color);
                if ((y1 * 640) + x1 + pixelOffset < 0)
                    continue;

                frameBuffer[(y1 * 640) + x1 + pixelOffset] = (byte)color;
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x1 += dx1;
                    y1 += dy1;
                }
                else
                {
                    x1 += dx2;
                    y1 += dy2;
                }
            }
        }

		public override string ToString() => $"Line ({X1}, {Y1}) => ({X2}, {Y2})";
    }
}
