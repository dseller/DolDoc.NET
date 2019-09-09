namespace DolDoc.Core.Parser
{
    public class Flag
    {
        public Flag(string code, bool status)
        {
            Code = code;
            Status = status;
        }

        public string Code { get; }

        public bool Status { get; }

        public override string ToString() => Status ? $"+{Code}" : $"-{Code}";
    }
}
