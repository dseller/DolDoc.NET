using System.Runtime.InteropServices;

namespace DolDoc.Core.Editor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Character
    {
        public CharacterFlags Flags;
        public byte Color;
        public byte Char;

        public Character(byte ch, byte color, CharacterFlags flags)
        {
            Char = ch;
            Color = color;
            Flags = flags;
        }
    }
}
