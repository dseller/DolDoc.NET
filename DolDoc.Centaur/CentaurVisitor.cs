using System;
using System.Collections.Generic;
using System.Globalization;
using DolDoc.Centaur.Nodes;
using DolDoc.Shared;

namespace DolDoc.Centaur
{
    public abstract class ASTNode
    {
    }

    public interface IBytecodeEmitter
    {
        public void Emit(FunctionCompilerContext ctx);

        public Type Type(ICompilerContext ctx);
    }

    public interface IWrite
    {
        void EmitWrite(FunctionCompilerContext ctx);
    }

    public class CentaurVisitor : centaurBaseVisitor<ASTNode>
    {
        private readonly ILogger logger;

        public CentaurVisitor(ILogger logger)
        {
            this.logger = logger;
        }

        public override ASTNode VisitSymbol(centaurParser.SymbolContext context) =>
            new SymbolNode(context.T_SYMBOL().GetText());

        public override ASTNode VisitVar(centaurParser.VarContext context) =>
            new DeclareVariableNode(context.name.Text, context.type.Text);

        public override ASTNode VisitAssign(centaurParser.AssignContext context) =>
            new AssignNode((IBytecodeEmitter) Visit(context.source), (IBytecodeEmitter) Visit(context.target));

        public override ASTNode VisitVarAssign(centaurParser.VarAssignContext context) =>
            new DeclareVariableNode(context.name.Text, context.type.Text, (IBytecodeEmitter) Visit(context.value));

        public override ASTNode VisitArithMultiply(centaurParser.ArithMultiplyContext context) =>
            new MultiplyNode((IBytecodeEmitter) Visit(context.left), (IBytecodeEmitter) Visit(context.right));

        public override ASTNode VisitArithShiftLeft(centaurParser.ArithShiftLeftContext context) =>
            new ShiftLeftNode((IBytecodeEmitter) Visit(context.left), (IBytecodeEmitter) Visit(context.right));

        public override ASTNode VisitArithShiftRight(centaurParser.ArithShiftRightContext context) =>
            new ShiftRightNode((IBytecodeEmitter) Visit(context.left), (IBytecodeEmitter) Visit(context.right));

        public override ASTNode VisitArithAdd(centaurParser.ArithAddContext context) =>
            new AddNode((IBytecodeEmitter) Visit(context.left), (IBytecodeEmitter) Visit(context.right));

        public override ASTNode VisitArithSubtract(centaurParser.ArithSubtractContext context) =>
            new SubtractNode((IBytecodeEmitter) Visit(context.left), (IBytecodeEmitter) Visit(context.right));

        public override ASTNode VisitConstInteger(centaurParser.ConstIntegerContext context) =>
            new ConstantIntegerNode(long.Parse(context.T_INTEGER().GetText()));

        public override ASTNode VisitConstHexInteger(centaurParser.ConstHexIntegerContext context) =>
            new ConstantIntegerNode(long.Parse(context.T_HEX_INTEGER().GetText()[2..], NumberStyles.AllowHexSpecifier));

        public override ASTNode VisitConstString(centaurParser.ConstStringContext context) =>
            new ConstantStringNode(context.T_STRING().GetText()[1..^1]);

        public override ASTNode VisitConstTrue(centaurParser.ConstTrueContext context) =>
            new ConstantBooleanNode(true);

        public override ASTNode VisitConstFalse(centaurParser.ConstFalseContext context) =>
            new ConstantBooleanNode(false);

        public override ASTNode VisitConstNull(centaurParser.ConstNullContext context) =>
            new ConstantNullNode();

        public override ASTNode VisitMember(centaurParser.MemberContext context) =>
            new MemberNode((IBytecodeEmitter)Visit(context.ctx), context.field.Text);

        public override ASTNode VisitJump_statement(centaurParser.Jump_statementContext context) =>
            new ReturnNode(Visit(context.value) as IBytecodeEmitter);

        public override ASTNode VisitNewObj(centaurParser.NewObjContext context) =>
            new NewNode(context.type.Text);

        public override ASTNode VisitEquals(centaurParser.EqualsContext context) =>
            new EqualsNode((IBytecodeEmitter)Visit(context.left), (IBytecodeEmitter)Visit(context.right));

        public override ASTNode VisitNotEquals(centaurParser.NotEqualsContext context) =>
            new NotEqualsNode((IBytecodeEmitter)Visit(context.left), (IBytecodeEmitter)Visit(context.right));

