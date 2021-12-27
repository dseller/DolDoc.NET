using System.Runtime.InteropServices;

namespace DolDoc.Renderer.OpenGL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EgaColor
    {
        public static EgaColor[] Palette = new[]
        {
            new EgaColor(0x000000),
            new EgaColor(0x0000aa),
            new EgaColor(0x00aa00),
            new EgaColor(0x00aaaa),
            new EgaColor(0xaa0000),
            new EgaColor(0xaa00aa),
            new EgaColor(0xaa5500),
            new EgaColor(0xaaaaaa),
            new EgaColor(0x555555),
            new EgaColor(0x5555ff),
            new EgaColor(0x55ff55),
            new EgaColor(0x55ffff),
            new EgaColor(0xff5555),
            new EgaColor(0xff55ff),
            new EgaColor(0xffff55),
            new EgaColor(0xffffff), // END EGA
        };

        public EgaColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public EgaColor(int value)
        {
            R = (byte)((value >> 16) & 0xFF);
            G = (byte)((value >> 8) & 0xFF);
            B = (byte)(value & 0xFF);
        }

        public byte R;
        public byte G;
        public byte B;
    }
}
