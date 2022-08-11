using System.Collections.Generic;
using System.Linq;
using DolDoc.Editor.Extensions;

namespace DolDoc.Editor.Fonts
{
    public class YaffFont : IFont
    {
        private readonly IList<byte[]> glyphs;

        public YaffFont(Dictionary<string, string> values, int width, int height, bool mirror)
        {
            Width = width;
            Height = height;

            glyphs = new byte[255][];
            for (int i = 0; i < 255; i++)
            {
                var asHex = string.Format("0x{0:x2}", i);
                if (!values.TryGetValue(asHex, out var glyph) || string.IsNullOrWhiteSpace(glyph))
                {
                    glyphs[i] = Enumerable.Repeat<byte>(0x00, Width * Height).ToArray();
                    continue;
                }

                glyphs[i] = new byte[(Width * Height)/8];

                var glyphEnumerator = glyph.GetEnumerator();
                glyphEnumerator.MoveNext();

                var bytes = Width * Height / 8;
                for (int segment = 0; segment < bytes; segment++)
                {
                    for (int bt = 0; bt < 8; bt++)
                    {
                        if (glyphEnumerator.Current == '@')
                            glyphs[i][segment] |= (byte)(1 << bt);
                        glyphEnumerator.MoveNext();
                    }

                    if (mirror)
                        glyphs[i][segment] = glyphs[i][segment].Reverse();
                }

                if (mirror)
                    glyphs[i] = glyphs[i].Reverse().ToArray();
            }
        }

        public byte[] this[int ch] => glyphs[ch];

        public int Width { get; }

        public int Height { get; }
    }
}
