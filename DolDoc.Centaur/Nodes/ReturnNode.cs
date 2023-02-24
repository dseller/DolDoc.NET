using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ReturnNode : ASTNode, IBytecodeEmitter
    {
        private readonly IBytecodeEmitter result;

        public ReturnNode(IBytecodeEmitter result)
        {
            this.result = result;
        }
        
        public void Emit(FunctionCompilerContext ctx)
        {
            if (result != null)
                result.Emit(ctx);
            ctx.Generator.Emit(OpCodes.Ret);
        }

        public Type Type(ICompilerContext ctx) => null;
    }
}