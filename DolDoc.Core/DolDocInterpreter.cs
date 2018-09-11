using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DolDoc.Core.Editor
{
    public class DolDocInterpreter
    {
        private Stream _stream;
        private StreamReader _reader;

        public event Action OnClear;
        public event Action<DolDocInstruction<string>> OnWriteLink;
        public event Action<DolDocInstruction<string>> OnWriteString;
        public event Action<bool> OnUnderline;
        public event Action<char> OnWriteCharacter;
        public event Action<EgaColor?> OnForegroundColor;
        public event Action<EgaColor?> OnBackgroundColor;

        public DolDocInterpreter(Stream stream)
        {
            _stream = stream;
            _reader = new StreamReader(_stream);
        }

        public void Run()
        {
            string content = _reader.ReadToEnd();

            for (int i = 0; i < content.Length; i++)
            {
                char ch = content[i];
                if (ch == '$')
                {
                    var flags = new List<string>();
                    var arguments = new List<string>();

                    i++;
                    if (content[i] == '$')
                    {
                        OnWriteCharacter(content[i]);
                        continue;
                    }

                    string cmd = content.Substring(i, 2);
                    i += 2;

                    while (content[i] == '+' || content[i] == '-')
                    {
                        StringBuilder flag = new StringBuilder();
                        flag.Append(content[i++]);
                        while (content[i] != '+' && 
                            content[i] != '-' &&
                            content[i] != '$' &&
                            content[i] != ',' &&
                            i <= content.Length - 1)
                        {
                            flag.Append(content[i++]);
                        }
                        flags.Add(flag.ToString());
                    }

                    while (content[i] != '$')
                    {
                        if (content[i] == ',')
                        {
                            StringBuilder builder = new StringBuilder();

                            i++; // comma
                            if (content[i] == '"')
                            {
                                i++; // beginning quote
                                while (content[i] != '"')
                                {
                                    if (content[i] == '\\')
                                        i++;
                                    builder.Append(content[i++]);
                                }
                                i++; // ending quote
                            }
                            else
                            {
                                while (content[i] != ',' && content[i] != '$')
                                    builder.Append(content[i++]);
                            }

                            arguments.Add(builder.ToString());
                        }
                    }

                    switch (cmd)
                    {
                        case "BG":
                            if (arguments.Count == 0)
                                OnBackgroundColor(null);
                            else
                                OnBackgroundColor((EgaColor)Enum.Parse(typeof(EgaColor), arguments[0], true));
                            break;

                        case "CL":
                            OnClear();
                            break;

                        case "FG":
                            if (arguments.Count == 0)
                                OnForegroundColor(null);
                            else
                                OnForegroundColor((EgaColor)Enum.Parse(typeof(EgaColor), arguments[0], true));
                            break;

                        case "LK":
                            OnWriteLink(new DolDocInstruction<string>(null, arguments[0]));
                            break;

                        case "TX":
                            OnWriteString(new DolDocInstruction<string>(flags, arguments[0]));
                            break;

                        case "UL":
                            OnUnderline(arguments[0] == "1");
                            break;
                    }
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    while (content[i] != '$' && i < content.Length - 1)
                        builder.Append(content[i++]);

                    if (content[i] == '$')
                        i--;

                    OnWriteString(new DolDocInstruction<string>(new string[0], builder.ToString()));
                }
            }
        }
    }
}
