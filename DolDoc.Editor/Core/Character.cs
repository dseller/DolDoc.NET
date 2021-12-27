﻿namespace DolDoc.Editor.Core
{
    /// <summary>
    /// Represents a character on the screen.
    /// </summary>
    public class Character
    {
        public Character(DocumentEntry entry, int relativeTextOffset, byte ch, CombinedColor color, CharacterFlags flags, byte layer = 0, sbyte shiftX = 0, sbyte shiftY = 0)
        {
            Char = ch;
            Color = color;
            Entry = entry;
            Flags = flags;
            RelativeTextOffset = relativeTextOffset;
            Dirty = true;

            Layer = layer;
            ShiftX = shiftX;
            ShiftY = shiftY;
        }

        /// <summary>
        /// The flags for this character (e.g. underlined, blink, etc.)
        /// </summary>
        public CharacterFlags Flags { get; private set; }

        public sbyte ShiftX;

        public sbyte ShiftY;

        public byte Layer { get; }

        /// <summary>
        /// Combined color byte, upper half is background color, lower half is foreground color.
        /// </summary>
        public CombinedColor Color;

        /// <summary>
        /// The actual character value.
        /// </summary>
        public byte Char;

        public bool Dirty { get; set; }

        /// <summary>
        /// Points to the <see cref="DocumentEntry"/> for this character.
        /// </summary>
        public DocumentEntry Entry { get; private set; }

        /// <summary>
        /// The relative text offset, relative to the entry's text offset.
        /// </summary>
        public int RelativeTextOffset { get; }

        public bool HasEntry => Entry != null;

        public void Reset()
        {
            Entry = null;
            Char = 0x00;
            Color = new CombinedColor(EgaColor.White, EgaColor.White);
            Flags = CharacterFlags.None;
            Dirty = true;
        }
    }
}
