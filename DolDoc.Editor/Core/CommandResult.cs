namespace DolDoc.Editor.Core
{
    public class CommandResult
    {
        public bool Success { get; }

        public int WrittenCharacters { get; }

        public bool RefreshRequested { get; }

        public CommandResult(bool success, int writtenCharacters = 0, bool refreshRequested = false)
        {
            Success = success;
            RefreshRequested = refreshRequested;
            WrittenCharacters = writtenCharacters;
        }
    }
}
