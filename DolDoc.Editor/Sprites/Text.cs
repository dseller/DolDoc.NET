using DolDoc.Editor.Core;
using DolDoc.Editor.Extensions;
using System.IO;

namespace DolDoc.Editor.Sprites
{
    public class Text : SpriteElementBase
    {
        public Text(BinaryReader reader)
        {
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            Value = reader.ReadNullTerminatedString();
        }

        public int X { get; }

        public int Y { get; }

        public string Value { get; }

        public override void Render(SpriteRenderContext ctx, byte[] frameBuffer, int pixelOffset)
        {
            // foreach (char ch in Value)
            for (int i = 0; i < Value.Length; i++)
            {
                var character = SysFont.Font[(byte)Value[i]];
                for (int fx = 0; fx < 8; fx++)
                    for (int fy = 0; fy < 8; fy++)
                    {
                        bool draw = ((character >> ((fy * 8) + fx)) & 0x01) == 0x01;
                        frameBuffer[((fy + Y) * 640) + (fx + X) + pixelOffset + (i*8)] = draw ? (byte)ctx.Color : (byte)EgaColor.White;
                    }
            }
        }

        public override string ToString() => $"Text ({X}, {Y}) [{Value}]";
    }
}
