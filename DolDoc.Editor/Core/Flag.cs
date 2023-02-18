﻿namespace DolDoc.Editor.Core
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
