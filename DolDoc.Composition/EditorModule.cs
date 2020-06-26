using Autofac;
using DolDoc.Core.Editor;
using DolDoc.Editor;
using DolDoc.Editor.Input;
using DolDoc.Editor.Rendering;
using System;

namespace DolDoc.Composition
{
    public class EditorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register input listeners.
            /*builder.RegisterType<EditorState>().As<IInputListener>();
            builder.RegisterType<ViewerState>().As<IInputListener>();*/

            builder.RegisterType<InputManager>();
            //builder.RegisterType<ViewerState>().As<ITickListener>();
        }
    }
}
