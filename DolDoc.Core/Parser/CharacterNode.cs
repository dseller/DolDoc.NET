namespace DolDoc.Core.Parser
{
    public class CharacterNode : DocumentNode
    {
        public CharacterNode(char ch)
        {
            Char = ch;
        }

        public char Char { get; }

        public override string GetInfo() => $"Character '{Char}'";

        public override string ToString() => GetInfo();
    }
}
