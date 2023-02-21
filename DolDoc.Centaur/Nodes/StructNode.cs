using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class StructFieldNode : ASTNode
    {
        public string Name { get; }
        public Type Type { get; }

        public StructFieldNode(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
    
    public class StructNode : DefinitionNode
    {
        private readonly List<StructFieldNode> fields;

        public StructNode(string name, List<StructFieldNode> fields)
        {
            this.fields = fields;
            Name = name;
        }

        public override string Name { get; }

        public Type CreateType(ModuleBuilder moduleBuilder)
        {
            var builder = moduleBuilder.DefineType(Name, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.AutoLayout);
            foreach (var field in fields)
                builder.DefineField(field.Name, field.Type, FieldAttributes.Public);
            return builder.CreateType();
        }
    }
}