using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DolDoc.Centaur
{
    public record Symbol(string name, Type type, int index, SymbolTarget target); 
    
    public abstract class Node
    {
        
    }

    public abstract class DataNode : Node
    {
        public Symbol symbol;
        
        public DataNode(Symbol symbol)
        {
            this.symbol = symbol;
        }

        public abstract void EmitRead(ILGenerator generator);

        public abstract void EmitWrite(ILGenerator generator);
    }

    public class VariableNode : DataNode
    {
        public VariableNode(Symbol symbol) : base(symbol)
        {
        }

        public override void EmitRead(ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldloc, symbol.index);
            Console.WriteLine($"LDC {symbol.index}");
        }

        public override void EmitWrite(ILGenerator generator)
        {
            generator.Emit(OpCodes.Stloc, symbol.index);
            Console.WriteLine($"STLOC {symbol.index}");
        }
    }

    class StructNode : Node
    {
        private Symbol symbol;
        private readonly Type type;

        public StructNode(Symbol symbol, Type type)
        {
            this.symbol = symbol;
            this.type = type;
        }

        public void EmitGetField(ILGenerator generator, string name)
        {
            var field = type.GetField(name, BindingFlags.Public);
            generator.Emit(OpCodes.Ldfld, field);
            Console.WriteLine($"LDFLD {field}");
        }

        public void EmitSetField(ILGenerator generator, string name)
        {
            var field = type.GetField(name, BindingFlags.Public);
            generator.Emit(OpCodes.Stfld, field);
            Console.WriteLine($"STFLD {field}");
        }
    }

    public class StructFieldNode : DataNode
    {
        public readonly DataNode target;
        private readonly string fieldName;

        public StructFieldNode(Symbol symbol, DataNode target, string fieldName) : base(symbol)
        {
            this.target = target;
            this.fieldName = fieldName;
        }

        public override void EmitRead(ILGenerator generator)
        {
            var field = symbol.type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            generator.Emit(OpCodes.Ldfld, field);
            Console.WriteLine($"LDFLD {field}");
        }

        public override void EmitWrite(ILGenerator generator)
        {
            var field = symbol.type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            generator.Emit(OpCodes.Stfld, field);
            Console.WriteLine($"STFLD {field}");
        }
    }

    public class ConstantVariableNode : VariableNode
    {
        public ConstantVariableNode() : base(null)
        {
            
        }
    }

    public class ConstantIntegerNode : ConstantVariableNode
    {
        private readonly long value;

        public ConstantIntegerNode(long value)
        {
            this.value = value;
        }

        public override void EmitRead(ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldc_I8, value);
            Console.WriteLine($"Ldc_I8 {value}");
        }
    }

    
    public class NewObjectNode : DataNode
    {
        private readonly Type type;

        public NewObjectNode(Type type) : base(null)
        {
            this.type = type;
        }

        public override void EmitRead(ILGenerator generator)
        {
            generator.Emit(OpCodes.Newobj, type.GetConstructor(new Type[] { }));
            Console.WriteLine($"Newobj {type}");
        }

        public override void EmitWrite(ILGenerator generator)
        {
            throw new NotImplementedException();
        }
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
        public readonly AssemblyBuilder assemblyBuilder;
        private readonly ModuleBuilder moduleBuilder;
        private readonly TypeBuilder codeBuilder;
        
        private ILGenerator generator;
        private Dictionary<string, LocalBuilder> variables;
        
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
                return new VariableNode(s);
            return null;
        }

        public override Node VisitVar(centaurParser.VarContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception();
            // TODO: validate in whole table not exists
            var x = generator.DeclareLocal(type);
            var symbol = new Symbol(context.name.Text, type, x.LocalIndex, SymbolTarget.Variable);
            CurrentSymbolTable.Add(symbol);
            return new VariableNode(symbol);
        }

        public override Node VisitAssign(centaurParser.AssignContext context)
        {
            var source = Visit(context.source) as DataNode;
            var target = Visit(context.target) as DataNode;
            
            if (target is StructFieldNode sfn)
                sfn.target.EmitRead(generator);
            if (source is StructFieldNode sfn2)
                sfn2.target.EmitRead(generator);
            source.EmitRead(generator);
            target.EmitWrite(generator);
            
            return null;
        }

        public override Node VisitVarAssign(centaurParser.VarAssignContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception();
            // TODO: validate in whole table not exists
            var x = generator.DeclareLocal(type);
            var symbol = new Symbol(context.name.Text, type, x.LocalIndex, SymbolTarget.Variable);
            CurrentSymbolTable.Add(symbol);

            var srcNode = Visit(context.value) as DataNode;
            srcNode.EmitRead(generator);

            var targetNode = new VariableNode(symbol);
            targetNode.EmitWrite(generator);
            return targetNode;
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
            // generator.Emit(OpCodes.Ldc_I8, long.Parse(context.T_INTEGER().GetText()));
            return new ConstantIntegerNode(long.Parse(context.T_INTEGER().GetText()));
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

        public override Node VisitMember(centaurParser.MemberContext context)
        {
            var src = Visit(context.ctx) as DataNode;
            return new StructFieldNode(src.symbol, src, context.field.Text);
        }

        public override Node VisitJump_statement(centaurParser.Jump_statementContext context)
        {
            if (context.value != null)
            {
                var node = Visit(context.value) as DataNode;
                node.EmitRead(generator);
                
            }
            generator.Emit(OpCodes.Ret);
            return null;
        }

        public override Node VisitNewObj(centaurParser.NewObjContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception("type");

            return new NewObjectNode(type);
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