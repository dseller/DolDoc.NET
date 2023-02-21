namespace DolDoc.Centaur.Nodes
{
    public class AssignNode : ASTNode
    {
        private readonly ASTNode source, target;

        public AssignNode(ASTNode source, ASTNode target)
        {
            this.source = source;
            this.target = target;
        }
    }
}