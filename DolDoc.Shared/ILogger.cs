namespace DolDoc.Shared
{
    public interface ILogger
    {
        void Debug(string message, params object[] parameters);
        void Info(string message, params object[] parameters);
        void Warning(string message, params object[] parameters);
        void Error(string message, params object[] parameters);
        void Enter(string text);
        void Leave();
    }
}