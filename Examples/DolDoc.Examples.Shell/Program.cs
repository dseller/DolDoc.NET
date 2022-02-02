using DolDoc.Editor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using DolDoc.Examples.Shell.Helpers;
using DolDoc.Renderer.OpenGL;
using NLua;
using NLua.Exceptions;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DolDoc.Examples.Shell
{
    public class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

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
                document.Entries.AddLast(new Error(ex.ToString()));
            }
        }

        public void Run()
        {
            var compositor = new Compositor<OpenGLNativeWindow>();
            var window = compositor.NewWindow();
            document = new Document();
            lua = new Lua();
            lua.LoadCLRPackage();

            document.OnMacro += entry =>
            {
                try
                {
                    lua.GetFunction("OnMacro")?.Call(entry);
                }
                catch (Exception ex)
                {
                    document.Entries.AddLast(new Error(ex.ToString()));
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
                    document.Entries.AddLast(new Error(ex.ToString()));
                }
            };


            lua.RegisterFunction("print", this, typeof(Program).GetMethod(nameof(Print)));
            lua.RegisterFunction("eval", this, typeof(Program).GetMethod(nameof(Eval)));
            lua.RegisterFunction("reload", this, typeof(Program).GetMethod(nameof(Load)));
            lua.RegisterFunction("directory_listing", typeof(DirectoryListing).GetMethod("List"));
            lua["WORKING_DIRECTORY"] = Directory.GetCurrentDirectory();
            Load();

            window.Show("DolDoc.NET File Browser", 1600, 1200, document);
        }

        public void Load()
        {
            if (File.Exists("scripts\\main.lua"))
                lua.DoFile("scripts\\main.lua");
        }

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);

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
