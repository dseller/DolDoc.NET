using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ConstantIntegerAstNode : ConstantVariableAstNode
    {
        private readonly long value;

        public ConstantIntegerAstNode(long value)
        {
            this.value = value;
        }

        public override void EmitRead(LoggingILGenerator generator)
        {
            generator.Emit(OpCodes.Ldc_I8, value);
        }
    }
}
