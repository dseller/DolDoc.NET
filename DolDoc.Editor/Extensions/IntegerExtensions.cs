using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Extensions
{
    public static class IntegerExtensions
    {
        public static int Clamp(this int value, int min, int max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }
    }
}
