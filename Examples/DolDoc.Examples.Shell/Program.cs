using DolDoc.Editor;
using DolDoc.Editor.Core;
using DolDoc.Renderer.OpenGL;
using NLua;
using NLua.Exceptions;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DolDoc.Examples.Shell
{
    public class Program
    {
        private string pwd = Environment.CurrentDirectory;
        private Document document;
        private Lua lua;

        public void Print(object str)
        {
            if (str != null)
                document.Write(str.ToString());
        }

        public void Eval(string str)
        {
            try
            {
                lua.DoString(str);
            }
            catch (LuaScriptException ex)
            {
                document.Write($"$BG,RED$$FG,WHITE${ex.Message}$FG$$BG$\n");
            }
        }

        public void Run()
        {
            var compositor = new Compositor<OpenGLNativeWindow>();
            var window = compositor.NewWindow();
            document = new Document();
            lua = new Lua();
            lua.LoadCLRPackage();
            

            void OnMacro(DocumentEntry entry)
            {
                var command = entry.GetArgument("LE");
                switch (command)
                {
                    //case "ChangeDir":
                    //    pwd = entry.GetArgument("RE");
                    //    document.Write(DirectoryListing(pwd));
                    //    document.Write(RenderPrompt());
                    //    //document = new Document(DirectoryListing(entry.GetArgument("RE")), null);
                    //    //document.OnMacro += OnMacro;
                    //    //window.State.LoadDocument(document);
                    //    break;
                }
            }

            document.OnMacro += entry =>
            {
                try
                {
                    lua.GetFunction("OnMacro")?.Call(entry);
                }
                catch (Exception ex)
                {
                    document.Write($"$BG,RED$$FG,WHITE${ex.ToString()}$FG$$BG$\n");
                }
            };
            document.OnPromptEntered += str =>
            {
                try
                {
                    lua.GetFunction("OnPrompt")?.Call(str);
                }
                catch (Exception ex)
                {
                    document.Write($"$BG,RED$$FG,WHITE${ex.ToString()}$FG$$BG$\n");
                }

                    // document.Write($"\nEntered: $FG,RED${str}$FG$\n\n> $PT$\n");
                    //var output = new StringBuilder();

                    //if (str == "dir")
                    //{
                    //    document.Write(DirectoryListing(pwd));
                    //    document.Write(RenderPrompt());
                    //    return;
                    //}

                    //var p = new Process();
                    //p.StartInfo.FileName = "cmd.exe";
                    //p.StartInfo.Arguments = @"/c " + str;
                    //p.StartInfo.WorkingDirectory = pwd;
                    //p.StartInfo.CreateNoWindow = true;
                    //p.StartInfo.RedirectStandardError = true;
                    //p.StartInfo.RedirectStandardOutput = true;
                    //p.StartInfo.RedirectStandardInput = false;
                    //p.OutputDataReceived += (a, b) => output.Append(b.Data?.Replace("$", "$$") + "\n");
                    //p.ErrorDataReceived += (a, b) => output.Append($"$FG,RED${b.Data?.Replace("$", "$$")}$FG$\n");
                    //p.Start();
                    //p.BeginErrorReadLine();
                    //p.BeginOutputReadLine();
                    //p.WaitForExit();

                    //document.Write("\n\n" + output.ToString());
                    //document.Write($"\n{RenderPrompt()}");
                };


            lua.RegisterFunction("print", this, typeof(Program).GetMethod(nameof(Print)));
            lua.RegisterFunction("eval", this, typeof(Program).GetMethod(nameof(Eval)));
            if (File.Exists("scripts\\main.lua"))
                lua.DoFile("scripts\\main.lua");

            // document.Load("");
            window.Show("DolDoc.NET File Browser", 1600, 1200, document);
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }

        //private static string RenderPrompt()
        //{
        //    var builder = new StringBuilder();
        //    var segments = pwd.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
        //    var accumulator = string.Empty;

        //    foreach (var segment in segments)
        //    {
        //        accumulator += $"{segment}{Path.AltDirectorySeparatorChar}";
        //        builder.Append($"$MA,\"{segment}\",LE=\"ChangeDir\",RE=\"{accumulator}\"${Path.AltDirectorySeparatorChar}");
        //    }

        //    builder.Append("> $PT$");
        //    return "\n\n" + builder.ToString();//.Replace("\\", "\\\\");
        //}

        //private static string DirectoryListing(string path = ".")
        //{
        //    var builder = new StringBuilder();
        //    var d = new DirectoryInfo(path);
        //    builder.AppendFormat("\n\n$FG,BLUE$Directory of {0}\n", d.FullName);

        //    builder.AppendFormat("DATE       TIME  SIZE\n");
        //    builder.AppendFormat("0000/00/00 00:00 0000 $MA,\".\",LE=\"ChangeDir\",RE=\"{0}\"$\n", d.FullName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        //    if (d.Parent != null)
        //        builder.AppendFormat("0000/00/00 00:00 0000 $MA,\"..\",LE=\"ChangeDir\",RE=\"{0}\"$\n", d.Parent.FullName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

        //    foreach (var directory in d.EnumerateDirectories())
        //        builder.AppendFormat("{0} {1} {2:X4} $MA,\"{3}\",LE=\"ChangeDir\",RE=\"{4}\"$\n", directory.LastWriteTime.ToString("yyyy/MM/dd"), directory.LastWriteTime.ToString("HH:mm"), 0, directory.Name, directory.FullName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        //    foreach (var file in d.EnumerateFiles())
        //        builder.AppendFormat("{0} {1} {2:X4} $LK,\"{4}\",A=\"{3}\"$\n", file.LastWriteTime.ToString("yyyy/MM/dd"), file.LastWriteTime.ToString("HH:mm"), file.Length / 1024, file.FullName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), file.Name);

        //    return builder.ToString();
        //}
    }
}
