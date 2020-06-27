﻿using System.Collections.Generic;
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
                var offset = i;
                char ch = content[i];
                if (ch == '$')
                {
                    var flags = new List<Flag>();
                    var arguments = new List<Argument>();

                    i++;
                    if (content[i] == '$')
                    {
                        // OnWriteCharacter?.Invoke(content[i]);
                        result.Add(new Command(offset, "TX", flags, new[] { new Argument(null, new string(content[i], 1)) }));
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

                            string arg = builder.ToString();
                            string key = null, value;

                            if (arg.Contains("="))
                            {
                                var tokens = arg.Split('=');
                                key = tokens[0];
                                value = tokens[1];
                            }
                            else
                                value = arg;

                            arguments.Add(new Argument(key, value));
                        }
                    }

                    // OnCommand?.Invoke(new Command(cmd, flags, arguments));
                    result.Add(new Command(offset, cmd, flags, arguments));
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    while (i < content.Length && content[i] != '$')
                        builder.Append(content[i++]);

                    if (i < content.Length && content[i] == '$')
                        i--;

                    // OnWriteString?.Invoke(builder.ToString());
                    result.Add(Command.CreateTextCommand(offset, new Flag[0], builder.ToString()));
                }
            }

            return result;
        }
    }
}
