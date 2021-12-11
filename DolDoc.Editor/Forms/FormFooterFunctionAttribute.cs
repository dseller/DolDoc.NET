using System;

namespace DolDoc.Editor.Forms
{
    public class FormFooterFunctionAttribute : Attribute
    {
        public FormFooterFunctionAttribute(string function)
        {
            Function = function;
        }

        public string Function { get; }
    }
}
