using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAssembly;

namespace HolyScript.Compiler.Symbols
{
    public abstract class Symbol
    {
        protected Symbol(string name, SymbolType type, bool isPointer)
        {
            Name = name;
            Type = type;
            IsPointer = isPointer;
        }

        public string Name { get; }

        public SymbolType Type { get; }

        public bool IsPointer { get; }

        public abstract Instruction EmitLoad();

        public abstract Instruction EmitStore();
    }
}
