using System;

namespace DolDoc.Editor.Forms
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValueFieldAttribute : Attribute, IFieldAttribute
    {
        public ValueFieldAttribute(string label = null, string prefix = null, string suffix = "\n")
        {
            Label = label;
            Prefix = prefix;
            Suffix = suffix;
        }

        public string Label { get; }

        public string Prefix { get; }

        public string Suffix { get; }

        public string GetDolDocCommand(Type propertyType, string propertyName, int labelLength) =>
            $"{Prefix}$VA,A=\"{Label}\",PROP=\"{propertyName}\"${Suffix}";
    }
}
