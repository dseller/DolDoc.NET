namespace DolDoc.Centaur.Nodes
{
    public class ParameterNode : ASTNode
    {
        public string Name { get; }
        public string Type { get; }

        public ParameterNode(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}