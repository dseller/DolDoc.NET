using System.Runtime.InteropServices;

namespace DolDoc.Editor.Core
{
    /// <summary>
    /// Represents a character on the screen.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Character
    {
        /// <summary>
        /// The flags for this character (e.g. underlined, blink, etc.)
        /// </summary>
        public CharacterFlags Flags { get; }

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

        /// <summary>
        /// Combined color byte, upper half is background color, lower half is foreground color.
        /// </summary>
        public CombinedColor Color;

        /// <summary>
        /// The actual character value.
        /// </summary>
        public byte Char;

        /// <summary>
        /// Points to the <see cref="DocumentEntry"/> for this character.
        /// </summary>
        public DocumentEntry Entry { get; }

        /// <summary>
        /// The relative text offset, relative to the entry's text offset.
        /// </summary>
        public int RelativeTextOffset { get; }

        public int AbsoluteTextOffset => Entry.TextOffset + RelativeTextOffset;

        public bool HasEntry => Entry != null;

        public Character(DocumentEntry entry, int relativeTextOffset, byte ch, CombinedColor color, CharacterFlags flags, byte shiftX = 0, byte shiftY = 0)
        {
            _shiftPosition = 0;
            
            Char = ch;
            Color = color;
            Entry = entry;
            Flags = flags;
            RelativeTextOffset = relativeTextOffset;

            ShiftX = shiftX;
            ShiftY = shiftY;
        }
    }
}
