using System.Collections.Generic;

namespace DolDoc.Centaur.Nodes
{
    public class DefinitionListNode : ASTNode
    {
        public DefinitionListNode(List<DefinitionNode> definitions)
        {
            Definitions = definitions;
        }
        
        public List<DefinitionNode> Definitions { get; }

    }
}