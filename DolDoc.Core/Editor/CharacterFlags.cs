using System;

namespace DolDoc.Core.Editor
{
    [Flags]
    public enum CharacterFlags : byte
    {
        None =          0b00000000,
        Underline =     0b00000001,
        Hold =          0b00000010,

        Center =        0b00000100,
        Right =         0b00001000,
        Left =          0b00001100,

        WordWrap =      0b00010000
    }
}
