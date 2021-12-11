using System;
using System.Threading;
using DolDoc.Editor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Forms;
using DolDoc.Renderer.OpenGL;
using Serilog;

namespace DolDoc.Examples.SimpleForm
{
    public enum Gender
    {
        Male,
        Female
    }

    public enum TimeFormat
    {
        AmPm,
        TwentyFourHours
    }

    [FormHeader("$TI,\"Test Form\"$This is an $FG,RED$example$FG$ form. Enter the data below.\n\n")]
    [FormFooter("\n\n\n$BK,1$$FG,RED$$TX+B+CX,\"Please verify before submitting!\"$$BK,0$")]
    public class TestForm
    {
        private readonly Random _random;

        public TestForm()
        {
            _random = new Random(DateTime.UtcNow.Millisecond);
        }

        [DataField("File path")]
        public string FileName { get; set; }

        [CheckboxField("Open read-only?")]
        public bool ReadOnly { get; set; }

        [ListField("Enter your gender", typeof(Gender))]
        public Gender Gender { get; set; }

        [ListField("Time format", typeof(TimeFormat))]
        public TimeFormat TimeFormat { get; set; }

        [ButtonField("Submit", nameof(OnSubmit), prefix: "\n\n   ", suffix: null)]
        public string Submit { get; set; }

        [ButtonField("Random number", nameof(GetRandomNumber), prefix: "   ")]
        public string DoRandom { get; set; }

        [ValueField(prefix: "\n\n")]
        public string TheTime { get; set; }

        [ValueField(prefix: "\n\n")]
        public int Random { get; set; }

        public void GetRandomNumber(FormDocument<TestForm> form) => Random = _random.Next();

        public void OnSubmit(FormDocument<TestForm> form)
        {
            Log.Information("Submitting form:");
            Log.Information("FileName: {0}", FileName);
            Log.Information("Readonly: {0}", ReadOnly);
            Log.Information("Gender: {0}", Gender);
        }
    }
    
    public static class Program
    {
        public static void Main(string[] args)
        {
            var compositor = new Compositor<OpenGLNativeWindow>();
            var window = compositor.NewWindow();
            var obj = new TestForm();
            window.Show("Form Test", 1024, 768, new FormDocument<TestForm>(obj, 128, 60));

            new Thread(() =>
            {
                while (true)
                {
                    if (obj.TimeFormat == TimeFormat.AmPm)
                        obj.TheTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt");
                    else
                        obj.TheTime = DateTime.Now.ToString();

                    Thread.Sleep(1000);
                }
            }).Start();
        }
    }
}