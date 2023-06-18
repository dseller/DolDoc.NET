using System.Collections.Generic;
using System.IO;
using DolDoc.Editor.Compositor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using DolDoc.Renderer.OpenGL;

namespace DolDoc.Examples
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var window = new OpenTKWindow();

            Document document;
            using (var fs = File.Open("Examples.DD", FileMode.Open))
            {
                document = DocumentLoader.Load(fs, "Examples.DD");
            }

            document.OnMacro += OnMacro;
            var compositor = Compositor.Initialize(window, 1600, 1200, document);

            if (args.Length > 0)
                OnMacro(new Macro(new List<Flag>(), new List<Argument>()
                {
                    new Argument("LE", args[0])
                }));

            compositor.Start();
        }

        private static void OnMacro(DocumentEntry entry)
        {
            switch (entry.GetArgument("LE"))
            {
                case "SimpleForm":
                    Compositor.Instance.Root.State.LoadDocument(new FormDocument<TestForm>(new TestForm()), true);
                    break;

                case "TodoList":
                    Compositor.Instance.Root.State.LoadDocument(new FormDocument<TodoForm>(new TodoForm()), true);
                    break;

                case "Sprites":
                    Compositor.Instance.Root.State.LoadDocument(new FormDocument<SpriteDemoForm>(new SpriteDemoForm()), true);
                    break;

                case "FileBrowser":
                    Compositor.Instance.Root.State.LoadDocument(FileBrowser.GetFileBrowserDocument(), true);
                    break;
            }
        }
    }
}