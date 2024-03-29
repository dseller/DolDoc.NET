﻿using DolDoc.Editor.Core;

namespace DolDoc.Editor.Logging
{
    public class DocumentLogger : ILogger
    {
        private readonly Document document;

        public DocumentLogger(Document document)
        {
            this.document = document;
        }
        
        public void Debug(string message)
        {
            document.Write($"[$FG,LTGRAY$DEBUG$FG$] $FG,LTGRAY${message}$FG$\n");
        }

        public void Info(string message)
        {
            document.Write($"[$FG,GREEN$ INFO$FG$] $FG,GREEN${message}$FG$\n");
        }

        public void Warning(string message)
        {
            document.Write($"[$FG,YELLOW$ WARN$FG$] $FG,YELLOW${message}$FG$\n");
        }

        public void Error(string message)
        {
            document.Write($"[$FG,RED$ERROR$FG$] $FG,RED${message}$FG$\n");
        }
    }
}