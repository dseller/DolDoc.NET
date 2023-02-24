using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ConstantBooleanNode : ASTNode, IBytecodeEmitter
    {
        private readonly bool value;

        public ConstantBooleanNode(bool value)
        {
            this.value = value;
        }

        public void Emit(FunctionCompilerContext ctx)
        {
            if (value)
                ctx.Generator.Emit(OpCodes.Ldc_I4_1);
            else
                ctx.Generator.Emit(OpCodes.Ldc_I4_0);
        }

        public Type Type(ICompilerContext ctx) => typeof(bool);
    }
}