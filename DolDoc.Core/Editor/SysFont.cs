﻿namespace DolDoc.Core.Editor
{
    public static class SysFont
    {
        public static ulong[] Font =
        {
            0x0000000000000000,0x0000000000000000,
            0x000000FF00000000,0x000000FF00FF0000,//..
            0x1818181818181818,0x6C6C6C6C6C6C6C6C,//..
            0x181818F800000000,0x6C6C6CEC0CFC0000,//..
            0x1818181F00000000,0x6C6C6C6F607F0000,//..
            0x000000F818181818,0x000000FC0CEC6C6C,//..
            0x0000001F18181818,0x0000007F606F6C6C,//..
            0x0000000000000000,0x0000000000000000,
            0x0000000000000000,0x0000000000000000,
            0x0000000000000000,0x0000000000000000,
            0x0000000000000000,0x0000000000000000,
            0x0000000000000000,0x0000000000000000,
            0x0000000000000000,0x0000000000000000,
            0x0000000000000000,0x0000000000000000,
            0x0000000000000000,0x0000000000000000,
            0x0000000000000000,0x0008000000000000,//
            0x0000000000000000,0x00180018183C3C18,// !
            0x0000000000363636,0x006C6CFE6CFE6C6C,//"#
            0x00187ED07C16FC30,0x0060660C18306606,//$%
            0x00DC66B61C36361C,0x0000000000181818,//&'
            0x0030180C0C0C1830,0x000C18303030180C,//()
            0x0000187E3C7E1800,0x000018187E181800,//*+
            0x0C18180000000000,0x000000007E000000,//,-
            0x0018180000000000,0x0000060C18306000,//./
            0x003C666E7E76663C,0x007E181818181C18,//01
            0x007E0C183060663C,0x003C66603860663C,//23
            0x0030307E363C3830,0x003C6660603E067E,//45
            0x003C66663E060C38,0x000C0C0C1830607E,//67
            0x003C66663C66663C,0x001C30607C66663C,//89
            0x0018180018180000,0x0C18180018180000,//:;
            0x0030180C060C1830,0x0000007E007E0000,//<=
            0x000C18306030180C,0x001800181830663C,//>?
            0x003C06765676663C,0x006666667E66663C,//@A
            0x003E66663E66663E,0x003C66060606663C,//BC
            0x001E36666666361E,0x007E06063E06067E,//DE
            0x000606063E06067E,0x003C66667606663C,//FG
            0x006666667E666666,0x007E18181818187E,//HI
            0x001C36303030307C,0x0066361E0E1E3666,//JK
            0x007E060606060606,0x00C6C6D6D6FEEEC6,//LM
            0x006666767E6E6666,0x003C66666666663C,//NO
            0x000606063E66663E,0x006C36566666663C,//PQ
            0x006666363E66663E,0x003C66603C06663C,//RS
            0x001818181818187E,0x003C666666666666,//TU
            0x00183C6666666666,0x00C6EEFED6D6C6C6,//VW
            0x0066663C183C6666,0x001818183C666666,//XY
            0x007E060C1830607E,0x003E06060606063E,//Z[
            0x00006030180C0600,0x007C60606060607C,//\]
            0x000000000000663C,0xFFFF000000000000,//^_
            0x000000000030180C,0x007C667C603C0000,//`a
            0x003E6666663E0606,0x003C6606663C0000,//bc
            0x007C6666667C6060,0x003C067E663C0000,//de
            0x000C0C0C3E0C0C38,0x3C607C66667C0000,//fg
            0x00666666663E0606,0x003C1818181C0018,//hi
            0x0E181818181C0018,0x0066361E36660606,//jk
            0x003C18181818181C,0x00C6D6D6FE6C0000,//lm
            0x00666666663E0000,0x003C6666663C0000,//no
            0x06063E66663E0000,0xE0607C66667C0000,//pq
            0x000606066E360000,0x003E603C067C0000,//rs
            0x00380C0C0C3E0C0C,0x007C666666660000,//tu
            0x00183C6666660000,0x006CFED6D6C60000,//vw
            0x00663C183C660000,0x3C607C6666660000,//xy
            0x007E0C18307E0000,0x003018180E181830,//z{
            0x0018181818181818,0x000C18187018180C,//|}
            0x000000000062D68C,0xFFFFFFFFFFFFFFFF,//~
            0x1E30181E3303331E,0x007E333333003300,//..
            0x001E033F331E0038,0x00FC667C603CC37E,//..
            0x007E333E301E0033,0x007E333E301E0007,//..
            0x007E333E301E0C0C,0x3C603E03033E0000,//..
            0x003C067E663CC37E,0x001E033F331E0033,//..
            0x001E033F331E0007,0x001E0C0C0C0E0033,//..
            0x003C1818181C633E,0x001E0C0C0C0E0007,//..
            0x00333F33331E0C33,0x00333F331E000C0C,//..
            0x003F061E063F0038,0x00FE33FE30FE0000,//..
            0x007333337F33367C,0x001E33331E00331E,//..
            0x001E33331E003300,0x001E33331E000700,//..
            0x007E33333300331E,0x007E333333000700,//..
            0x1F303F3333003300,0x001C3E63633E1C63,//..
            0x001E333333330033,0x18187E03037E1818,//..
            0x003F67060F26361C,0x000C3F0C3F1E3333,//..
            0x70337B332F1B1B0F,0x0E1B18187E18D870,//..
            0x007E333E301E0038,0x001E0C0C0C0E001C,//..
            0x001E33331E003800,0x007E333333003800,//..
            0x003333331F001F00,0x00333B3F3733003F,//..
            0x00007E007C36363C,0x00007E003C66663C,//..
            0x001E3303060C000C,0x000003033F000000,//..
            0x000030303F000000,0xF81973C67C1B3363,//..
            0xC0F9F3E6CF1B3363,0x183C3C1818001800,//..
            0x0000CC663366CC00,0x00003366CC663300,//..
            0x1144114411441144,0x55AA55AA55AA55AA,//..
            0xEEBBEEBBEEBBEEBB,0x1818181818181818,//..
            0x1818181F18181818,0x1818181F181F1818,//..
            0x6C6C6C6F6C6C6C6C,0x6C6C6C7F00000000,//..
            0x1818181F181F0000,0x6C6C6C6F606F6C6C,//..
            0x6C6C6C6C6C6C6C6C,0x6C6C6C6F607F0000,//..
            0x0000007F606F6C6C,0x0000007F6C6C6C6C,//..
            0x0000001F181F1818,0x1818181F00000000,//..
            0x000000F818181818,0x000000FF18181818,//..
            0x181818FF00000000,0x181818F818181818,//..
            0x000000FF00000000,0x181818FF18181818,//..
            0x181818F818F81818,0x6C6C6CEC6C6C6C6C,//..
            0x000000FC0CEC6C6C,0x6C6C6CEC0CFC0000,//..
            0x000000FF00EF6C6C,0x6C6C6CEF00FF0000,//..
            0x6C6C6CEC0CEC6C6C,0x000000FF00FF0000,//..
            0x6C6C6CEF00EF6C6C,0x000000FF00FF1818,//..
            0x000000FF6C6C6C6C,0x181818FF00FF0000,//..
            0x6C6C6CFF00000000,0x000000FC6C6C6C6C,//..
            0x000000F818F81818,0x181818F818F80000,//..
            0x6C6C6CFC00000000,0x6C6C6CEF6C6C6C6C,//..
            0x181818FF00FF1818,0x0000001F18181818,//..
            0x181818F800000000,0xFFFFFFFFFFFFFFFF,//..
            0xFFFFFFFF00000000,0x0F0F0F0F0F0F0F0F,//..
            0xF0F0F0F0F0F0F0F0,0x00000000FFFFFFFF,//..
            0x006E3B133B6E0000,0x03031F331F331E00,//..
            0x0003030303637F00,0x0036363636367F00,//.pi
            0x007F660C180C667F,0x001E3333337E0000,//..
            0x03063E6666666600,0x00181818183B6E00,//u.
            0x3F0C1E33331E0C3F,0x001C36637F63361C,//phitheta
            0x007736366363361C,0x001E33333E180C38,//omega.
            0x00007EDBDB7E0000,0x03067EDBDB7E3060,//inf.
            0x003C06033F03063C,0x003333333333331E,//..
            0x00003F003F003F00,0x003F000C0C3F0C0C,//..
            0x003F00060C180C06,0x003F00180C060C18,//..
            0x1818181818D8D870,0x0E1B1B1818181818,//..
            0x000C0C003F000C0C,0x0000394E00394E00,//..
            0x000000001C36361C,0x0000001818000000,//..
            0x0000001800000000,0x383C3637303030F0,//..
            0x000000363636361E,0x0000003E061C301E,//..
            0x00003C3C3C3C0000,0xFFFFFFFFFFFFFFFF,//..
        };
    }
}
