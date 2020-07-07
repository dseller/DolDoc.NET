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
            Console.WriteLine("Sprite: ^^^ is actually arrow", X1, Y1, X2, Y2);
        }

        public override void Render(byte[] frameBuffer, int pixelOffset)
        {
            // First, render the line.
            base.Render(frameBuffer, pixelOffset);
        }
    }
}
