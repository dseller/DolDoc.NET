// <copyright file="FormDocument.cs" company="Dennis Seller">
// Copyright (c) Dennis Seller. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DolDoc.Editor.Entries;
using DolDoc.Editor.Forms;
using Serilog;

namespace DolDoc.Editor.Core
{
    public class FormDocument<T> : Document
        where T : class, new()
    {
        public FormDocument(T dataObject = null, int columns = 80, int rows = 60, IList<BinaryChunk> binaryChunks = null)
            : base(columns, rows, binaryChunks)
        {
            DataObject = dataObject ?? new T();

            OnButtonClick += FormDocument_OnButtonClick;
            OnFieldChange += FormDocument_OnFieldChange;
            OnMacro += FormDocument_OnMacro;

            Load(Generate());
        }

        public T DataObject { get; }

        public override object GetData(string key)
        {
            var property = typeof(T).GetProperty(key);
            return property.GetValue(DataObject);
        }

        private void FormDocument_OnMacro(Macro obj)
        {
            var method = obj.GetArgument("LE");
            if (string.IsNullOrEmpty(method))
            {
                Log.Warning("No method {0} for macro", method);
                return;
            }

            var methodInfo = typeof(T).GetMethod(method);
            if (methodInfo == null)
            {
                Log.Warning("Could not find method {0} on type {1}", methodInfo, typeof(T));
                return;
            }

            methodInfo.Invoke(DataObject, new object[] { this, obj });
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

        private string GetHeader(Type formType) =>
            formType.GetCustomAttribute<FormHeaderAttribute>()?.Header ?? string.Empty;

        private string GetFooter(Type formType) =>
            formType.GetCustomAttribute<FormFooterAttribute>()?.Footer ?? string.Empty;
    }
}
