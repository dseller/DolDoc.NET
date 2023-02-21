using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ShiftLeftNode : CodeNode
    {
        private readonly CodeNode left;
        private readonly CodeNode right;

        public ShiftLeftNode(CodeNode left, CodeNode right)
        {
            this.left = left;
            this.right = right;
        }
        
        public override void Emit(LoggingILGenerator generator, SymbolTable symbolTable)
        {
            left.Emit(generator, symbolTable);
            right.Emit(generator, symbolTable);
            generator.Emit(OpCodes.Shl);
        }
    }
}