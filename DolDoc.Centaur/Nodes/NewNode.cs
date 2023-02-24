using System;
using System.Reflection.Emit;
using DolDoc.Centaur.Symbols;

namespace DolDoc.Centaur.Nodes
{
    public class NewNode : ASTNode, IBytecodeEmitter
    {
        private readonly string type;

        public NewNode(string type)
        {
            this.type = type;
        }

        public void Emit(FunctionCompilerContext ctx)
        {
            var t = Type(ctx);
            ctx.Generator.Emit(OpCodes.Newobj, t.GetConstructor(Array.Empty<Type>()));
        }

        public Type Type(ICompilerContext ctx)
        {
            var t = ctx.SymbolTable.FindSymbol<TypeSymbol>(type);
            return t?.Type;
        }
    }
}