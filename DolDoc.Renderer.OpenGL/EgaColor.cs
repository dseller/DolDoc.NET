using System.Runtime.InteropServices;

namespace DolDoc.Renderer.OpenGL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EgaColor
    {
        public static readonly EgaColor[] Palette = new[]
        {
            new EgaColor(0x00, 0x00, 0x00),
            new EgaColor(0x00, 0x00, 0xAA),
            new EgaColor(0x00, 0xAA, 0x00),
            new EgaColor(0x00, 0xAA, 0xAA),
            new EgaColor(0xAA, 0x00, 0x00),
            new EgaColor(0xAA, 0x00, 0xAA),
            new EgaColor(0xAA, 0x55, 0x00),
            new EgaColor(0xAA, 0xAA, 0xAA),
            new EgaColor(0x55, 0x55, 0x55),
            new EgaColor(0x55, 0x55, 0xFF),
            new EgaColor(0x55, 0xFF, 0x55),
            new EgaColor(0x55, 0xFF, 0xFF),
            new EgaColor(0xFF, 0x55, 0x55),
            new EgaColor(0xFF, 0x55, 0xFF),
            new EgaColor(0xFF, 0xFF, 0x55),
            new EgaColor(0xFF, 0xFF, 0xFF)
        };

        public EgaColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
            RD = r / 255d;
            GD = g / 255d;
            BD = b / 255d;
        }

        public byte R;
        public byte G;
        public byte B;

        public double RD;
        public double GD;
        public double BD;
    }
}
