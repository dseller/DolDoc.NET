using System;

namespace DolDoc.Editor.Forms
{
    public class CheckboxFieldAttribute : Attribute, IFieldAttribute
    {
        private readonly string label;

        public CheckboxFieldAttribute(string label)
        {
            this.label = label;
        }

        public string GetDolDocCommand(Type propertyType) => $"$CB,\"{label}\"$\n";
    }
}
