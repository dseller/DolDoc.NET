namespace DolDoc.Editor.Core
{
    /// <summary>
    /// Represents a character on the screen.
    /// </summary>
    public class Character
    {
        private CombinedColor color;
        private sbyte shiftX, shiftY;
        private byte layer, ch;
        private CharacterFlags flags;

        public Character(int index)
        {
            Index = index;
        }

        public Character(byte ch, CombinedColor color)
        {
            Char = ch;
            Color = color;
        }
        
        public void Write(DocumentEntry entry, int relativeTextOffset, byte ch, CombinedColor color, CharacterFlags flags, byte layer = 0, sbyte shiftX = 0, sbyte shiftY = 0)
        {
            Char = ch;
            Color = color;
            Entry = entry;
            Flags = flags;
            RelativeTextOffset = relativeTextOffset;

            Layer = layer;
            ShiftX = shiftX;
            ShiftY = shiftY;
        }

        public int Index { get; }
        
        public bool Dirty { get; set; }

        /// <summary>
        /// The flags for this character (e.g. underlined, blink, etc.)
        /// </summary>
        public CharacterFlags Flags
        {
            get => flags;
            set
            {  
                if (flags != value)
                    Dirty = true;
                flags = value;
            }
        }

        public sbyte ShiftX
        {
            get => shiftX;
            set
            {
                if (shiftX != value)
                    Dirty = true;
                shiftX = value;
            }
        }

        public sbyte ShiftY
        {
            get => shiftY;
            set
            {
                if (shiftY != value)
                    Dirty = true;
                shiftY = value;
            }
        }

        public byte Layer
        {
            get => layer;
            set
            {
                if (layer != value)
                    Dirty = true;
                layer = value;
            }
        }

        /// <summary>
        /// Combined color byte, upper half is background color, lower half is foreground color.
        /// </summary>
        public CombinedColor Color
        {
            get => color;
            set
            {
                if (color != value)
                    Dirty = true;
                color = value;
            }
        }

        /// <summary>
        /// The actual character value.
        /// </summary>
        public byte Char
        {
            get => ch;
            set
            {
                if (ch != value)
                    Dirty = true;
                ch = value;
            }
        }

        /// <summary>
        /// Points to the <see cref="DocumentEntry"/> for this character.
        /// </summary>
        public DocumentEntry Entry { get; private set; }

        /// <summary>
        /// The relative text offset, relative to the entry's text offset.
        /// </summary>
        public int RelativeTextOffset { get; private set; }

        public bool HasEntry => Entry != null;
    }
}
