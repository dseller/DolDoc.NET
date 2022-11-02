namespace DolDoc.Editor.Extensions
{
    public static class ByteExtensions
    {
        public static byte Reverse(this byte value)
        {
            value = (byte) ((value & 0xF0) >> 4 | (value & 0x0F) << 4);
            value = (byte) ((value & 0xCC) >> 2 | (value & 0x33) << 2);
            value = (byte) ((value & 0xAA) >> 1 | (value & 0x55) << 1);
            return value;
        }
    }
}