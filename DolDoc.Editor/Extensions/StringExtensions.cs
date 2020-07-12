namespace DolDoc.Editor.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the index of the nearest whitespace, with the specified offset.
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="offset">The offset within the string</param>
        /// <returns>The index of the whitespace</returns>
        public static int? IndexOfWhitespace(this string str, int offset = 0)
        {
            for (int i = offset; i < str.Length; i++)
            {
                if (char.IsWhiteSpace(str[i]))
                    return i;
            }

            return null;
        }
    }
}
