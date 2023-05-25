namespace DolDoc.Centaur.Internals
{
    public class Math
    {
        public static double ATan(double n) => System.Math.Atan(n);
        public static double Abs(double n) => System.Math.Abs(n);
        public static double AbsI64(long i) => System.Math.Abs(i);
        public static double Cos(double n) => System.Math.Cos(n);
        public static double MaxI64(long n1, long n2) => System.Math.Max(n1, n2);
        public static double MaxU64(ulong n1, ulong n2) => System.Math.Max(n1, n2);
        public static double MinI64(long n1, long n2) => System.Math.Min(n1, n2);
        public static double MinU64(ulong n1, ulong n2) => System.Math.Min(n1, n2);
        public static double Sin(double n) => System.Math.Sin(n);
        public static double Sqr(double n) => System.Math.Sqrt(n);
        public static double SqrI64(long n) => System.Math.Sqrt(n);
        public static double SqrU64(ulong n) => System.Math.Sqrt(n);
        public static double Tan(double n) => System.Math.Tan(n);
        public static double Ceil(double n) => System.Math.Ceiling(n);
        public static double Floor(double n) => System.Math.Floor(n);
        public static double Ln(double n) => System.Math.Log(n);
        public static double Log10(double n) => System.Math.Log10(n);
        public static double Log2(double n) => System.Math.Log2(n);
        public static double Round(double n) => System.Math.Round(n);
        public static double Trunc(double n) => System.Math.Truncate(n);
        


    }
}