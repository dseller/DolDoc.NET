using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DolDoc.Editor.Fonts
{
    /// <summary>
    /// Font provider that can read fonts from YAFF files. See https://github.com/robhagemans/hoard-of-bitfonts 
    /// for more information. The Courier_8 font comes from there.
    /// </summary>
    public class YaffFontProvider : IFontProvider
    {
        private readonly bool mirror;
        private readonly string _fontsFolder;
        private readonly bool useDefault;

        public YaffFontProvider(bool mirror = true, string fontsFolder = "Fonts", bool useDefault = true)
        {
            this.mirror = mirror;
            _fontsFolder = fontsFolder;
            this.useDefault = useDefault;
        }

        public IFont Get(string name)
        {
            Stream stream;
            if (useDefault)
                stream = typeof(YaffFontProvider).Assembly.GetManifestResourceStream("DolDoc.Editor.Fonts.Terminal_VGA_cp861.yaff");
            else
                stream = File.Open($"{_fontsFolder}/{name}.yaff", FileMode.Open);
            
            // TODO: this is the ugliest stuff ever, but meh it works.
            using (stream)
            {
                using (var reader = new StreamReader(stream))
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

                        foreach (var key in keys)
                            values.Add(key, value);
                    }

                    Debug.WriteLine($"YAFF Font {name} loaded with {values.Count} glyphs");

                    // TODO: font width/height is still hardcoded here, only supports
                    // Courier_8 now.
                    return new YaffFont(values, 8, 12, mirror);
                }
            }

        }
    }
}

