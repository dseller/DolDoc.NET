using DolDoc.Interpreter.Commands;
using DolDoc.Interpreter.Domain;
using System;
using System.IO;

namespace DolDoc.Interpreter.Parser
{
    public interface IDolDocParser
    {
        event Action<char> OnWriteCharacter;

        event Action<string> OnWriteString;

        event Action<Command> OnCommand;

        void Parse(Stream input);
    }
}
