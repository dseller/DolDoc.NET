using System.Collections.Generic;

namespace DolDoc.Centaur.Nodes
{
    public class DefinitionListNode : ASTNode
    {
        public DefinitionListNode(List<ASTNode> definitions)
        {
            Definitions = definitions;
        }
        
        public List<ASTNode> Definitions { get; }

    }
}