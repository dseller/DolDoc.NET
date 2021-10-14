using DolDoc.Editor.Core;
using System;

namespace DolDoc.Editor.Forms
{
    public class ButtonFieldAttribute : Attribute, IFieldAttribute
    {
        public ButtonFieldAttribute(string label, string handlerMethod, string prefix = "\n ", string suffix = "\n")
        {
            Label = label;
            Prefix = prefix;
            Suffix = suffix;
            HandlerMethod = handlerMethod;
        }

        public string Label { get; }

        public string HandlerMethod { get; }

        public string Prefix { get; }

        public string Suffix { get; }

        public string GetDolDocCommand(Type propertyType, string propertyName, int labelLength) => new CommandBuilder("BT")
            .WithPrefix(Prefix)
            .WithTag(Label)
            .WithNamedParameter("H", HandlerMethod, !string.IsNullOrEmpty(HandlerMethod))
            .WithSuffix(Suffix)
            .ToString();
    }
}
