using System;
using System.Collections.Generic;
using System.Text;
using DolDoc.Editor.Compositor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Forms;

namespace DolDoc.Examples
{
    public class TodoItem
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public bool Done { get; set; }

        public TodoItem(Guid id, bool done, string text)
        {
            Id = id;
            Done = done;
            Text = text;
        }
    }

    [FormHeader("$UL,1$$TX+CX,\"Todo List\"$$UL,0$\n\n")]
    [FormHeaderFunction("GetTodoList")]
    [FormFooterFunction("GetValidationErrors")]
    public class TodoForm
    {
        private readonly List<TodoItem> items;
        private string validationError;

        public TodoForm()
        {
            items = new List<TodoItem>();
            items.Add(new TodoItem(Guid.NewGuid(), true, "Buy milk"));
            items.Add(new TodoItem(Guid.NewGuid(), false, "Go shopping"));
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
                    builder.Append($" $CB,RE=1,LE=\"ToggleDone\",LM=\"{item.Id}\"$ $FG,GREEN${item.Text}$FG$ $MA,\"Delete\",LE=\"Delete\",LM=\"{item.Id}\"$\n");
                else
                    builder.Append($" $CB,RE=0,LE=\"ToggleDone\",LM=\"{item.Id}\"$ $FG,RED${item.Text}$FG$ $MA,\"Delete\",LE=\"Delete\",LM=\"{item.Id}\"$\n");
            }

            builder.Append("\n\n$FG,CYAN$Create entry:$FG$\n");
            return builder.ToString();
        }

        public string GetValidationErrors(FormDocument<TodoForm> document)
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

            items.Add(new TodoItem(Guid.NewGuid(), IsDone, Item));
            IsDone = false;
            Item = string.Empty;
            validationError = null;
            document.Reload();
        }

        public void Delete(FormDocument<TodoForm> document, DocumentEntry entry)
        {
            var id = Guid.Parse(entry.GetArgument("LM"));
            var item = items.Find(i => i.Id == id);
            if (item == null)
                return;

            var modal = new ConfirmationModal($"\n Are you sure you want to delete $FG,RED${item.Text}$FG$?", () =>
            {
                items.Remove(item);
                document.Reload();    
            });

            Compositor.Instance.NewWindow("Confirm", 50, 8, 10, 10, modal);
        }

        public void ToggleDone(FormDocument<TodoForm> document, DocumentEntry entry)
        {
            var id = Guid.Parse(entry.GetArgument("LM"));
            var item = items.Find(i => i.Id == id);
            if (item == null)
                return;

            item.Done = !item.Done;
            document.Reload();
        }
    }
}
