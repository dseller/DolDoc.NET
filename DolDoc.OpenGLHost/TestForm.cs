using DolDoc.Editor.Forms;

namespace DolDoc.OpenGLHost
{
    public enum Gender
    {
        Male,
        Female
    }

    [FormHeader("This is an $FG,RED$example$FG$ form. Enter the data below.\n\n")]
    public class TestForm
    {
        [DataField("File path")]
        public string FileName { get; set; }

        [CheckboxField("Open read-only?")]
        public bool ReadOnly { get; set; }

        [DataField("Enter your gender")]
        public Gender Gender { get; set; }
    }
}
