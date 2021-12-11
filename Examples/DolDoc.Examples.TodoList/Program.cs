using DolDoc.Editor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Forms;
using DolDoc.Renderer.OpenGL;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Examples.TodoList
{
    public class TodoItem
    {
        public string Text { get; set; }

        public bool Done { get; set; }

        public TodoItem(bool done, string text)
        {
            Done = done;
            Text = text;
        }
    }

    [FormHeader("$UL,1$$TX+CX,\"Todo List\"$$UL,0$\n\n")]
    [FormHeaderFunction("GetTodoList")]
    [FormFooterFunction("GetFooter")]
    public class TodoForm
    {
        private readonly List<TodoItem> items;
        private string validationError;

        public TodoForm()
        {
            items = new List<TodoItem>();
            items.Add(new TodoItem(true, "Buy milk"));
            items.Add(new TodoItem(false, "Go shopping"));
        }

        [CheckboxField("Is done")]
        public bool IsDone { get; set; }

        [DataField("Task")]
        public string Item { get; set; }

        [ButtonField("Add item", nameof(Add), prefix: "\n\n  ")]
        public string AddButton { get; set; }

        public string GetTodoList(FormDocument<TodoForm> document)
        {
            var builder = new StringBuilder();

            builder.Append("Current list:\n");
            foreach (var item in items)
            {
                if (item.Done)
                    builder.Append($" * $FG,GREEN${item.Text}$FG$\n");
                else
                    builder.Append($" * $FG,RED${item.Text}$FG$\n");
            }

            builder.Append("\n\n$FG,CYAN$Create entry:$FG$\n");
            return builder.ToString();
        }

        public string GetFooter(FormDocument<TodoForm> document)
        {
            if (validationError == null)
                return string.Empty;

            return $"\n\n$BK,1$$FG,RED$$TX+CX+B,\"{validationError}\"$$FG$$BK,0$";
        }

        public void Add(FormDocument<TodoForm> document)
        {
            if (string.IsNullOrWhiteSpace(Item))
            {
                validationError = "PLEASE ENTER A VALUE!";
                document.Reload();
                return;
            }

            items.Add(new TodoItem(IsDone, Item));
            IsDone = false;
            Item = string.Empty;
            validationError = null;
            document.Reload();
        }
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            var compositor = new Compositor<OpenGLNativeWindow>();
            var window = compositor.NewWindow();

            var list = new TodoForm();
            var document = new FormDocument<TodoForm>(list, 128, 63);

            window.Show("TempleTodo", 1024, 768, document);
        }
    }
}
