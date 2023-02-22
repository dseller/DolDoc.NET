using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Symbols
{
    public class ParameterSymbol : Symbol
    {
        public ParameterSymbol(string name, Type type, int index) : base(name, type)
        {
            Index = index;
        }
        
        public int Index { get; }

        public override void EmitGet(FunctionCompilerContext ctx) => ctx.Generator.Emit(OpCodes.Ldarg, Index);

        public override void EmitSet(FunctionCompilerContext ctx) => ctx.Generator.Emit(OpCodes.Starg, Index);
    }
}