
using System;
using System.Linq;

namespace DolDoc.Centaur.Internals
{
    public class Strings
    {
        public static long StrCmp(string st1, string st2) =>
            string.Compare(st1, st2, StringComparison.InvariantCulture);
        
        public static long StrICmp(string st1, string st2) =>
            string.Compare(st1, st2, StringComparison.InvariantCultureIgnoreCase);

        public static long StrNCmp(string st1, string st2, long n) =>
            string.Compare(st1, 0, st2, 0, (int)n, StringComparison.InvariantCulture);
        
        public static long StrNICmp(string st1, string st2, long n) =>
            string.Compare(st1, 0, st2, 0, (int)n, StringComparison.InvariantCultureIgnoreCase);

        public static long StrOcc(string src, long ch) =>
            src.Count(c => c == (char)ch);

        public static long StrLen(string str) => str.Length;

        public static long ToUpper(long ch) => char.ToUpper((char) ch);
    }
}
