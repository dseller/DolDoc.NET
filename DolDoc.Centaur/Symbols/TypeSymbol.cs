using System;

namespace DolDoc.Centaur.Symbols
{
    public class TypeSymbol : Symbol
    {
        public TypeSymbol(string name, Type type) : base(name, type)
        {
        }

        public override void EmitGet(FunctionCompilerContext ctx) =>
            throw new NotSupportedException();

        public override void EmitSet(FunctionCompilerContext ctx) =>
            throw new NotSupportedException();
    }
}