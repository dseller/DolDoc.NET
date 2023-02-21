using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class DeclareVariableNode : CodeNode
    {
        private readonly string name;
        private readonly Type type;
        private readonly CodeNode assignment;

        public DeclareVariableNode(string name, Type type, CodeNode assignment = null)
        {
            this.name = name;
            this.type = type;
            this.assignment = assignment;
        }
        
        public override void Emit(LoggingILGenerator generator, SymbolTable symbolTable)
        {
            var local = generator.DeclareLocal(type);
            symbolTable.NewSymbol(new SymbolTable.Symbol(name, type, local.LocalIndex, SymbolTarget.Variable));

            if (assignment != null)
            {
                assignment.Emit(generator, symbolTable);
                generator.Emit(OpCodes.Stloc, local.LocalIndex);
            }
        }
    }
}