using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAssembly;
using WebAssembly.Instructions;

namespace HolyScript.Compiler.Symbols
{
    public class Parameter : Symbol
    {
        public Parameter(List<WebAssemblyValueType> parameters, string name, SymbolType type, bool isPointer) : base(name, type, isPointer)
        {
            ParamIdx = (uint)parameters.Count;
        }

        public uint ParamIdx { get; }

        public Local AsLocal => new Local
        {
            Count = 1,
            Type = HolyCVisitor.GetWasmType(Type.ToString()).Value
        };

        public override Instruction EmitLoad() => new LocalGet(ParamIdx);

        public override Instruction EmitStore() => new LocalSet(ParamIdx);
    }
}
