using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DolDoc.Centaur.Nodes;
using DolDoc.Shared;

namespace DolDoc.Centaur
{
    public abstract class ASTNode
    {
        
    }

    public abstract class DataAstNode : ASTNode
    {
        public SymbolTable.Symbol symbol;
        
        public DataAstNode(SymbolTable.Symbol symbol)
        {
            this.symbol = symbol;
        }

        public abstract void EmitRead(LoggingILGenerator generator);

        public abstract void EmitWrite(LoggingILGenerator generator);
    }

    public class VariableAstNode : DataAstNode
    {
        public VariableAstNode(SymbolTable.Symbol symbol) : base(symbol)
        {
        }

        public override void EmitRead(LoggingILGenerator generator)
        {
            generator.Emit(OpCodes.Ldloc, symbol.Index);
        }

        public override void EmitWrite(LoggingILGenerator generator)
        {
            generator.Emit(OpCodes.Stloc, symbol.Index);
        }
    }

    public class ParameterAstNode : DataAstNode
    {
        public ParameterAstNode(SymbolTable.Symbol symbol) : base(symbol)
        {
        }

        public override void EmitRead(LoggingILGenerator generator)
        {
            generator.Emit(OpCodes.Ldarg, symbol.Index);
        }

        public override void EmitWrite(LoggingILGenerator generator)
        {
            throw new NotImplementedException();
        }
    }

    class StructAstNode : ASTNode
    {
        private SymbolTable.Symbol symbol;
        private readonly Type type;

        public StructAstNode(SymbolTable.Symbol symbol, Type type)
        {
            this.symbol = symbol;
            this.type = type;
        }

        public void EmitGetField(LoggingILGenerator generator, string name)
        {
            var field = type.GetField(name, BindingFlags.Public);
            generator.Emit(OpCodes.Ldfld, field);
        }

        public void EmitSetField(LoggingILGenerator generator, string name)
        {
            var field = type.GetField(name, BindingFlags.Public);
            generator.Emit(OpCodes.Stfld, field);
        }
    }

    public class StructFieldAstNode : DataAstNode
    {
        public readonly DataAstNode target;
        private readonly string fieldName;

        public StructFieldAstNode(SymbolTable.Symbol symbol, DataAstNode target, string fieldName) : base(symbol)
        {
            this.target = target;
            this.fieldName = fieldName;
        }

        public override void EmitRead(LoggingILGenerator generator)
        {
            var field = symbol.Type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            generator.Emit(OpCodes.Ldfld, field);
        }

        public override void EmitWrite(LoggingILGenerator generator)
        {
            var field = symbol.Type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            generator.Emit(OpCodes.Stfld, field);
        }
    }

    public class ConstantVariableAstNode : VariableAstNode
    {
        public ConstantVariableAstNode() : base(null)
        {
            
        }
    }

    public class ParameterListNode : ASTNode
    {
        public ParameterListNode(IEnumerable<SymbolTable.Symbol> symbols)
        {
            Symbols = new List<SymbolTable.Symbol>(symbols);
        }
        
        public List<SymbolTable.Symbol> Symbols { get; }
        
        
    }

    public class ParameterNode : ASTNode
    {
        public string Name { get; }
        public Type Type { get; }

        public ParameterNode(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }

    public class NewObjectAstNode : DataAstNode
    {
        private readonly Type type;

        public NewObjectAstNode(Type type) : base(null)
        {
            this.type = type;
        }

        public override void EmitRead(LoggingILGenerator generator)
        {
            generator.Emit(OpCodes.Newobj, type.GetConstructor(new Type[] { }));
        }

        public override void EmitWrite(LoggingILGenerator generator)
        {
            throw new NotImplementedException();
        }
    }

    public enum SymbolTarget
    {
        Variable = 0,
        Function = 1,
        Parameter = 2
    }
    
    
    public class CentaurVisitor : centaurBaseVisitor<ASTNode>
    {
        private readonly ILogger logger;
        private Dictionary<string, Type> types;
        private Dictionary<string, MethodInfo> functions;
        private readonly AssemblyBuilder assemblyBuilder;
        private readonly ModuleBuilder moduleBuilder;
        private readonly TypeBuilder codeBuilder;
        private readonly SymbolTable symbolTable;
        
