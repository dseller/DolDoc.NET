using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ConstantStringNode : CodeNode
    {
        private readonly string value;

        public ConstantStringNode(string value)
        {
            this.value = value;
        }

        public override void Emit(LoggingILGenerator generator, SymbolTable symbolTable) =>
            generator.Emit(OpCodes.Ldstr, value);
    }
}