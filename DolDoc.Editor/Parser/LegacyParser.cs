using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DolDoc.Editor.Core;

namespace DolDoc.Core.Parser
{
    public class LegacyParser : IDolDocParser
    {
        public IEnumerable<Command> Parse(string content)
        {
            var result = new List<Command>();

            for (int i = 0; i < content.Length; i++)
            {
                char ch = content[i];
                if (ch == '$')
                {
                    var flags = new List<Flag>();
                    var arguments = new List<Argument>();

                    i++;
                    if (content[i] == '$')
                    {
                        // OnWriteCharacter?.Invoke(content[i]);
                        result.Add(new Command(i, "TX", flags, new[] { new Argument(null, new string(content[i], 1)) }));
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

                        string f = flag.ToString();
                        flags.Add(new Flag(f[0] == '+', f.Substring(1)));
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

                            // TODO: this implementation does not support key/value arguments :/
                            // thats why its legacy
                            arguments.Add(new Argument(null, builder.ToString()));
                        }
                    }

                    // OnCommand?.Invoke(new Command(cmd, flags, arguments));
                    result.Add(new Command(i, cmd, flags, arguments));

                    //var f = CreateFlags(flags);
                    //switch (cmd)
                    //{
                    //    case "BG":
                    //        if (arguments.Count == 0)
                    //            OnBackgroundColor(null);
                    //        else
                    //            OnBackgroundColor((EgaColor)Enum.Parse(typeof(EgaColor), arguments[0], true));
                    //        break;

                    //    case "CL":
                    //        OnClear();
                    //        break;

                    //    case "FG":
                    //        if (arguments.Count == 0)
                    //            OnForegroundColor(null);
                    //        else
                    //            OnForegroundColor((EgaColor)Enum.Parse(typeof(EgaColor), arguments[0], true));
                    //        break;

                    //    case "LK":
                    //        OnWriteLink(new DolDocInstruction<string>(f, arguments[0]));
                    //        break;

                    //    case "SY":
                    //        // Shift cursor amount of pixels on Y axis.
                    //        _yOffset = int.Parse(arguments[0]);
                    //        break;

                    //    case "TX":
                    //        OnWriteString(new DolDocInstruction<string>(f, arguments[0]));
                    //        break;

                    //    case "UL":
                    //        _underLine = arguments[0] == "1";
                    //        break;

                    //    case "WW":
                    //        _wordWrap = arguments[0] == "1";
                    //        break;
                    //}
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    while (i < content.Length && content[i] != '$')
                        builder.Append(content[i++]);

                    if (i < content.Length && content[i] == '$')
                        i--;

                    // OnWriteString?.Invoke(builder.ToString());
                    result.Add(Command.CreateTextCommand(i, new Flag[0], builder.ToString()));
                }
            }

            return result;
        }
    }
}
