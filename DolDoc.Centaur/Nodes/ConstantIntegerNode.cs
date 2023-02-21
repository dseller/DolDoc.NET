using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ConstantIntegerNode : CodeNode
    {
        private readonly long value;

        public ConstantIntegerNode(long value)
        {
            this.value = value;
        }

        public override void Emit(LoggingILGenerator generator, SymbolTable symbolTable) => generator.Emit(OpCodes.Ldc_I8, value);
    }
}