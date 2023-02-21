using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ReturnNode : CodeNode
    {
        private readonly CodeNode result;

        public ReturnNode(CodeNode result)
        {
            this.result = result;
        }
        
        public override void Emit(LoggingILGenerator generator, SymbolTable symbolTable)
        {
            if (result != null)
                result.Emit(generator, symbolTable);
            generator.Emit(OpCodes.Ret);
        }
    }
}