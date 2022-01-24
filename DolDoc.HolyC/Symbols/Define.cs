using System;
using WebAssembly;
using WebAssembly.Instructions;

namespace HolyScript.Compiler.Symbols
{
    public class Define : Symbol
    {
        public Define(string key, int value)
            : base(key, SymbolType.I32, false)
        {
            Value = value;
        }

        public int Value { get; }

        public override Instruction EmitLoad() => new Int32Constant(Value);

        public override Instruction EmitStore() => throw new NotSupportedException("Can't store to a Define");
    }
}
