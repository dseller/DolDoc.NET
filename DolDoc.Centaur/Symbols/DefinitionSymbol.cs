using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Symbols
{
    public class DefinitionSymbol : Symbol
    {
        private readonly long value;

        public DefinitionSymbol(string name, Type type, long value) : base(name, type)
        {
            this.value = value;
        }

        public override void EmitGet(FunctionCompilerContext ctx)
        {
            ctx.Generator.Emit(OpCodes.Ldc_I8, value);
        }

        public override void EmitSet(FunctionCompilerContext ctx)
        {
            throw new NotImplementedException();
        }
    }
}