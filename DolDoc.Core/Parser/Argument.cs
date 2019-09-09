namespace DolDoc.Core.Parser
{
    public class Argument
    {
        public Argument(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }

        public override string ToString()
        {
            if (Key == null)
                return Value;
            return $"{Key}={Value}";
        }
    }
}
