using DolDoc.Interpreter.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Interpreter.Commands
{
    public interface IDolDocCommand
    {
        CommandResult Execute(IEnumerable<Flag> flags, IEnumerable<Argument> arguments);
    }
}
