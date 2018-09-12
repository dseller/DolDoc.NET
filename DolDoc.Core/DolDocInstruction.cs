namespace DolDoc.Core.Editor
{
    public class DolDocInstruction<T>
    {
        public CharacterFlags Flags { get; set; }
                
        public T Data { get; }

        public DolDocInstruction(CharacterFlags flags, T data)
        {
            Flags = flags;
            Data = data;
        }
    }
}
