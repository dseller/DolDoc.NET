using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DolDoc.Editor.Sprites
{
    public class Thick : SpriteElementBase
    {
        public Thick(BinaryReader reader)
        {
            Thickness = reader.ReadInt32();
        }

        public int Thickness { get; }

        public override void Render(SpriteRenderContext ctx, byte[] frameBuffer, int pixelOffset)
        {
            ctx.Thickness = Thickness;
        }
    }
}
