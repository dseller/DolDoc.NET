using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ConstantDoubleNode : ASTNode, IBytecodeEmitter
    {
        private readonly double value;

        public ConstantDoubleNode(double value)
        {
            this.value = value;
        }

        public void Emit(FunctionCompilerContext ctx) => ctx.Generator.Emit(OpCodes.Ldc_R8, value);
        public Type Type(ICompilerContext ctx) => typeof(double);
    }
}