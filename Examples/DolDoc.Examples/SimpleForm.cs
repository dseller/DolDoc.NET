using System;
using DolDoc.Editor.Compositor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Forms;

namespace DolDoc.Examples
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
            Compositor.Instance.Logger.Info($"Submitting form:");
            Compositor.Instance.Logger.Info($"FileName: {FileName}");
            Compositor.Instance.Logger.Info($"Readonly: {ReadOnly}");
            Compositor.Instance.Logger.Info($"Gender: {Gender}");
        }
    }
}