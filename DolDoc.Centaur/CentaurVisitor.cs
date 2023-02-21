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

    public abstract class CodeNode : ASTNode
    {
        public abstract void Emit(LoggingILGenerator generator, SymbolTable symbolTable);
    }

    public interface IWrite
    {
        void EmitWrite(LoggingILGenerator generator, SymbolTable symbolTable);
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

    // public class NewObjectAstNode : DataAstNode
    // {
    //     private readonly Type type;
    //
    //     public NewObjectAstNode(Type type) : base(null)
    //     {
    //         this.type = type;
    //     }
    //
    //     public override void EmitRead(LoggingILGenerator generator)
    //     {
    //         generator.Emit(OpCodes.Newobj, type.GetConstructor(new Type[] { }));
    //     }
    //
    //     public override void EmitWrite(LoggingILGenerator generator)
    //     {
    //         throw new NotImplementedException();
    //     }
    // }

    public class CentaurVisitor : centaurBaseVisitor<ASTNode>
    {
        private readonly ILogger logger;
        private Dictionary<string, Type> types;
        private readonly AssemblyBuilder assemblyBuilder;
        private readonly ModuleBuilder moduleBuilder;
        public readonly TypeBuilder codeBuilder;
        
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
        }

        public override ASTNode VisitSymbol(centaurParser.SymbolContext context) =>
            new SymbolNode(context.T_SYMBOL().GetText());

        public override ASTNode VisitVar(centaurParser.VarContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception();
            return new DeclareVariableNode(context.name.Text, type);
        }

        public override ASTNode VisitAssign(centaurParser.AssignContext context)  =>
            new AssignNode((CodeNode) Visit(context.source), (CodeNode) Visit(context.target));

        public override ASTNode VisitVarAssign(centaurParser.VarAssignContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception();
            return new DeclareVariableNode(context.name.Text, type, (CodeNode)Visit(context.value));
        }

        public override ASTNode VisitArithMultiply(centaurParser.ArithMultiplyContext context)  =>
            new MultiplyNode((CodeNode) Visit(context.left), (CodeNode) Visit(context.right));

        public override ASTNode VisitArithShiftLeft(centaurParser.ArithShiftLeftContext context) =>
            new ShiftLeftNode((CodeNode) Visit(context.left), (CodeNode) Visit(context.right));

        public override ASTNode VisitArithShiftRight(centaurParser.ArithShiftRightContext context) =>
            new ShiftRightNode((CodeNode) Visit(context.left), (CodeNode) Visit(context.right));

        public override ASTNode VisitArithAdd(centaurParser.ArithAddContext context) =>
            new AddNode((CodeNode) Visit(context.left), (CodeNode) Visit(context.right));

        public override ASTNode VisitArithSubtract(centaurParser.ArithSubtractContext context) =>
            new SubtractNode((CodeNode) Visit(context.left), (CodeNode) Visit(context.right));

        public override ASTNode VisitConstInteger(centaurParser.ConstIntegerContext context) =>
            new ConstantIntegerNode(long.Parse(context.T_INTEGER().GetText()));

        public override ASTNode VisitConstString(centaurParser.ConstStringContext context) =>
            new ConstantStringNode(context.T_STRING().GetText()[1..^1]);

        public override ASTNode VisitConstNull(centaurParser.ConstNullContext context) =>
            new ConstantNullNode();

        // public override ASTNode VisitMember(centaurParser.MemberContext context)
        // {
        //     var src = Visit(context.ctx) as DataAstNode;
        //     return new StructFieldAstNode(src.symbol, src, context.field.Text);
        // }

        public override ASTNode VisitJump_statement(centaurParser.Jump_statementContext context) =>
            new ReturnNode(Visit(context.value) as CodeNode);

        public override ASTNode VisitNewObj(centaurParser.NewObjContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception("type");

            // return new NewObjectAstNode(type);
            return null;
        }

        public override ASTNode VisitCall(centaurParser.CallContext context) =>
            new CallNode(context.name.GetText());

        public override ASTNode VisitCompound_statement(centaurParser.Compound_statementContext context)
        {
            var nodes = new List<CodeNode>();
            foreach (var child in context.statement_list().children)
            {
                var node = (CodeNode) Visit(child);
                if (node != null)
                    nodes.Add((CodeNode)Visit(child));
            }
            return new ScopeNode(nodes);
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
            
            ParameterListNode parameters = null;
            if (context.parameters != null)
                parameters = Visit(context.parameters) as ParameterListNode;

            var body = Visit(context.body) as ScopeNode;
            return new FunctionDefinitionNode(context.name.Text, returnType, parameters?.Symbols, body);
        }

        public override ASTNode VisitStruct_field(centaurParser.Struct_fieldContext context)
        {
            if (!types.TryGetValue(context.type.Text, out var type))
                throw new Exception($"Unknown type {context.type.Text}");
            return new StructFieldNode(context.name.Text, type);
        }

        public override ASTNode VisitStruct_definition(centaurParser.Struct_definitionContext context)
        {
            var fields = new List<StructFieldNode>();
            foreach (var field in context.struct_field())
                fields.Add(Visit(field) as StructFieldNode);
            return new StructNode(context.name.Text, fields);
        }

        public override ASTNode VisitExpression_statement(centaurParser.Expression_statementContext context) => Visit(context.e);

        public override ASTNode VisitDefinition_list(centaurParser.Definition_listContext context)
        {
            var definitions = new List<DefinitionNode>();
            foreach (var child in context.children)
                definitions.Add((DefinitionNode)Visit(child));
            return new DefinitionListNode(definitions);
        }
    }
}
