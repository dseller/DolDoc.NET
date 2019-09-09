namespace DolDoc.Core.Parser
{
    public class StringNode : DocumentNode
    {
        public StringNode(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string GetInfo() => Value;

        public override string ToString() => Value;
    }
}
