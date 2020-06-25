namespace DolDoc.Core.Editor
{
    public class DolDocInstruction<T>
    {
        public CharacterFlags Flags { get; set; }
                
        public T Data { get; }

        public int TextOffset { get; }

        public DolDocInstruction(CharacterFlags flags, T data, int textOffset)
        {
            Flags = flags;
            Data = data;
            TextOffset = textOffset;
        }
    }
}
