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

            Console.WriteLine("Sprite: line ({0}, {1}) -> ({2}, {3})", X1, Y1, X2, Y2);
        }

        public int X1 { get; }

        public int Y1 { get; }

        public int X2 { get; }

        public int Y2 { get; }

		public override void Render(byte[] frameBuffer, int pixelOffset) => RenderLine(frameBuffer, pixelOffset, X1, Y1, X2, Y2);

		protected void RenderLine(byte[] frameBuffer, int pixelOffset, int x1, int y1, int x2, int y2, EgaColor color = EgaColor.Black)
        {
			var steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
			if (steep)
			{
				Swap(ref x1, ref y1);
				Swap(ref x2, ref y2);
			}

			if (x1 > x2)
			{
				Swap(ref x1, ref x2);
				Swap(ref y1, ref y2);
			}

			int dx = x2 - x1;
			int dy = Math.Abs(y2 - y1);
			int error = dx / 2;
			int ystep = (y1 < y2) ? 1 : -1;
			int y = y1;
			int maxX = x2;

			for (int x = x1; x < maxX; x++)
			{
				if (steep)
					frameBuffer[(x * 640) + y + pixelOffset] = (byte)color;
				else
					frameBuffer[(y * 640) + x + pixelOffset] = (byte)color;

				error -= dy;
				if (error < 0)
				{
					y += ystep;
					error += dx;
				}
			}
		}

		private static void Swap<T>(ref T a, ref T b)
        {
			T temp = a;
			a = b;
			b = temp;
        }
    }
}
