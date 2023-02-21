using System.Collections.Generic;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class ScopeNode : CodeNode
    {
        public List<CodeNode> Nodes { get; }

        public ScopeNode(List<CodeNode> nodes)
        {
            Nodes = nodes;
        }

        public override void Emit(LoggingILGenerator generator, SymbolTable symbolTable)
        {
            symbolTable.BeginScope();
            foreach (var node in Nodes)
                node.Emit(generator, symbolTable);
            symbolTable.EndScope();
        }
    }
}