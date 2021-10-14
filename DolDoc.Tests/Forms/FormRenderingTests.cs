using DolDoc.Editor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using DolDoc.Editor.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DolDoc.Tests.Forms
{
    [TestClass]
    [FormHeader("HEADER")]
    [FormFooter("FOOTER")]
    public class FormHeaderFooterTests
    {
        [TestMethod]
        public void RendersHeaderAndFooter()
        {
            var doc = new FormDocument<FormHeaderFooterTests>();
            var contents = doc.ToPlainText();

            Assert.IsTrue(contents.StartsWith("HEADER"));
            Assert.IsTrue(contents.EndsWith("FOOTER"));
        }
    }

    [TestClass]
    public class FormButtonTests
    {
        private bool buttonClicked;

        [ButtonField("Press me!", nameof(HandleButton))]
        public string TestButton { get; set; }

        public void HandleButton(FormDocument<FormButtonTests> form)
        {
            buttonClicked = true;
        }

        [TestMethod]
        public void RendersButton()
        {
            var doc = new FormDocument<FormButtonTests>();
            var contents = doc.ToPlainText();

            Assert.AreEqual("\n $BT,\"Press me!\",H=\"HandleButton\"$\n", contents);
        }

        [TestMethod]
        public void HandlesButtonClick()
        {
            var doc = new FormDocument<FormButtonTests>(this);
            doc.ButtonClicked(doc.Entries.OfType<Button>().Single());

            Assert.IsTrue(buttonClicked);
        }
    }

    [TestClass]
    public class CheckboxTests
    {
        [CheckboxField("This is my checkbox")]
        public bool MyCheckbox { get; set; }

        [TestMethod]
        public void RendersCheckbox()
        {
            var doc = new FormDocument<CheckboxTests>();
            var contents = doc.ToPlainText();

            Assert.AreEqual("$CB,\"This is my checkbox\",PROP=\"MyCheckbox\"$\n", contents);
        }

        [TestMethod]
        public void ChecksValue()
        {
            var doc = new FormDocument<CheckboxTests>(this);
            var entry = doc.Entries.OfType<CheckBox>().Single();
            entry.KeyPress(new ViewerState(null, doc, 100, 100), Key.SPACE, ' ', 0);

            Assert.IsTrue(MyCheckbox);
        }

        [TestMethod]
        public void UnchecksValue()
        {
            MyCheckbox = true;
            var doc = new FormDocument<CheckboxTests>(this);
            var entry = doc.Entries.OfType<CheckBox>().Single();
            entry.KeyPress(new ViewerState(null, doc, 100, 100), Key.SPACE, ' ', 0);

            Assert.IsFalse(MyCheckbox);
        }
    }

    [TestClass]
    public class EnumListFieldTests
    {
        public enum Values
        {
            ValueA,
            ValueB
        }

        [ListField("My List Field", typeof(Values))]
        public Values MyEnumValue { get; set; }

        [TestMethod]
        public void RendersListField()
        {
            var doc = new FormDocument<EnumListFieldTests>();
            var contents = doc.ToPlainText();

            Assert.AreEqual($"$LS,A=\"My List Field\",TYPE=\"Enum\",SRC=\"{typeof(Values).AssemblyQualifiedName}\",PROP=\"MyEnumValue\"$\n", contents);
        }
    }
}
