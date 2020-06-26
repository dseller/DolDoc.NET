//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;

//namespace DolDoc.Core.Editor
//{
//    public class DolDocInterpreter
//    {
//        private bool _underLine, _wordWrap;
//        private int _yOffset, _xOffset;

//        public event Action OnClear;
//        public event Action<DolDocInstruction<string>> OnWriteLink;
//        public event Action<DolDocInstruction<string>> OnWriteString;
//        public event Action<char, int> OnWriteCharacter;
//        public event Action<EgaColor?> OnForegroundColor;
//        public event Action<EgaColor?> OnBackgroundColor;
//        public event Action<Exception> OnParseError;

//        public void Render(string content)
//        {
//            OnClear();

//            try
//            {
//                for (int i = 0; i < content.Length; i++)
//                {
//                    int offset = i;
//                    char ch = content[i];
//                    if (ch == '$')
//                    {
//                        var flags = new List<string>();
//                        var arguments = new List<string>();

//                        i++;
//                        if (content[i] == '$')
//                        {
//                            OnWriteCharacter(content[i], i);
//                            continue;
//                        }

//                        string cmd = content.Substring(i, 2);
//                        i += 2;

//                        while (content[i] == '+' || content[i] == '-')
//                        {
//                            StringBuilder flag = new StringBuilder();
//                            flag.Append(content[i++]);
//                            while (content[i] != '+' &&
//                                content[i] != '-' &&
//                                content[i] != '$' &&
//                                content[i] != ',' &&
//                                i <= content.Length - 1)
//                            {
//                                flag.Append(content[i++]);
//                            }
//                            flags.Add(flag.ToString());
//                        }

//                        while (content[i] != '$')
//                        {
//                            if (content[i] == ',')
//                            {
//                                StringBuilder builder = new StringBuilder();

//                                i++; // comma
//                                if (content[i] == '"')
//                                {
//                                    i++; // beginning quote
//                                    while (content[i] != '"')
//                                    {
//                                        if (content[i] == '\\')
//                                            i++;
//                                        builder.Append(content[i++]);
//                                    }
//                                    i++; // ending quote
//                                }
//                                else
//                                {
//                                    while (content[i] != ',' && content[i] != '$')
//                                        builder.Append(content[i++]);
//                                }

//                                arguments.Add(builder.ToString());
//                            }
//                        }

//                        var f = CreateFlags(flags);
//                        switch (cmd)
//                        {
//                            case "BG":
//                                if (arguments.Count == 0)
//                                    OnBackgroundColor(null);
//                                else
//                                    OnBackgroundColor((EgaColor)Enum.Parse(typeof(EgaColor), arguments[0], true));
//                                break;

//                            case "CL":
//                                OnClear();
//                                break;

//                            case "FG":
//                                if (arguments.Count == 0)
//                                    OnForegroundColor(null);
//                                else
//                                    OnForegroundColor((EgaColor)Enum.Parse(typeof(EgaColor), arguments[0], true));
//                                break;

//                            case "LK":
//                                OnWriteLink(new DolDocInstruction<string>(f, arguments[0], offset));
//                                break;

//                            case "SY":
//                                // Shift cursor amount of pixels on Y axis.
//                                _yOffset = int.Parse(arguments[0]);
//                                break;

//                            case "TX":
//                                OnWriteString(new DolDocInstruction<string>(f, arguments[0], offset));
//                                break;

//                            case "UL":
//                                _underLine = arguments[0] == "1";
//                                break;

//                            case "WW":
//                                _wordWrap = arguments[0] == "1";
//                                break;
//                        }
//                    }
//                    else
//                    {
//                        StringBuilder builder = new StringBuilder();
//                        while (content[i] != '$' && i < content.Length - 1)
//                            builder.Append(content[i++]);

//                        if (content[i] == '$')
//                            i--;

//                        OnWriteString(new DolDocInstruction<string>(CreateFlags(new string[0]), builder.ToString(), offset));
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                OnParseError(ex);
//            }
//        }

//        private CharacterFlags CreateFlags(IEnumerable<string> flags)
//        {
//            var result = CharacterFlags.None;

//            if (_underLine)
//                result |= CharacterFlags.Underline;
//            if (_wordWrap)
//                result |= CharacterFlags.WordWrap;

//            foreach (var flag in flags)
//            {
//                if (flag == "+H")
//                    result |= CharacterFlags.Hold;
//                else if (flag == "+CX")
//                    result |= CharacterFlags.Center;
//                else if (flag == "+RX")
//                    result |= CharacterFlags.Right;
//            }

//            return result;
//        }
//    }
//}
