﻿using DolDoc.Editor.Forms;

namespace DolDoc.OpenGLHost
{
    public enum Gender
    {
        Male,
        Female
    }

    [FormHeader("This is an $FG,RED$example$FG$ form. Enter the data below.\n\n")]
    [FormFooter("\n\n$BK,1$$FG,RED$$TX+B+CX,\"Please verify before submitting!\"$$BK,0$")]
    public class TestForm
    {
        [DataField("File path")]
        public string FileName { get; set; }

        [CheckboxField("Open read-only?")]
        public bool ReadOnly { get; set; }

        [ListField("Enter your gender", typeof(Gender))]
        public Gender Gender { get; set; }

        [ButtonField("Submit", nameof(OnSubmit), suffix: null)]
        public string Submit { get; set; }

        [ButtonField("Verify", null, prefix: "   ")]
        public string Verify { get; set; }

        public void OnSubmit()
        {
            
        }
    }
}
