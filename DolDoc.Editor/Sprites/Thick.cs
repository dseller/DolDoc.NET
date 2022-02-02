﻿using System.IO;

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

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)SpriteElementType.Thick);
            writer.Write(Thickness);
        }
    }
}
