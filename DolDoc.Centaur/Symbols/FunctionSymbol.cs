using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Symbols
{
    public class FunctionSymbol : Symbol
    {
        public FunctionSymbol(string name, Type type, Type[] parameters, MethodInfo methodInfo) : base(name, type)
        {
            Parameters = parameters;
            MethodInfo = methodInfo;
        }
        
        public Type[] Parameters { get; }
        
        public MethodInfo MethodInfo { get; }
        
        public Type CodeType { get; internal set; }

        public override void EmitGet(FunctionCompilerContext ctx) => ctx.Generator.Emit(OpCodes.Ldobj, MethodInfo);

        public override void EmitSet(FunctionCompilerContext ctx)
        {
            throw new NotImplementedException();
        }
    }
}