using System;

namespace DolDoc.Editor.Entries
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntryAttribute : Attribute
    {
        public EntryAttribute(string command)
        {
            Command = command;
        }

        public string Command { get; }
    }
}
