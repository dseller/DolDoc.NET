﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Interpreter.Domain
{
    public class Flag
    {
        public bool Status { get; }

        public string Value { get; }

        public Flag(bool status, string value)
        {
            Status = status;
            Value = value;
        }
    }
}
