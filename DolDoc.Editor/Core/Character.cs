// <copyright file="Character.cs" company="Dennis Seller">
// Copyright (c) Dennis Seller. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Runtime.InteropServices;

namespace DolDoc.Editor.Core
{
    /// <summary>
    /// Represents a character on the screen.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Character
    {
        public sbyte ShiftX;
        public sbyte ShiftY;

        /// <summary>
        /// Combined color byte, upper half is background color, lower half is foreground color.
        /// </summary>
        public CombinedColor Color;

        /// <summary>
        /// The actual character value.
        /// </summary>
        public byte Char;

        public Character(DocumentEntry entry, int relativeTextOffset, byte ch, CombinedColor color, CharacterFlags flags, byte layer = 0, sbyte shiftX = 0, sbyte shiftY = 0)
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

        /// <summary>
        /// Gets the flags for this character (e.g. underlined, blink, etc.)
        /// </summary>
        public CharacterFlags Flags { get; }

        public byte Layer { get; }

        /// <summary>
        /// Points to the <see cref="DocumentEntry"/> for this character.
        /// </summary>
        public DocumentEntry Entry { get; }

        /// <summary>
        /// The relative text offset, relative to the entry's text offset.
        /// </summary>
        public int RelativeTextOffset { get; }

        public bool HasEntry => Entry != null;
    }
}
