using System;

namespace DolDoc.Editor.Forms
{
    public class FormFooterAttribute : Attribute
    {
        public FormFooterAttribute(string footer)
        {
            Footer = footer;
        }

        public string Footer { get; }
    }
}
