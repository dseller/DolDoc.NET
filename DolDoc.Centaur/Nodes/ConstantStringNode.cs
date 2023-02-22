using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ConstantStringNode : ASTNode, IBytecodeEmitter
    {
        private readonly string value;

        public ConstantStringNode(string value)
        {
            this.value = value;
        }

        public void Emit(FunctionCompilerContext ctx) =>
            ctx.Generator.Emit(OpCodes.Ldstr, value);

        public Type Type => typeof(string);
    }
}