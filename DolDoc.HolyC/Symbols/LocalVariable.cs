using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAssembly;
using WebAssembly.Instructions;

namespace HolyScript.Compiler.Symbols
{
    public class LocalVariable : Symbol
    {
        public LocalVariable(FunctionBody body, int paramCount, string name, SymbolType type, bool isPointer, uint count = 1) : base(name, type, isPointer)
        {
            LocalIdx = (uint)(body.Locals.Count + paramCount);
            Count = count;
        }

        public uint LocalIdx { get; }

        public Local AsLocal => new Local
        {
            Count = Count,
            Type = HolyCVisitor.GetWasmType(Type.ToString()).Value
        };

        public uint Count { get; }

        public override Instruction EmitLoad() => new LocalGet(LocalIdx);

        public override Instruction EmitStore() => new LocalSet(LocalIdx);
    }
}
