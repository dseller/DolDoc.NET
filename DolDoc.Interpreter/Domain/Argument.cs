﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Interpreter.Domain
{
    public class Argument
    {
        public string Key { get; }

        public string Value { get; }

        public Argument(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
