using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class GreaterThanOrEqualNode : ASTNode, IBytecodeEmitter
    {
        private readonly IBytecodeEmitter left;
        private readonly IBytecodeEmitter right;

        public GreaterThanOrEqualNode(IBytecodeEmitter left, IBytecodeEmitter right)
        {
            this.left = left;
            this.right = right;
        }
        
        public void Emit(FunctionCompilerContext ctx)
        {
            var label = ctx.Generator.DefineLabel("false");
            var endLabel = ctx.Generator.DefineLabel("end");
            left.Emit(ctx);
            right.Emit(ctx);
            ctx.Generator.Emit(OpCodes.Clt);
            ctx.Generator.Emit(OpCodes.Ldc_I4_0);
            ctx.Generator.Emit(OpCodes.Ceq);
            ctx.Generator.Emit(OpCodes.Brfalse, label);
            ctx.Generator.Emit(OpCodes.Ldc_I4_1);
            ctx.Generator.Emit(OpCodes.Br, endLabel);
            ctx.Generator.MarkLabel(label);
            ctx.Generator.Emit(OpCodes.Ldc_I4_0);
            ctx.Generator.MarkLabel(endLabel);
        }

        public Type Type(ICompilerContext ctx) => typeof(bool);
    }
}