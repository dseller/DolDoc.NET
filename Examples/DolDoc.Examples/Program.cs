using System.IO;
using DolDoc.Editor.Compositor;
using DolDoc.Editor.Core;
using DolDoc.Renderer.OpenGL;

namespace DolDoc.Examples
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var window = new OpenTKWindow();

            var obj = new TestForm();
            var list = new TodoForm();
            var sprite = new SpriteDemoForm();

            Document document;
            using (var fs = File.Open("Examples.DD", FileMode.Open))
            {
                document = DocumentLoader.Load(fs, "Examples.DD");
            }

            document.OnMacro += entry =>
            {
                switch (entry.GetArgument("LE"))
                {
                    case "SimpleForm":
                        Compositor.Instance.Root.State.LoadDocument(new FormDocument<TestForm>(obj), true);
                        break;
                    
                    case "TodoList":
                        Compositor.Instance.Root.State.LoadDocument(new FormDocument<TodoForm>(list), true);
                        break;
                    
                    case "Sprites":
                        Compositor.Instance.Root.State.LoadDocument(new FormDocument<SpriteDemoForm>(sprite), true);
                        break;
                    
                    case "FileBrowser":
                        Compositor.Instance.Root.State.LoadDocument(FileBrowser.GetFileBrowserDocument(), true);
                        break;

                    case "NewDocument":
                    {
                        var emptyDocument = new Document();
                        Compositor.Instance.Root.State.LoadDocument(emptyDocument);
                        break;
                    }
                }
            };

            var compositor = Compositor.Initialize(window, 1600, 1200, document);
            compositor.Start();
        }
    }
}