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
            var window = compositor.NewWindow();
            window.Show("Demo", 1024, 768, DocumentLoader.Load(File.Open("Examples/GraphicsOverview.DD", FileMode.Open), 128, 73));

            //var window2 = compositor.NewWindow();
            //window2.Show("Another", 640, 480, DocumentLoader.Load(File.Open("Examples/Job.DD", FileMode.Open)));
        }
    }
}
