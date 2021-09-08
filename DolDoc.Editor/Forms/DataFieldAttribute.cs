using System;

namespace DolDoc.Editor.Forms
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataFieldAttribute : Attribute, IFieldAttribute
    {
        public DataFieldAttribute(string format, int maxLength = 64, string prefix = null, string suffix = "\n")
        {
            MaxLength = maxLength;
            Label = format;
            Prefix = prefix;
            Suffix = suffix;
        }

        public string Label { get; }

        public int MaxLength { get; }

        public string Prefix { get; }

        public string Suffix { get; }

        public string GetDolDocCommand(Type propertyType, string propertyName, int labelLength) => 
            $"{Prefix}$DA,A=\"{Label?.PadLeft(labelLength)}\",LEN={MaxLength},RT=\"{propertyType.Name}\",PROP=\"{propertyName}\"${Suffix}";
    }
}
