using System.Runtime.InteropServices;

namespace DolDoc.Core.Editor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Character
    {
        public CharacterFlags Flags;
        public EgaColor ForegroundColor, BackgroundColor;
        public char Char;

        public Character(char ch, EgaColor fgColor, EgaColor bgColor, CharacterFlags flags)
        {
            Char = ch;
            Flags = flags;
            ForegroundColor = fgColor;
            BackgroundColor = bgColor;
        }

        public static Character Empty(EgaColor fg, EgaColor bg) =>
            new Character((char)0x00, fg, bg, CharacterFlags.None);
    }
}
