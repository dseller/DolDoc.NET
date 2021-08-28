using System;

namespace DolDoc.Editor.Forms
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataFieldAttribute : Attribute, IFieldAttribute
    {
        public DataFieldAttribute(string format, int maxLength = 64)
        {
            MaxLength = maxLength;
            FormatString = format;
        }

        public string FormatString { get; }

        public int MaxLength { get; }

        public string GetDolDocCommand(Type propertyType) => $"$DA,A=\"{FormatString}\",LEN={MaxLength},RT=\"{propertyType.Name}\"$\n";
    }
}
