namespace DolDoc.Editor.Core
{
    public class CombinedColor
    {
        public CombinedColor(byte color)
        {
            Value = color;
        }

        public CombinedColor(EgaColor background, EgaColor foreground)
        {
            Value = (byte)((((byte)foreground & 0x0F) << 4) | ((byte)background & 0x0F));
        }

        public EgaColor Background
        {
            get => (EgaColor)(Value & 0x0F);
            set => Value = (byte)((byte)(Value & 0xF0) | ((byte)value & 0x0F));
        }

        public EgaColor Foreground
        {
            get => (EgaColor)((Value >> 4) & 0x0F);
            set => Value = (byte)((byte)(Value & 0x0F) | (((byte)value & 0x0F) << 4));
        }

        public byte Value { get; set; }
    }
}