        private LoggingILGenerator generator;
        
        public Assembly Assembly => assemblyBuilder;
        
        public CentaurVisitor(ILogger logger)
        {
            this.logger = logger;
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
            functions = new Dictionary<string, MethodInfo>();
            symbolTable = new SymbolTable();
        }

        public void RegisterFunction(string name, MethodInfo m)
        {
            if (functions.ContainsKey(name))
                throw new Exception($"Function {name} already defined");
            functions.Add(name, m);
            symbolTable.RootSymbols.Add(new SymbolTable.Symbol(name, m.ReturnType, 0, SymbolTarget.Function, m.GetParameters().Select(p => p.ParameterType).ToArray()));
        } 

        public override ASTNode VisitSymbol(centaurParser.SymbolContext context)
        {
            var s = symbolTable.FindSymbol(context.T_SYMBOL().GetText());
            if (s == null)
                throw new Exception($"Could not find {context.T_SYMBOL().GetText()}");

            if (s.Target == SymbolTarget.Variable)
                return new VariableAstNode(s);
            else if (s.Target == SymbolTarget.Parameter)
                return new ParameterAstNode(s);
            throw new Exception($"Unsupported {s.Target}");
        }

        public override ASTNode VisitVar(centaurParser.VarContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception();

            if (symbolTable.IsRootScope)
            {
                var y = "";
            }
            // TODO: validate in whole table not exists
            var x = generator.DeclareLocal(type);
            var symbol = new SymbolTable.Symbol(context.name.Text, type, x.LocalIndex, SymbolTarget.Variable);
            symbolTable.NewSymbol(symbol);
            return new VariableAstNode(symbol);
        }

        public override ASTNode VisitAssign(centaurParser.AssignContext context)
        {
            var source = Visit(context.source) as DataAstNode;
            var target = Visit(context.target) as DataAstNode;

            if (target is StructFieldAstNode sfn)
                sfn.target.EmitRead(generator);
            if (source is StructFieldAstNode sfn2)
                sfn2.target.EmitRead(generator);
            source?.EmitRead(generator);
            target.EmitWrite(generator);

            return null;
        }

        public override ASTNode VisitVarAssign(centaurParser.VarAssignContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception();
            // TODO: validate in whole table not exists
            // if (symbolTable.IsRootScope)
            // {
            //     var field = codeBuilder.DefineField(context.name.Text, type, FieldAttributes.Public | FieldAttributes.Static);
            //     
            // }
            
            var x = generator.DeclareLocal(type);
            var symbol = new SymbolTable.Symbol(context.name.Text, type, x.LocalIndex, SymbolTarget.Variable);
            symbolTable.NewSymbol(symbol);

            var srcNode = Visit(context.value) as DataAstNode;
            srcNode?.EmitRead(generator);

            var targetNode = new VariableAstNode(symbol);
            targetNode.EmitWrite(generator);
            return targetNode;
        }

        public override ASTNode VisitArithMultiply(centaurParser.ArithMultiplyContext context)
        {
            var left = Visit(context.left) as DataAstNode;
            var right = Visit(context.right) as DataAstNode;
            left.EmitRead(generator);
            right.EmitRead(generator);
            generator.Emit(OpCodes.Mul);
            return null;
        }

        public override ASTNode VisitArithShiftLeft(centaurParser.ArithShiftLeftContext context)
        {
            Visit(context.left);
            Visit(context.right);
            generator.Emit(OpCodes.Shl);
            return null;
        }

        public override ASTNode VisitArithShiftRight(centaurParser.ArithShiftRightContext context)
        {
            Visit(context.left);
            Visit(context.right);
            generator.Emit(OpCodes.Shr_Un);
            return null;
        }

        public override ASTNode VisitArithAdd(centaurParser.ArithAddContext context)
        {
            Visit(context.left);
            Visit(context.right);
            generator.Emit(OpCodes.Add);
            return null;
        }

        public override ASTNode VisitArithSubtract(centaurParser.ArithSubtractContext context)
        {
            Visit(context.left);
            Visit(context.right);
            generator.Emit(OpCodes.Sub);
            return null;
        }