        public override ASTNode VisitLessThan(centaurParser.LessThanContext context) =>
            new LessThanNode((IBytecodeEmitter)Visit(context.left), (IBytecodeEmitter)Visit(context.right));

        public override ASTNode VisitGreaterThan(centaurParser.GreaterThanContext context) =>
            new GreaterThanNode((IBytecodeEmitter)Visit(context.left), (IBytecodeEmitter)Visit(context.right));

        public override ASTNode VisitLessThanOrEqual(centaurParser.LessThanOrEqualContext context) =>
            new LessThanOrEqualNode((IBytecodeEmitter)Visit(context.left), (IBytecodeEmitter)Visit(context.right));

        public override ASTNode VisitGreaterThanOrEqual(centaurParser.GreaterThanOrEqualContext context) =>
            new GreaterThanOrEqualNode((IBytecodeEmitter)Visit(context.left), (IBytecodeEmitter)Visit(context.right));

        public override ASTNode VisitBitwiseAnd(centaurParser.BitwiseAndContext context) =>
            new BitwiseAndNode((IBytecodeEmitter)Visit(context.left), (IBytecodeEmitter)Visit(context.right));

        public override ASTNode VisitBitwiseOr(centaurParser.BitwiseOrContext context) =>
            new BitwiseOrNode((IBytecodeEmitter)Visit(context.left), (IBytecodeEmitter)Visit(context.right));

        public override ASTNode VisitXor(centaurParser.XorContext context) =>
            new BitwiseXorNode((IBytecodeEmitter)Visit(context.left), (IBytecodeEmitter)Visit(context.right));

        public override ASTNode VisitCall(centaurParser.CallContext context)
        {
            var arguments = new List<IBytecodeEmitter>();
            if (context.args != null)
            {
                foreach (var child in context.args.children)
                {
                    var be = (IBytecodeEmitter)Visit(child);
                    if (be != null)
                        arguments.Add(be);
                }
            }

            return new CallNode(context.name.GetText(), arguments);
        }


        public override ASTNode VisitCompound_statement(centaurParser.Compound_statementContext context)
        {
            var nodes = new List<IBytecodeEmitter>();
            foreach (var child in context.statement_list().children)
            {
                var node = (IBytecodeEmitter) Visit(child);
                if (node != null)
                    nodes.Add(node);
            }

            return new ScopeNode(nodes);
        }

        public override ASTNode VisitParameter(centaurParser.ParameterContext context) =>
            new ParameterNode(context.name.Text, context.type.Text);

        public override ASTNode VisitParameter_list(centaurParser.Parameter_listContext context)
        {
            var parameters = new List<ParameterNode>();
            foreach (var child in context.children)
                parameters.Add(Visit(child) as ParameterNode);
            return new ParameterListNode(parameters);
        }

        public override ASTNode VisitFunction_definition(centaurParser.Function_definitionContext context)
        {
            ParameterListNode parameters = null;
            if (context.parameters != null)
                parameters = Visit(context.parameters) as ParameterListNode;

            var body = Visit(context.body) as ScopeNode;
            return new FunctionDefinitionNode(context.name.Text, context.resultType.Text, parameters, body);
        }

        public override ASTNode VisitStruct_field(centaurParser.Struct_fieldContext context) =>
            new StructFieldNode(context.name.Text, context.type.Text);

        public override ASTNode VisitStruct_definition(centaurParser.Struct_definitionContext context)
        {
            var fields = new List<StructFieldNode>();
            foreach (var field in context.struct_field())
                fields.Add(Visit(field) as StructFieldNode);
            return new StructNode(context.name.Text, fields);
        } 

        public override ASTNode VisitIf(centaurParser.IfContext context) =>
            new IfNode((IBytecodeEmitter)Visit(context.expr), (IBytecodeEmitter)Visit(context.body));

        public override ASTNode VisitIfElse(centaurParser.IfElseContext context) =>
            new IfElseNode((IBytecodeEmitter)Visit(context.expr),
                (IBytecodeEmitter)Visit(context.body),
                (IBytecodeEmitter)Visit(context.elseBody));

        public override ASTNode VisitExpression_statement(centaurParser.Expression_statementContext context) => Visit(context.e);

        public override ASTNode VisitDefinition_list(centaurParser.Definition_listContext context)
        {
            var definitions = new List<ASTNode>();
            foreach (var child in context.children)
                definitions.Add(Visit(child));
            return new DefinitionListNode(definitions);
        }
    }
}