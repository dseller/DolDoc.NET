using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ConstantNullNode : ASTNode, IBytecodeEmitter
    {
        public void Emit(FunctionCompilerContext ctx) =>
            ctx.Generator.Emit(OpCodes.Ldnull);

        public Type Type => typeof(object);
    }
}