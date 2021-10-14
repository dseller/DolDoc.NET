using DolDoc.Editor.Core;
using System;

namespace DolDoc.Editor.Forms
{
    public class CheckboxFieldAttribute : Attribute, IFieldAttribute
    {
        public CheckboxFieldAttribute(string label, string prefix = null, string suffix = "\n")
        {
            Label = label;
            Prefix = prefix;
            Suffix = suffix;
        }

        public string Label { get; }

        public string Prefix { get; }

        public string Suffix { get; }

        public string GetDolDocCommand(Type propertyType, string propertyName, int labelLength) => new CommandBuilder("CB")
            .WithPrefix(Prefix)
            .WithTag(Label.PadLeft(labelLength))
            .WithNamedParameter("PROP", propertyName)
            .WithSuffix(Suffix)
            .ToString();
    }
}
