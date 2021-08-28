using System;

namespace DolDoc.Editor.Forms
{
    public interface IFieldAttribute
    {
        string GetDolDocCommand(Type propertyType);
    }
}
