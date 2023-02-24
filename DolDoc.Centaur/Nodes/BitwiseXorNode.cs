using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class BitwiseXorNode : ASTNode, IBytecodeEmitter
    {
        private readonly IBytecodeEmitter left;
        private readonly IBytecodeEmitter right;

        public BitwiseXorNode(IBytecodeEmitter left, IBytecodeEmitter right)
        {
            this.left = left;
            this.right = right;
        }

        public void Emit(FunctionCompilerContext ctx)
        {
            left.Emit(ctx);
            right.Emit(ctx);
            ctx.Generator.Emit(OpCodes.Xor);
        }

        public Type Type(ICompilerContext ctx) => left.Type(ctx);
    }
}