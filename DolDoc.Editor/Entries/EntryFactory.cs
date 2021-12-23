using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DolDoc.Editor.Entries
{
    public static class EntryFactory
    {
        private delegate DocumentEntry FactoryMethod(IList<Flag> flags, IList<Argument> args);
        private static readonly Lazy<Dictionary<string, FactoryMethod>> FactoryMethods = new Lazy<Dictionary<string, FactoryMethod>>(Initialize);

        public static DocumentEntry Create(string cmd, IList<Flag> flags, IList<Argument> args)
        {
            if (!FactoryMethods.Value.TryGetValue(cmd, out var fn))
                return new Error($"Unrecognized cmd '{cmd}'.");

            return fn(flags, args);
        }

        private static Dictionary<string, FactoryMethod> Initialize()
        {
            var result = new Dictionary<string, FactoryMethod>();
            var assembly = typeof(EntryFactory).Assembly;
            var types = assembly.GetTypes();

            var paramFlags = Expression.Parameter(typeof(IList<Flag>), "flags");
            var paramArgs = Expression.Parameter(typeof(IList<Argument>), "args");

            foreach (var type in types)
            {
                var attrib = type.GetCustomAttribute<EntryAttribute>();
                if (attrib == null)
                    continue;

                var ctor = type.GetConstructor(new[] { typeof(IList<Flag>), typeof(IList<Argument>) });
                if (ctor == null)
                    continue;

                var exp = Expression.New(ctor, paramFlags, paramArgs);
                var lambda = Expression.Lambda(typeof(FactoryMethod), exp, new[] { paramFlags, paramArgs });

                var fn = (FactoryMethod)lambda.Compile();

                result.Add(attrib.Command, fn);
            }

            return result;
        }
    }
}
