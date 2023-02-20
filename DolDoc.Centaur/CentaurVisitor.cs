using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DolDoc.Centaur
{
    public record Symbol(string name, Type type, int index, SymbolTarget target); 
    
    public class Node
    {
        
    }

    public enum SymbolTarget
    {
        Variable = 0,
        Function = 1
    }
    
    public class CentaurVisitor : centaurBaseVisitor<Node>
    {
        private Dictionary<string, Type> types;
        private Dictionary<string, MethodInfo> functions;
        private readonly AssemblyBuilder assemblyBuilder;
        private readonly ModuleBuilder moduleBuilder;
        private readonly TypeBuilder codeBuilder;
        
        private ILGenerator generator;
        private Dictionary<string, LocalBuilder> variables;
        private int varCounter;
        
        private Stack<List<Symbol>> symbols;
        private List<Symbol> rootSymbols;

        private List<Symbol> CurrentSymbolTable => symbols.LastOrDefault();

        private Symbol FindSymbol(string name)
        {
            foreach (var scope in symbols.Reverse())
            {
                var symbol = scope.Find(s => s.name == name);
                if (symbol != default)
                    return symbol;
            }

            return null;
        }
        
        public CentaurVisitor()
        {
            types = new Dictionary<string, Type>
            {
                { "byte", typeof(byte) },
                { "int", typeof(uint) },
                { "bool", typeof(bool) },
                { "string", typeof(string) },
                { "object", typeof(object) }
            };
            
            assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("test"), AssemblyBuilderAccess.RunAndCollect);
            moduleBuilder = assemblyBuilder.DefineDynamicModule("test");
            codeBuilder = moduleBuilder.DefineType("gen_code");
            rootSymbols = new List<Symbol>();
            functions = new Dictionary<string, MethodInfo>();
        }

        public override Node VisitSymbol(centaurParser.SymbolContext context)
        {
            var s = FindSymbol(context.T_SYMBOL().GetText());
            if (s == null)
                throw new Exception("s is null");

            if (s.target == SymbolTarget.Variable)
                generator.Emit(OpCodes.Ldloc, s.index);
            return null;
        }

        public override Node VisitVar(centaurParser.VarContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception();
            // TODO: validate in whole table not exists
            var x = generator.DeclareLocal(type);
            CurrentSymbolTable.Add(new Symbol(context.name.Text, type, x.LocalIndex, SymbolTarget.Variable));
            
            return null;
        }

        public override Node VisitVarAssign(centaurParser.VarAssignContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception();
            // TODO: validate in whole table not exists
            var x = generator.DeclareLocal(type);
            CurrentSymbolTable.Add(new Symbol(context.name.Text, type, x.LocalIndex, SymbolTarget.Variable));

            Visit(context.value);
            generator.Emit(OpCodes.Stloc, x.LocalIndex);
            
            return null;
        }

        public override Node VisitArithShiftLeft(centaurParser.ArithShiftLeftContext context)
        {
            Visit(context.left);
            Visit(context.right);
            generator.Emit(OpCodes.Shl);
            return null;
        }

        public override Node VisitArithShiftRight(centaurParser.ArithShiftRightContext context)
        {
            Visit(context.left);
            Visit(context.right);
            generator.Emit(OpCodes.Shr_Un);
            return null;
        }

        public override Node VisitArithAdd(centaurParser.ArithAddContext context)
        {
            Visit(context.left);
            Visit(context.right);
            generator.Emit(OpCodes.Add);
            return null;
        }

        public override Node VisitArithSubtract(centaurParser.ArithSubtractContext context)
        {
            Visit(context.left);
            Visit(context.right);
            generator.Emit(OpCodes.Sub);
            return null;
        }

        public override Node VisitConstInteger(centaurParser.ConstIntegerContext context)
        {
            generator.Emit(OpCodes.Ldc_I8, long.Parse(context.T_INTEGER().GetText()));
            return null;
        }

        public override Node VisitConstString(centaurParser.ConstStringContext context)
        {
            generator.Emit(OpCodes.Ldstr, context.T_STRING().GetText()[1..^1]);
            return null;
        }

        public override Node VisitConstNull(centaurParser.ConstNullContext context)
        {
            generator.Emit(OpCodes.Ldnull);
            return null;
        }

        public override Node VisitJump_statement(centaurParser.Jump_statementContext context)
        {
            if (context.value != null)
                Visit(context.value);
            generator.Emit(OpCodes.Ret);
            return null;
        }

        public override Node VisitNewObj(centaurParser.NewObjContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception("type");
            
            generator.Emit(OpCodes.Newobj, type.GetConstructor(new Type[] {}));
            return null;
        }

        public override Node VisitCall(centaurParser.CallContext context)
        {
            var text = context.postfix_expression().GetText();
            // var sym = FindSymbol(text);
            // if (sym.target != SymbolTarget.Function)
            //     throw new Exception("target");

            if (!functions.TryGetValue(text, out var mi))
                throw new Exception("fn");
        
            generator.Emit(OpCodes.Call, mi);
            return null;
        }

        public override Node VisitCompound_statement(centaurParser.Compound_statementContext context)
        {
            symbols.Push(new());
            VisitChildren(context);
            symbols.Pop();
            return null;
        }

        public override Node VisitFunction_definition(centaurParser.Function_definitionContext context)
        {
            if (!types.TryGetValue(context.resultType.Text, out var returnType))
                throw new Exception();
            var builder = codeBuilder.DefineMethod(context.name.Text, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, returnType, new Type[]{ });
            generator = builder.GetILGenerator();
            variables = new Dictionary<string, LocalBuilder>();
            functions.Add(context.name.Text, builder);
            symbols = new();
            rootSymbols.Add(new Symbol(context.resultType.Text, returnType, 0, SymbolTarget.Function));
            VisitCompound_statement(context.body);
            generator = null;
            variables = null;
            symbols = null;

            return null;
        }

        public override Node VisitStruct_definition(centaurParser.Struct_definitionContext context)
        {
            var builder = moduleBuilder.DefineType(context.name.Text, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.AutoLayout);
            foreach (var field in context.struct_field())
            {
                if (!types.TryGetValue(field.type.Text, out var type))
                    throw new Exception($"Unknown type {field.type.Text}");
                builder.DefineField(field.name.Text, type, FieldAttributes.Public);
            }
            
            types.Add(context.name.Text, builder.CreateType());
            return null;
        }

        public Type Save()
        {
            return codeBuilder.CreateType();
        }
    }
}