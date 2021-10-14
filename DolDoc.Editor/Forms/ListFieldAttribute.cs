using DolDoc.Editor.Core;
using System;

namespace DolDoc.Editor.Forms
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ListFieldAttribute : Attribute, IFieldAttribute
    {
        public ListFieldAttribute(string label, Type enumType, string prefix = null, string suffix = "\n")
        {
            Label = label;
            EnumType = enumType;
            Prefix = prefix;
            Suffix = suffix;
            Source = ListFieldSource.Enum;
        }

        public ListFieldAttribute(string label, string callback, string prefix = null, string suffix = "\n")
        {
            Label = label;
            Callback = callback;
            Prefix = prefix;
            Suffix = suffix;
            Source = ListFieldSource.Callback;
        }

        public ListFieldSource Source { get; }

        public string Label { get; }

        public string Callback { get; }

        public Type EnumType { get; }

        public string Prefix { get; }

        public string Suffix { get; }

        public string GetDolDocCommand(Type propertyType, string propertyName, int labelLength) => new CommandBuilder("LS")
            .WithPrefix(Prefix)
            .WithNamedParameter("A", Label?.PadLeft(labelLength))
            .WithNamedParameter("TYPE", Source.ToString())
            .WithNamedParameter("SRC", Source == ListFieldSource.Enum ? EnumType.AssemblyQualifiedName : Callback)
            .WithNamedParameter("PROP", propertyName)
            .WithSuffix(Suffix)
            .ToString();
    }
}
