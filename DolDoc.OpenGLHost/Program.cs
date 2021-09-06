using DolDoc.Editor;
using DolDoc.Editor.Core;
using Serilog;
using System.IO;

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
            //window.Show("Demo", 1024, 768, DocumentLoader.Load(File.Open("Main.DD", FileMode.Open), 128, 60));

            //var window2 = compositor.NewWindow();
            //window2.Show("Another", 1024, 768, DocumentLoader.Load(File.Open("Examples/Hash.DD", FileMode.Open), 128, 60));

            var window = compositor.NewWindow();
            window.Show("Form Test", 1024, 768, new FormDocument<TestForm>(null, 128, 60));
        }
    }
}
