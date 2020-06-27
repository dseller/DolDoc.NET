using System.Runtime.InteropServices;

namespace DolDoc.Editor.Core
{
    /// <summary>
    /// Represents a character on the screen.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Character
    {
        public CharacterFlags Flags;

        private byte _shiftPosition;

        public byte ShiftX
        {
            get => (byte)(_shiftPosition & 0x0F);
            set => _shiftPosition |= (byte)(value & 0x0F);
        }

        public byte ShiftY
        {
            get => (byte)((_shiftPosition & 0xF0) >> 4);
            set => _shiftPosition |= (byte)((value << 4) & 0xF0);
        }

        public byte Color;

        /// <summary>
        /// The actual character value.
        /// </summary>
        public byte Char;

        /// <summary>
        /// Represents the offset in the raw text buffer for this character.
        /// </summary>
        public int? TextOffset;

        public Character(byte ch, byte color, int? textOffset, CharacterFlags flags, byte shiftX = 0, byte shiftY = 0)
        {
            _shiftPosition = 0;
            Char = ch;
            Color = color;
            Flags = flags;
            TextOffset = textOffset;
            ShiftX = shiftX;
            ShiftY = shiftY;
        }
    }
}
