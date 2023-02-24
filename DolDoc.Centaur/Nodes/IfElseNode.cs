using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class IfElseNode : ASTNode, IBytecodeEmitter
    {
        private readonly IBytecodeEmitter expression;
        private readonly IBytecodeEmitter body;
        private readonly IBytecodeEmitter elseBody;

        public IfElseNode(IBytecodeEmitter expression, IBytecodeEmitter body, IBytecodeEmitter elseBody)
        {
            this.expression = expression;
            this.body = body;
            this.elseBody = elseBody;
        }
        
        public void Emit(FunctionCompilerContext ctx)
        {
            var elseLabel = ctx.Generator.DefineLabel("else");
            var leaveLabel = ctx.Generator.DefineLabel("leave");

            expression.Emit(ctx);
            ctx.Generator.Emit(OpCodes.Brfalse, elseLabel);
            body.Emit(ctx);
            ctx.Generator.Emit(OpCodes.Br, leaveLabel);
            ctx.Generator.MarkLabel(elseLabel);
            elseBody.Emit(ctx);
            ctx.Generator.MarkLabel(leaveLabel);
        }

        public Type Type(ICompilerContext ctx) => null;
    }
}