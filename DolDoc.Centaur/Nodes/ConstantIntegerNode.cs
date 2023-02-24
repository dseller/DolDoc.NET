using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ConstantIntegerNode : ASTNode, IBytecodeEmitter
    {
        private readonly long value;

        public ConstantIntegerNode(long value)
        {
            this.value = value;
        }
        
        public void Emit(FunctionCompilerContext ctx) => ctx.Generator.Emit(OpCodes.Ldc_I8, value);
        
        public Type Type(ICompilerContext ctx) => typeof(long);
    }
}