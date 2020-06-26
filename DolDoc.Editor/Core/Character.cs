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
        public byte Color;

        /// <summary>
        /// The actual character value.
        /// </summary>
        public byte Char;

        /// <summary>
        /// Represents the offset in the raw text buffer for this character.
        /// </summary>
        public int? TextOffset;

        public Character(byte ch, byte color, int? textOffset, CharacterFlags flags)
        {
            Char = ch;
            Color = color;
            Flags = flags;
            TextOffset = textOffset;
        }
    }
}
