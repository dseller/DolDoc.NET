using System;

namespace DolDoc.Editor.Forms
{
    public interface IFieldAttribute
    {
        string Label { get; }

        string Prefix { get; }

        string Suffix { get; }

        string GetDolDocCommand(Type propertyType, string propertyName, int labelLength);
    }
}
