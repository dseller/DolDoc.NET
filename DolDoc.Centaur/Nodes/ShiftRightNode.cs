using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ShiftRightNode : ASTNode, IBytecodeEmitter
    {
        private readonly IBytecodeEmitter left;
        private readonly IBytecodeEmitter right;

        public ShiftRightNode(IBytecodeEmitter left, IBytecodeEmitter right)
        {
            this.left = left;
            this.right = right;
        }
        
        public void Emit(FunctionCompilerContext ctx)
        {
            left.Emit(ctx);
            right.Emit(ctx);
            ctx.Generator.Emit(OpCodes.Shr_Un);
        }
        
        public Type Type => left.Type;
    }
}