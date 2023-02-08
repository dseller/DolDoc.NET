using DolDoc.Editor.Core;
using DolDoc.Editor.Extensions;
using System.IO;
using System.Linq;
using OpenTK.Graphics.OpenGL;

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

        public Text(int x, int y, string text)
        {
            X = x;
            Y = y;
            Value = text;
        }

        public int X { get; }

        public int Y { get; }

        public string Value { get; }

        public override void Render(SpriteRenderContext ctx, int x, int y)
        {
            var bgColor = EgaColorRgbBitmap.Palette[(byte)ctx.State.DefaultBackgroundColor];
            var fgColor = EgaColorRgbBitmap.Palette[(byte)ctx.Color];
            var filledBitmap = Enumerable.Repeat((byte) 0xFF, (ctx.State.Font.Width * ctx.State.Font.Height) / 8).ToArray();
            for (int i = 0; i < Value.Length; i++)
            {
                GL.Color3(bgColor.RD, bgColor.GD, bgColor.BD);
                GL.WindowPos2(x + X + (i * ctx.State.Font.Width),  ctx.State.Height - (y + Y + 1));
                GL.Bitmap(ctx.State.Font.Width, ctx.State.Font.Height, 0, 0, 0, 0, filledBitmap);

                GL.Color3(fgColor.RD, fgColor.GD, fgColor.BD);
                GL.WindowPos2(x + X + (i  * ctx.State.Font.Width), ctx.State.Height - (y + Y + 1));

                GL.Bitmap(ctx.State.Font.Width, ctx.State.Font.Height,
                    0, 0,
                    0, //(chCounter % state.Columns == 0 ? -((state.Columns - 1) * state.Font.Width) : state.Font.Width), 
                    0, //(chCounter % state.Columns == 0) ? -state.Font.Height : 0, 
                    ctx.State.Font[(byte)Value[i]]);
            }
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)SpriteElementType.Text);
            writer.Write(X);
            writer.Write(Y);
            writer.WriteNullTerminatedString(Value);
        }

        public override string ToString() => $"Text ({X}, {Y}) [{Value}]";
    }
}
