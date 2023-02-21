using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ConstantNullNode : CodeNode
    {
        public override void Emit(LoggingILGenerator generator, SymbolTable symbolTable) =>
            generator.Emit(OpCodes.Ldnull);
    }
}