using System;

namespace DolDoc.HolyC
{
    public class Pointer<T>
    {
        public Pointer(T[] data, int offset = 0)
        {
            Data = data;
            Offset = offset;
        }

        public int Offset { get; set; }

        public T[] Data { get; set; }

        public T Dereference() => Data[Offset];

        public T[] GetArray()
        {
            // var result = new T[Data.Length - Offset];
            return Data[Offset..];
        }

        public static Pointer<T> operator +(Pointer<T> p, int offset) => new Pointer<T>(p.Data, p.Offset + offset);

        public static Pointer<T> operator -(Pointer<T> p, int offset) => new Pointer<T>(p.Data, p.Offset - offset);

        public static Pointer<T> operator ++(Pointer<T> p)
        {
            p.Offset++;
            return p;
        }

        public static Pointer<T> operator --(Pointer<T> p)
        {
            p.Offset--;
            return p;
        }

        public static implicit operator T(Pointer<T> p) => p.Dereference();
    }
}
