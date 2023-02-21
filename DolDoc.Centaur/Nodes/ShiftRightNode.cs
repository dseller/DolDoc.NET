using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ShiftRightNode : CodeNode
    {
        private readonly CodeNode left;
        private readonly CodeNode right;

        public ShiftRightNode(CodeNode left, CodeNode right)
        {
            this.left = left;
            this.right = right;
        }
        
        public override void Emit(LoggingILGenerator generator, SymbolTable symbolTable)
        {
            left.Emit(generator, symbolTable);
            right.Emit(generator, symbolTable);
            generator.Emit(OpCodes.Shr_Un);
        }
    }
}