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
                // var character = SysFont.Font[(byte)Value[i]];
                //for (int fx = 0; fx < 8; fx++)
                //    for (int fy = 0; fy < 8; fy++)
                //    {
                //        bool draw = ((character >> ((fy * 8) + fx)) & 0x01) == 0x01;
                //        frameBuffer[((fy + Y) * 640) + (fx + X) + pixelOffset + (i * 8)] = draw ? (byte)ctx.Color : (byte)EgaColor.White;
                //    }

                byte[] character = ctx.State.Font[(byte)Value[i]];
                const int byteSize = 8;

                for (int fy = 0; fy < ctx.State.Font.Height; fy++)
                    for (int fx = 0; fx < ctx.State.Font.Width; fx++)
                    {
                        var fontRow = character[(fy * ctx.State.Font.Width) / byteSize];
                        bool draw = ((fontRow >> (fx % byteSize)) & 0x01) == 0x01;
                        frameBuffer[((fy + Y) * ctx.State.Width) + (fx + X) + pixelOffset + (i * ctx.State.Font.Width)] = draw ? (byte)ctx.Color : (byte)EgaColor.White;
                        //frameBuffer[(((row * _font.Height) + fy) * Width) + (column * _font.Width) + fx + ch.ShiftX] = draw ? (byte)fg : (byte)bg;
                    }
            }
        }

        public override string ToString() => $"Text ({X}, {Y}) [{Value}]";
    }
}
