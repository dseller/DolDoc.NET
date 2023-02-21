using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class SymbolNode : CodeNode, IWrite
    {
        private readonly string identifier;

        public SymbolNode(string identifier)
        {
            this.identifier = identifier;
        }
        
        public override void Emit(LoggingILGenerator generator, SymbolTable symbolTable)
        {
            var sym = symbolTable.FindSymbol(identifier);
            if (sym.Target == SymbolTarget.Variable)
                generator.Emit(OpCodes.Ldloc, sym.Index);
            else
                throw new Exception();
        }

        public void EmitWrite(LoggingILGenerator generator, SymbolTable symbolTable)
        {
            var sym = symbolTable.FindSymbol(identifier);
            if (sym.Target == SymbolTarget.Variable)
                generator.Emit(OpCodes.Stloc, sym.Index);
            else
                throw new Exception();
        }
    }
}