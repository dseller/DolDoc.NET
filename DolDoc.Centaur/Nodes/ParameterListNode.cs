using System.Collections.Generic;

namespace DolDoc.Centaur.Nodes
{
    public class ParameterListNode : ASTNode
    {
        public ParameterListNode(List<ParameterNode> parameters)
        {
            Parameters = parameters;
        }
        
        public List<ParameterNode> Parameters { get; }
    }
}