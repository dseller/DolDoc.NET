using System.Runtime.InteropServices;

namespace DolDoc.OpenGLHost
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EgaColor
    {
        public static EgaColor[] Palette = new[]
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
        }

        public byte R;
        public byte G;
        public byte B;
    }
}
