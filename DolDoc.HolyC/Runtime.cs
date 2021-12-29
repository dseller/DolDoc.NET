using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolDoc.HolyC
{
    public static partial class Runtime
    {
        private static List<byte[]> memory = new List<byte[]>();

        public static void Puts(string str)
        {
            Console.WriteLine(str);
        }

        public static void Kak()
        {
            Console.WriteLine("Yay!");
        }

        public static void Print(string str)
        {
            Console.WriteLine($"Print: '{str}'");
        }

        public static byte[] MAlloc(int size)
        {
            var res = new byte[size];
            Console.WriteLine("Allocated {0} bytes", size);
            memory.Add(res);
            return res;
        }

        public static void StrCpy(byte[] dst, byte[] src)
        {

        }

        /// <summary>
        /// Compare two strings.
        /// </summary>
        public static long StrCmp(string st1, string st2) => string.Compare(st1, st2, false);

        /// <summary>
        /// Compare two strings, ignoring case.
        /// </summary>
        public static long StrICmp(string st1, string st2) => string.Compare(st1, st2, true);

        /// <summary>
        /// Compare N bytes in two strings.
        /// </summary>
        public static long StrNCmp(string st1, string st2, long n) => string.Compare(st1, 0, st2, 0, (int)n);

        /// <summary>
        /// Compare N bytes in two strings, ignoring case.
        /// </summary>
        public static long StrNICmp(string st1, string st2, long n) => string.Compare(st1, 0, st2, 0, (int)n, true);

        /// <summary>
        /// Scan for string in string.
        /// </summary>
        public static long StrMatch(string needle, string haystack) => haystack?.IndexOf(needle) ?? 0;

        /// <summary>
        /// Scan for string in string, ignoring case.
        /// </summary>
        public static long StrIMatch(string needle, string haystack) => haystack?.IndexOf(needle, StringComparison.InvariantCultureIgnoreCase) ?? 0;

        public static void Free(byte[] data)
        {
            Console.WriteLine("Freeing {0} bytes", data.Length);

            memory.Remove(data);
        }
    }
}
