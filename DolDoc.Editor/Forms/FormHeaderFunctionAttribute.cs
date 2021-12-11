using System;

namespace DolDoc.Editor.Forms
{
    public class FormHeaderFunctionAttribute : Attribute
    {
        public FormHeaderFunctionAttribute(string function)
        {
            Function = function;
        }

        public string Function { get; }
    }
}