        public override ASTNode VisitConstInteger(centaurParser.ConstIntegerContext context)
        {
            // generator.Emit(OpCodes.Ldc_I8, long.Parse(context.T_INTEGER().GetText()));
            return new ConstantIntegerAstNode(long.Parse(context.T_INTEGER().GetText()));
        }

        public override ASTNode VisitConstString(centaurParser.ConstStringContext context)
        {
            generator.Emit(OpCodes.Ldstr, context.T_STRING().GetText()[1..^1]);
            return null;
        }

        public override ASTNode VisitConstNull(centaurParser.ConstNullContext context)
        {
            generator.Emit(OpCodes.Ldnull);
            return null;
        }

        public override ASTNode VisitMember(centaurParser.MemberContext context)
        {
            var src = Visit(context.ctx) as DataAstNode;
            return new StructFieldAstNode(src.symbol, src, context.field.Text);
        }

        public override ASTNode VisitJump_statement(centaurParser.Jump_statementContext context)
        {
            if (context.value != null)
            {
                var node = Visit(context.value) as DataAstNode;
                node?.EmitRead(generator);
            }
            generator.Emit(OpCodes.Ret); 
            return null;
        }

        public override ASTNode VisitNewObj(centaurParser.NewObjContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception("type");

            return new NewObjectAstNode(type);
        }

        public override ASTNode VisitCall(centaurParser.CallContext context)
        {
            var text = context.postfix_expression().GetText();
            // var sym = FindSymbol(text);
            // if (sym.target != SymbolTarget.Function)
            //     throw new Exception("target");

            if (!functions.TryGetValue(text, out var mi))
                throw new Exception("fn");

            var sym = symbolTable.FindSymbol(text);

            if (sym.ParameterTypes != null && sym.ParameterTypes.Length > 0)
            {
                foreach (var child in context.args.children)
                {
                    var x = Visit(child) as DataAstNode;
                    x?.EmitRead(generator);
                }
                generator.EmitCall(OpCodes.Call, mi, sym.ParameterTypes);                
            }
            else
                generator.Emit(OpCodes.Call, mi);  
            return null;
        }

        public override ASTNode VisitCompound_statement(centaurParser.Compound_statementContext context)
        {
            symbolTable.BeginScope();
            VisitChildren(context);
            symbolTable.EndScope();
            return null;
        }

        public override ASTNode VisitParameter(centaurParser.ParameterContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception("type");
            return new ParameterNode(context.name.Text, type);
        }

        public override ASTNode VisitParameter_list(centaurParser.Parameter_listContext context)
        {
            var parameters = new List<ParameterNode>();
            foreach (var child in context.children)
                parameters.Add(Visit(child) as ParameterNode);
            
            return new ParameterListNode(parameters.Select((p, i) => new SymbolTable.Symbol(p.Name, p.Type, i, SymbolTarget.Parameter)));
        }

        public override ASTNode VisitFunction_definition(centaurParser.Function_definitionContext context)
        {
            if (!types.TryGetValue(context.resultType.Text, out var returnType))
                throw new Exception();
            logger.Debug($"=== Function {context.name.Text} ===");

            ParameterListNode parameters = null;
            if (context.parameters != null)
                parameters = Visit(context.parameters) as ParameterListNode;

            var builder = codeBuilder.DefineMethod(context.name.Text, 
                MethodAttributes.Public | MethodAttributes.Static, 
                CallingConventions.Standard, 
                returnType, 
                parameters == null ? Array.Empty<Type>() : parameters.Symbols.Select(s => s.Type).ToArray());
            
            generator = new LoggingILGenerator(logger, builder.GetILGenerator());
            functions.Add(context.name.Text, builder);
            symbolTable.Clear();
            symbolTable.RootSymbols.Add(new SymbolTable.Symbol(context.name.Text, returnType, 0, SymbolTarget.Function, parameters?.Symbols?.Select(s => s.Type).ToArray()));
            
            symbolTable.BeginScope();
            if (parameters != null)
                foreach (var sym in parameters.Symbols)
                    symbolTable.NewSymbol(sym);
            VisitCompound_statement(context.body);
            symbolTable.EndScope();
            
            generator = null;
            logger.Debug($"=== Function END ===");

            return null;
        }

        public override ASTNode VisitStruct_definition(centaurParser.Struct_definitionContext context)
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
