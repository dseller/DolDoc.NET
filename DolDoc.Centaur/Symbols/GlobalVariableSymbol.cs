using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Symbols
{
    public class GlobalVariableSymbol : Symbol
    {
        private readonly FieldInfo fieldInfo;

        public GlobalVariableSymbol(string name, Type type, FieldInfo fieldInfo) : base(name, type)
        {
            this.fieldInfo = fieldInfo;
        }

        public override void EmitGet(FunctionCompilerContext ctx) =>
            ctx.Generator.Emit(OpCodes.Ldsfld, fieldInfo);

        public override void EmitSet(FunctionCompilerContext ctx) =>
            ctx.Generator.Emit(OpCodes.Stsfld, fieldInfo);
    }
}