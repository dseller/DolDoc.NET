using Serilog;
using System.Collections.Generic;
using System.IO;

namespace DolDoc.Editor.Fonts
{
    /// <summary>
    /// Font provider that can read fonts from YAFF files. See https://github.com/robhagemans/hoard-of-bitfonts 
    /// for more information. The Courier_8 font comes from there.
    /// </summary>
    public class YaffFontProvider : IFontProvider
    {
        private readonly string _fontsFolder;

        public YaffFontProvider(string fontsFolder = "Fonts")
        {
            _fontsFolder = fontsFolder;
        }

        public IFont Get(string name)
        {
            // TODO: this is the ugliest stuff ever, but meh it works.
            using (var fs = File.Open($"{_fontsFolder}/{name}.yaff", FileMode.Open))
            {
                using (var reader = new StreamReader(fs))
                {
                    var values = new Dictionary<string, string>();
                    while (!reader.EndOfStream)
                    {
                        string currentLine = string.Empty;
                        List<string> keys = new List<string>();
                        string value = string.Empty;

                        while (currentLine == string.Empty)
                            currentLine = reader.ReadLine();

                        if (currentLine.StartsWith("#"))
                            continue;

                        while (currentLine.Trim().EndsWith(":"))
                        {
                            keys.Add(currentLine.Replace(":", string.Empty).Trim());
                            currentLine = reader.ReadLine();
                        }

                        while (currentLine != string.Empty)
                        {
                            if (currentLine.StartsWith("#"))
                            {
                                currentLine = reader.ReadLine();
                                continue;
                            }
                            if (currentLine.Trim() == "-")
                            {
                                currentLine = reader.ReadLine();
                                break;
                            }
                            value += currentLine.Trim();
                            currentLine = reader.ReadLine();
                        }

                        // Log.Verbose("Adding glyph with keys: {0}, value: {1}", keys.ToArray(), value);
                        foreach (var key in keys)
                            values.Add(key, value);
                    }

                    Log.Information("YAFF Font {0} loaded with {1} glyphs", name, values.Count);

                    // TODO: font width/height is still hardcoded here, only supports
                    // Courier_8 now.
                    return new YaffFont(values, 8, 12);
                }
            }

        }
    }
}

