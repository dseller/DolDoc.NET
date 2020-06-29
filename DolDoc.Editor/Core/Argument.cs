using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Core
{
    public class Argument
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public Argument(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
