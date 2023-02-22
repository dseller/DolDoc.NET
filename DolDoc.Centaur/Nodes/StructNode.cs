using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using DolDoc.Centaur.Symbols;

namespace DolDoc.Centaur.Nodes
{
    public class StructFieldNode : ASTNode
    {
        public string Name { get; }
        public string Type { get; }

        public StructFieldNode(string name, string type)
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

        public Type CreateType(FunctionCompilerContext ctx)
        {
            var builder = ctx.ModuleBuilder.DefineType(Name, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.AutoLayout);
            foreach (var field in fields)
            {
                var type = ctx.SymbolTable.FindSymbol<TypeSymbol>(field.Type)?.Type;
                builder.DefineField(field.Name, type, FieldAttributes.Public);
            }

            return builder.CreateType();
        }
    }
}