using DolDoc.Editor.Forms;
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
        public FormDocument(T dataObject, int columns = 80, int rows = 60, EgaColor defaultBgColor = EgaColor.White, EgaColor defaultFgColor = EgaColor.Black, IList<BinaryChunk> binaryChunks = null)
            : base(columns, rows, defaultBgColor, defaultFgColor, binaryChunks)
        {
            DataObject = dataObject;

            Load(Generate());
        }

        public T DataObject { get; }

        private string Generate()
        {
            var builder = new StringBuilder();
            var type = typeof(T);
            var properties = type.GetProperties();

            builder.Append(GetHeader(type));
            foreach (var property in properties)
            {
                var propType = property.PropertyType;
                var attribs = property.GetCustomAttributes(true);

                var fieldAttr = attribs.OfType<IFieldAttribute>().FirstOrDefault();
                if (fieldAttr == null)
                    continue;

                builder.Append(fieldAttr.GetDolDocCommand(propType));

                //var attrs = property.GetCustomAttributes(true);

                //var dataFieldAttr = attrs.OfType<DataFieldAttribute>().SingleOrDefault();
                //if (dataFieldAttr == null)
                //    continue;

                //builder.Append(dataFieldAttr.GetDolDocCommand(propType));
            }

            return builder.ToString();
        }
        
        private string GetHeader(Type formType)
        {
            var headerAttribute = formType.GetCustomAttribute<FormHeaderAttribute>();
            return headerAttribute?.Header ?? string.Empty;
        }
    }
}
