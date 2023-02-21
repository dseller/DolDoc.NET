namespace DolDoc.Centaur.Nodes
{
    public abstract class DataNode : ASTNode
    {
        public abstract void EmitRead(LoggingILGenerator generator);

        public abstract void EmitWrite(LoggingILGenerator generator);
    }
}
