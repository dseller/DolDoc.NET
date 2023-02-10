using System;
using DolDoc.Editor.Core;

namespace DolDoc.Editor.Forms
{
    public class ConfirmationModalModel
    {
        [ButtonField("Yes", nameof(YesHandler), "\n\n\n          $FG,RED$", suffix: "$FG$")]
        public string Yes { get; set; }

        [ButtonField("No", nameof(NoHandler), "      ")]
        public string No { get; set; }

        public void YesHandler(FormDocument<ConfirmationModalModel> document)
        {
            ((ConfirmationModal) document).OnConfirm?.Invoke();
            CloseWindow();
        }

        public void NoHandler(FormDocument<ConfirmationModalModel> document) => CloseWindow();

        private void CloseWindow() => Compositor.Compositor.Instance.CloseWindow(Compositor.Compositor.Instance.FocusedWindow);
    }

    public class ConfirmationModal : FormDocument<ConfirmationModalModel>
    {
        private readonly string message;

        internal Action OnConfirm { get; }

        public ConfirmationModal(string message, Action onConfirm)
        {
            this.message = message;
            this.OnConfirm = onConfirm;
            Load(Generate());
        }

        protected override string GetHeader(Type formType) => message;
    }
}