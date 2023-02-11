using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using DolDoc.Examples.Shell.Helpers;
using DolDoc.Renderer.OpenGL;
using NLua;
using NLua.Exceptions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using DolDoc.Editor.Compositor;

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
                document.Write(new Error(ex.ToString()));
            }
        }

        public void Run()
        {
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
                    document.Write(new Error(ex.ToString()));
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
                    document.Write(new Error(ex.ToString()));
                }
            };
            document.OnSave += contents =>
            {
                try
                {
                    var prelude = $"\n\n$FG,RED$ --- SESSION STORED ON {DateTime.Now.ToString(CultureInfo.InvariantCulture)} --- $FG$\n\n";
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    using (var fs = File.Open(Path.Combine(path, "ddsh.buffer.dd"), FileMode.Create))
                    {
                        using (var writer = new StreamWriter(fs))
                            writer.Write(contents + prelude);
                    }
                }
                catch (Exception ex)
                {
                    document.Write(new Error(ex.ToString()));
                }
            };


            lua.RegisterFunction("print", this, typeof(Program).GetMethod(nameof(Print)));
            lua.RegisterFunction("eval", this, typeof(Program).GetMethod(nameof(Eval)));
            lua.RegisterFunction("reload", this, typeof(Program).GetMethod(nameof(Load)));
            lua.RegisterFunction("directory_listing", typeof(DirectoryListing).GetMethod("List"));
            lua.RegisterFunction("shell", this, typeof(Program).GetMethod(nameof(Shell)));
            lua["WORKING_DIRECTORY"] = Directory.GetCurrentDirectory();
            
            var bufferPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ddsh.buffer.dd");
            if (File.Exists(bufferPath))
            {
                using (var fs = File.Open(bufferPath, FileMode.Open))
                {
                    using (var reader = new StreamReader(fs))
                    {
                        var contents = reader.ReadToEnd();
                        document.Write(contents);
                    }
                }
            }
            
            Load();
            
            var window = new OpenTKWindow();
            var compositor = Compositor.Initialize(window, 1600, 1200, document);
            compositor.Start();
        }

        public void Shell(string cmd, Action<string> outputWriter)
        {
            if (string.IsNullOrWhiteSpace(cmd))
                return;
            
            var tokens = cmd.Split(new[] { ' ' }, 2);
            var psi = new ProcessStartInfo
            {
                FileName = tokens[0],
                Arguments = tokens.Length > 1 ? tokens[1] : string.Empty,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            var process = new Process
            {
                StartInfo = psi
            };

            try
            {
                process.Start();
                new Thread(() =>
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();
                        outputWriter?.Invoke(line + "\n");
                        Debug.WriteLine("Wrote a line!");
                    }
                }).Start();
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode != 2)
                    throw;
                
                document.Write(new Error("Command not found"));
            }
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
    }
}
