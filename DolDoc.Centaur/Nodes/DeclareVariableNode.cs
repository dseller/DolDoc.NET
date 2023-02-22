using System;
using System.Reflection;
using DolDoc.Centaur.Symbols;

namespace DolDoc.Centaur.Nodes
{
    public class DeclareVariableNode : ASTNode, IBytecodeEmitter
    {
        private readonly string name;
        private readonly string type;
        private readonly IBytecodeEmitter assignment;

        public DeclareVariableNode(string name, string type, IBytecodeEmitter assignment = null)
        {
            this.name = name;
            this.type = type;
            this.assignment = assignment;
        }

        public void DefineStaticVariable(CompilerContext ctx)
        {
            var type = ctx.SymbolTable.FindSymbol<TypeSymbol>(this.type)?.Type;
            var field = ctx.CodeBuilder.DefineField(name, type, FieldAttributes.Public | FieldAttributes.Static);
            ctx.Log.Debug(".decl static {name}", name);
            ctx.SymbolTable.NewSymbol(new GlobalVariableSymbol(name, type, field));
        }
        
        public void Emit(FunctionCompilerContext ctx)
        {
            var type = ctx.SymbolTable.FindSymbol<TypeSymbol>(this.type)?.Type;

            var local = ctx.Generator.DeclareLocal(type);
            var symbol = ctx.SymbolTable.NewSymbol(new VariableSymbol(name, type, local.LocalIndex));

            if (assignment != null)
            {
                assignment.Emit(ctx);
                symbol.EmitSet(ctx);
            }
        }

        public Type Type => null;
    }
}