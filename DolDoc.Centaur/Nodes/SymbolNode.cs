using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class SymbolNode : ASTNode, IWrite, IBytecodeEmitter
    {
        private readonly string identifier;

        public SymbolNode(string identifier)
        {
            this.identifier = identifier;
        }
        
        public void Emit(FunctionCompilerContext ctx)
        {
            var sym = ctx.SymbolTable.FindSymbol(identifier);
            sym.EmitGet(ctx);
        }

        public void EmitWrite(FunctionCompilerContext ctx)
        {
            var sym = ctx.SymbolTable.FindSymbol(identifier);
            sym.EmitSet(ctx);
        }

        public Type Type(ICompilerContext ctx)
        {
            var sym = ctx.SymbolTable.FindSymbol(identifier);
            return sym.Type;
        }
    }
}