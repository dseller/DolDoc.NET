using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ScopeNode : ASTNode, IBytecodeEmitter
    {
        public List<IBytecodeEmitter> Nodes { get; }

        public ScopeNode(List<IBytecodeEmitter> nodes)
        {
            Nodes = nodes;
        }

        public void Emit(FunctionCompilerContext ctx)
        {
            ctx.SymbolTable.BeginScope();
            foreach (var node in Nodes)
                node.Emit(ctx);
            ctx.SymbolTable.EndScope();
        }
        
        public Type Type(ICompilerContext ctx) => null;
    }
}