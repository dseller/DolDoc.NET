using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using DolDoc.Centaur.Symbols;

namespace DolDoc.Centaur.Nodes
{
    public class ClassFieldNode : ASTNode
    {
        public string Name { get; }
        public string Type { get; }

        public ClassFieldNode(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
    
    public class ClassNode : DefinitionNode
    {
        private readonly string baseClass;
        private readonly List<ClassFieldNode> fields;

        public ClassNode(string name, string baseClass, List<ClassFieldNode> fields)
        {
            this.baseClass = baseClass;
            this.fields = fields;
            Name = name;
        }

        public override string Name { get; }

        public void AddToSymbolTable(ICompilerContext ctx)
        {
            var type = CreateType(ctx);
            ctx.SymbolTable.NewSymbol(new TypeSymbol(Name, type));
        }

        public Type CreateType(ICompilerContext ctx)
        {
            Type baseType = null;
            if (baseClass != null)
            {
                var sym = ctx.SymbolTable.FindSymbol<TypeSymbol>(baseClass);
                baseType = sym.Type;
            }
            
            var builder = ctx.ModuleBuilder.DefineType(Name, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.AutoLayout, baseType);
            foreach (var field in fields)
            {
                var type = ctx.SymbolTable.FindSymbol<TypeSymbol>(field.Type)?.Type;
                builder.DefineField(field.Name, type, FieldAttributes.Public);
            }

            return builder.CreateType();
        }
    }
}