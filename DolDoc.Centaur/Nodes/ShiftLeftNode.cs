using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ShiftLeftNode : ASTNode, IBytecodeEmitter
    {
        private readonly IBytecodeEmitter left;
        private readonly IBytecodeEmitter right;

        public ShiftLeftNode(IBytecodeEmitter left, IBytecodeEmitter right)
        {
            this.left = left;
            this.right = right;
        }
        
        public void Emit(FunctionCompilerContext ctx)
        {
            left.Emit(ctx);
            right.Emit(ctx);
            ctx.Generator.Emit(OpCodes.Shl);
        }
        
        public Type Type(ICompilerContext ctx) => left.Type(ctx);
    }
}