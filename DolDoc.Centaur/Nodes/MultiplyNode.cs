using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class MultiplyNode : ASTNode, IBytecodeEmitter
    {
        private readonly IBytecodeEmitter left;
        private readonly IBytecodeEmitter right;

        public MultiplyNode(IBytecodeEmitter left, IBytecodeEmitter right)
        {
            this.left = left;
            this.right = right;
        }

        public void Emit(FunctionCompilerContext ctx)
        {
            left.Emit(ctx);
            right.Emit(ctx);
            ctx.Generator.Emit(OpCodes.Mul);
        }

        public Type Type(ICompilerContext ctx) => left.Type(ctx);
    }
}