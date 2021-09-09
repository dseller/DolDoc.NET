using DolDoc.Editor;
using DolDoc.Editor.Core;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace DolDoc.OpenGLHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Verbose()
                .CreateLogger();

            // var compositor = new Compositor<OpenGLNativeWindow>();

            //var window = compositor.NewWindow();
            //var obj = new TestForm();
            //window.Show("Form Test", 1024, 768, new FormDocument<TestForm>(obj, 128, 60));

            //new Thread(() =>
            //{
            //    while (true)
            //    {
            //        if (obj.TimeFormat == TimeFormat.AmPm)
            //            obj.TheTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt");
            //        else
            //            obj.TheTime = DateTime.Now.ToString();

            //        Thread.Sleep(1000);
            //    }
            //}).Start();
        }
    }
}
