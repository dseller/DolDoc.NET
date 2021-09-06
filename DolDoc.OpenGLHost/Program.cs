using DolDoc.Editor;
using DolDoc.Editor.Core;
using Serilog;
using System;
using System.IO;
using System.Text;

namespace DolDoc.OpenGLHost
{
    public class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Verbose()
                .CreateLogger();

            var compositor = new Compositor<OpenGLNativeWindow>();
            //var window = compositor.NewWindow();
            //window.Show("Demo", 1024, 768, DocumentLoader.Load(File.Open("Examples/GraphicsOverview.DD", FileMode.Open), 128, 73));

            //var window2 = compositor.NewWindow();
            //window2.Show("Another", 1024, 768, DocumentLoader.Load(File.Open("Examples/Hash.DD", FileMode.Open), 128, 60));

            //var window = compositor.NewWindow();
            //window.Show("Form Test", 1024, 768, new FormDocument<TestForm>(null, 128, 60));

            var window = compositor.NewWindow();
            var document = new Document(128, 63);

            var builder = new StringBuilder();
            var d = new DirectoryInfo("Examples");
            builder.AppendFormat("\n$FG,CYAN$$TX+CX+B,\"DolDoc.NET File Browser\"$\n\n");
            builder.AppendFormat("$FG,RED$$UL,1$$TX+CX,\"Released under the MIT License, Copyright Dennis Seller 2018-{0}\"$$UL,0$\n\n", DateTime.Now.Year);

            builder.AppendFormat("$FG,BLUE$Directory of {0}\n", d.FullName);
            
            builder.AppendFormat("DATE       TIME  SIZE\n");
            foreach (var directory in d.EnumerateDirectories())
                builder.AppendFormat("{0} {1} {2:X4} $MA,\"{3}\"$\n", directory.LastWriteTime.ToString("yyyy/MM/dd"), directory.LastWriteTime.ToString("HH:mm"), 0, directory.Name);
            foreach (var file in d.EnumerateFiles())
                builder.AppendFormat("{0} {1} {2:X4} $LK,\"{3}\",A=\"{4}\"$\n", file.LastWriteTime.ToString("yyyy/MM/dd"), file.LastWriteTime.ToString("HH:mm"), file.Length / 1024, file.FullName, file.Name);


            document.Load(builder.ToString());
            window.Show("DolDoc.NET File Browser", 1024, 768, document);
        }
    }
}
