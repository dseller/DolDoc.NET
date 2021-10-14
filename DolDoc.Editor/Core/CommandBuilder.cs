using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DolDoc.Editor.Core
{
    internal class CommandBuilder
    {
        private readonly string command;
        private readonly Dictionary<string, string> parameters;
        private string tag, prefix, suffix;

        internal CommandBuilder(string command)
        {
            this.command = command;
            prefix = string.Empty;
            suffix = string.Empty;
            parameters = new Dictionary<string, string>();
        }

        internal CommandBuilder WithTag(string tag)
        {
            this.tag = tag;
            return this;
        }

        internal CommandBuilder WithNamedParameter(string key, string value, bool condition = true)
        {
            if (condition)
                parameters.Add(key, value);
            return this;
        }

        internal CommandBuilder WithPrefix(string prefix)
        {
            this.prefix = prefix;
            return this;
        }

        internal CommandBuilder WithSuffix(string suffix)
        {
            this.suffix = suffix;
            return this;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"{prefix}${command}");

            if (!string.IsNullOrEmpty(tag))
                builder.Append($",\"{tag}\"");

            if (parameters.Count > 0)
            {
                builder.Append(",");
                builder.Append(string.Join(", ", parameters.Select(param => $"{param.Key}=\"{param.Value}\"")));
            }

            builder.Append($"${suffix}");
            return builder.ToString();
        }
    }
}
