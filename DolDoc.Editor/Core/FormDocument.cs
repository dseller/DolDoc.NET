using DolDoc.Editor.Entries;
using DolDoc.Editor.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DolDoc.Editor.Core
{
    public class FormDocument<T> : Document
        where T : class, new()
    {
        public FormDocument(T dataObject = null, IList<BinaryChunk> binaryChunks = null)
            : base(binaryChunks)
        {
            DataObject = dataObject ?? new T();

            OnButtonClick += FormDocument_OnButtonClick;
            OnFieldChange += FormDocument_OnFieldChange;

            Load(Generate());
        }

        public override void Macro(DocumentEntry entry)
        {
            var method = entry.GetArgument("LE");
            if (string.IsNullOrEmpty(method))
            {
                base.Macro(entry);
                return;
            }

            var methodInfo = typeof(T).GetMethod(method);
            if (methodInfo == null)
            {
                base.Macro(entry);
                return;
            }

            methodInfo.Invoke(DataObject, new object[] { this, entry });
        }

        private void FormDocument_OnFieldChange(string field, object value)
        {
            if (string.IsNullOrWhiteSpace(field))
                return;

            var property = typeof(T).GetProperty(field);
            property.SetValue(DataObject, value);
        }

        private void FormDocument_OnButtonClick(Button obj)
        {
            var handlerMethod = obj.GetArgument("H");
            if (string.IsNullOrEmpty(handlerMethod))
            {
                Debug.WriteLine("No handler method specified.");
                return;
            }

            var methodInfo = typeof(T).GetMethod(handlerMethod);
            if (methodInfo == null)
            {
                Debug.WriteLine($"Could not find method {methodInfo} on type {typeof(T)}");
                return;
            }

            methodInfo.Invoke(DataObject, new object[] { this });
        }

        public override object GetData(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            var property = typeof(T).GetProperty(key);
            return property.GetValue(DataObject);
        }

        public override void Reload() => Load(Generate());

        public T DataObject { get; }

        private string Generate()
        {
            var builder = new StringBuilder();
            builder.Append(GetHeader(typeof(T)));

            var attributes = GetFieldAttributes();
            var maxLabelLength = attributes.Count() == 0 ? 8 : attributes.Max(a => a.Item1.Label?.Length ?? 0);

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

        private string GetHeader(Type formType)
        {
            var builder = new StringBuilder();

            var headerAttribute = formType.GetCustomAttribute<FormHeaderAttribute>();
            if (headerAttribute != null)
                builder.Append(headerAttribute.Header);

            var fnHeaderAttributes = formType.GetCustomAttributes<FormHeaderFunctionAttribute>();
            foreach (var attribute in fnHeaderAttributes)
            {
                var methodInfo = typeof(T).GetMethod(attribute.Function);
                if (methodInfo != null)
                    builder.Append(methodInfo.Invoke(DataObject, new object[] { this }));
            }

            return builder.ToString();
        }
        
        private string GetFooter(Type formType)
        {
            var builder = new StringBuilder();

            var footerAttribute = formType.GetCustomAttribute<FormFooterAttribute>();
            if (footerAttribute != null)
                builder.Append(footerAttribute.Footer);

            var fnFooterAttributes = formType.GetCustomAttributes<FormFooterFunctionAttribute>();
            foreach (var attribute in fnFooterAttributes)
            {
                var methodInfo = typeof(T).GetMethod(attribute.Function);
                if (methodInfo != null)
                    builder.Append(methodInfo.Invoke(DataObject, new object[] { this }));
            }

            return builder.ToString();
        }
    }
}
