namespace DolDoc.Centaur.Nodes
{
    public class IncludeNode : ASTNode
    {
        public IncludeNode(string path)
        {
            Path = path;
        }

        public string Path { get; }    
    }
}