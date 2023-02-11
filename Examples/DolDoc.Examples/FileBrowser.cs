﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using DolDoc.Editor.Compositor;
using DolDoc.Editor.Core;

namespace DolDoc.Examples
{
    public static class FileBrowser
    {
        private static string DirectoryListing(string path = ".")
        {
            var builder = new StringBuilder();
            var d = new DirectoryInfo(path);
            builder.AppendFormat("\n$FG,CYAN$$TX+CX+B,\"DolDoc.NET File Browser\"$\n\n");
            builder.AppendFormat("$TI,\"{1}\"$$FG,RED$$BG,YELLOW$$UL,1$$TX+CX,\"Released under the MIT License, Copyright Dennis Seller 2018-{0}\"$$UL,0$$BG$\n\n", DateTime.Now.Year, d.FullName);

            builder.AppendFormat("$FG,BLUE$Directory of {0}\n", d.FullName);

            builder.AppendFormat("DATE       TIME  SIZE\n");
            builder.AppendFormat("0000/00/00 00:00 0000 $MA,\".\",LE=\"ChangeDir\",RE=\"{0}\"$\n", d.FullName);
            if (d.Parent != null)
                builder.AppendFormat("0000/00/00 00:00 0000 $MA,\"..\",LE=\"ChangeDir\",RE=\"{0}\"$\n", d.Parent.FullName.TrimEnd('\\'));
            
            foreach (var directory in d.EnumerateDirectories())
                builder.AppendFormat("{0} {1} {2:X4} $MA,\"{3}\",LE=\"ChangeDir\",RE=\"{4}\"$\n", directory.LastWriteTime.ToString("yyyy/MM/dd"), directory.LastWriteTime.ToString("HH:mm"), 0, directory.Name, directory.FullName);
            foreach (var file in d.EnumerateFiles())
                builder.AppendFormat("{0} {1} {2:X4} $LK,\"{4}\",A=\"{3}\"$\n", file.LastWriteTime.ToString("yyyy/MM/dd"), file.LastWriteTime.ToString("HH:mm"), file.Length / 1024, file.FullName, file.Name);

            return builder.ToString();
        }
        
        public static Document GetFileBrowserDocument()
        {
            var document = new Document();

            void OnMacro(DocumentEntry entry)
            {
                var command = entry.GetArgument("LE");
                Debug.WriteLine($"Executing macro {command}");

                switch (entry.GetArgument("LE"))
                {
                    case "ChangeDir":
                        document = new Document(DirectoryListing(entry.GetArgument("RE")));
                        document.OnMacro += OnMacro;
                        Compositor.Instance.Root.State.LoadDocument(document);
                        break;
                }
            }
            
            document.OnMacro += OnMacro;
            document.Load(DirectoryListing());
            return document;
        }
    }
}