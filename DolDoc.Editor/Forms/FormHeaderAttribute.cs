using System;

namespace DolDoc.Editor.Forms
{
    public class FormHeaderAttribute : Attribute
    {
        public FormHeaderAttribute(string header)
        {
            Header = header;
        }

        public string Header { get; }
    }
}
