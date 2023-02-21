using DolDoc.Editor.Core;
using DolDoc.Shared;

namespace DolDoc.Editor.Logging
{
    public class DocumentLogger : ILogger
    {
        private readonly Document document;
        private int indent;

        public DocumentLogger(Document document)
        {
            this.document = document;
            indent = 0;
        }
        
        public void Debug(string message, params object[] parameters)
        {
            document.Write($"[$FG,LTGRAY$DEBUG$FG$] $FG,LTGRAY${message}$FG$\n");
        }

        public void Info(string message, params object[] parameters)
        {
            document.Write($"[$FG,GREEN$ INFO$FG$] $FG,GREEN${message}$FG$\n");
        }

        public void Warning(string message, params object[] parameters)
        {
            document.Write($"[$FG,YELLOW$ WARN$FG$] $FG,YELLOW${message}$FG$\n");
        }

        public void Error(string message, params object[] parameters)
        {
            document.Write($"[$FG,RED$ERROR$FG$] $FG,RED${message}$FG$\n");
        }

        public void Enter(string text)
        {
            document.Write($"$TR,\"{text}\"$\n$ID,2$");
        }

        public void Leave()
        {
            document.Write("$ID,-2$");
        }
    }
}