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

        public static void Print(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes);
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

        public static void Free(byte[] data)
        {
            Console.WriteLine("Freeing {0} bytes", data.Length);

            memory.Remove(data);
        }
    }
}
