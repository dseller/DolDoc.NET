using System.Collections.Generic;

namespace DolDoc.Centaur.Nodes
{
    public class StatementListNode : ASTNode
    {
        public StatementListNode(List<ASTNode> statements)
        {
            Statements = statements;
        }
        
        public List<ASTNode> Statements { get; }
    }
}