using DolDoc.Editor.Entries;
using DolDoc.Editor.Forms;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DolDoc.Editor.Core
{
    public class FormDocument<T> : Document
        where T : class, new()
    {
        public FormDocument(T dataObject = null, int columns = 80, int rows = 60, EgaColor defaultBgColor = EgaColor.White, EgaColor defaultFgColor = EgaColor.Black, IList<BinaryChunk> binaryChunks = null)
            : base(columns, rows, defaultBgColor, defaultFgColor, binaryChunks)
        {
            DataObject = dataObject ?? new T();

            OnButtonClick += FormDocument_OnButtonClick;
            OnFieldChange += FormDocument_OnFieldChange;

            Load(Generate());
        }

        private void FormDocument_OnFieldChange(string field, object value)
        {
            var property = typeof(T).GetProperty(field);
            property.SetValue(DataObject, value);
        }

        private void FormDocument_OnButtonClick(Button obj)
        {
            var handlerMethod = obj.GetArgument("H");
            if (string.IsNullOrEmpty(handlerMethod))
            {
                Log.Warning("No handler method specified.");
                return;
            }

            var methodInfo = typeof(T).GetMethod(handlerMethod);
            if (methodInfo == null)
            {
                Log.Warning("Could not find method {0} on type {1}", methodInfo, typeof(T));
                return;
            }

            methodInfo.Invoke(DataObject, new object[] { this });
        }

        public T DataObject { get; }

        private string Generate()
        {
            var builder = new StringBuilder();
            builder.Append(GetHeader(typeof(T)));

            var attributes = GetFieldAttributes();
            var maxLabelLength = attributes.Max(a => a.Item1.Label.Length);

            foreach (var attribute in attributes)
                builder.Append(attribute.Item1.GetDolDocCommand(attribute.Item2, attribute.Item3, maxLabelLength));

            builder.Append(GetFooter(typeof(T)));
            return builder.ToString();
        }

        private IEnumerable<(IFieldAttribute, Type, string)> GetFieldAttributes()
        {
            var type = typeof(T);
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var propType = property.PropertyType;
                var attribs = property.GetCustomAttributes(true);

                var fieldAttr = attribs.OfType<IFieldAttribute>().FirstOrDefault();
                if (fieldAttr == null)
                    continue;

                yield return (fieldAttr, propType, property.Name);
            }
        }

        private string GetHeader(Type formType) =>
            formType.GetCustomAttribute<FormHeaderAttribute>()?.Header ?? string.Empty;
        
        private string GetFooter(Type formType) =>
            formType.GetCustomAttribute<FormFooterAttribute>()?.Footer ?? string.Empty;
    }
}
