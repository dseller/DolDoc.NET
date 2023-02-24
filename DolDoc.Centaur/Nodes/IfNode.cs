using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class IfNode : ASTNode, IBytecodeEmitter
    {
        private readonly IBytecodeEmitter expression;
        private readonly IBytecodeEmitter body;

        public IfNode(IBytecodeEmitter expression, IBytecodeEmitter body)
        {
            this.expression = expression;
            this.body = body;
        }
        
        public void Emit(FunctionCompilerContext ctx)
        {
            var label = ctx.Generator.DefineLabel("skip");

            expression.Emit(ctx);
            ctx.Generator.Emit(OpCodes.Brfalse, label);
            body.Emit(ctx);
            ctx.Generator.MarkLabel(label);
        }

        public Type Type(ICompilerContext ctx) => null;
    }
}