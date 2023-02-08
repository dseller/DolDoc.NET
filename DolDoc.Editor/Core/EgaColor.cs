namespace DolDoc.Editor.Core
{
    public enum EgaColor : byte
    {
        Black,
        Blue,
        Green,
        Cyan,
        Red,
        Purple,
        Brown,
        LtGray,
        DkGray,
        LtBlue,
        LtGreen,
        LtCyan,
        LtRed,
        LtPurple,
        Yellow,
        White
    }

    public class EgaColorRgbBitmap
    {
        public static readonly EgaColorRgbBitmap[] Palette = new[]                                                                             
        {
            new EgaColorRgbBitmap(0x00, 0x00, 0x00),
            new EgaColorRgbBitmap(0x00, 0x00, 0xAA),
            new EgaColorRgbBitmap(0x00, 0xAA, 0x00),
            new EgaColorRgbBitmap(0x00, 0xAA, 0xAA),
            new EgaColorRgbBitmap(0xAA, 0x00, 0x00),
            new EgaColorRgbBitmap(0xAA, 0x00, 0xAA),
            new EgaColorRgbBitmap(0xAA, 0x55, 0x00),
            new EgaColorRgbBitmap(0xAA, 0xAA, 0xAA),
            new EgaColorRgbBitmap(0x55, 0x55, 0x55),
            new EgaColorRgbBitmap(0x55, 0x55, 0xFF),
            new EgaColorRgbBitmap(0x55, 0xFF, 0x55),
            new EgaColorRgbBitmap(0x55, 0xFF, 0xFF),
            new EgaColorRgbBitmap(0xFF, 0x55, 0x55),
            new EgaColorRgbBitmap(0xFF, 0x55, 0xFF),
            new EgaColorRgbBitmap(0xFF, 0xFF, 0x55),
            new EgaColorRgbBitmap(0xFF, 0xFF, 0xFF)
        };

        public EgaColorRgbBitmap(byte r, byte g, byte b)
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