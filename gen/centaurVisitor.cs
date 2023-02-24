//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.11.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Z:/Projects/DolDoc.NET/DolDoc.Centaur\centaur.g4 by ANTLR 4.11.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="centaurParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.11.1")]
[System.CLSCompliant(false)]
public interface IcentaurVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.start"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStart([NotNull] centaurParser.StartContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ignore01</c>
	/// labeled alternative in <see cref="centaurParser.postfix_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIgnore01([NotNull] centaurParser.Ignore01Context context);
	/// <summary>
	/// Visit a parse tree produced by the <c>call</c>
	/// labeled alternative in <see cref="centaurParser.postfix_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCall([NotNull] centaurParser.CallContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>decrement</c>
	/// labeled alternative in <see cref="centaurParser.postfix_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDecrement([NotNull] centaurParser.DecrementContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>index</c>
	/// labeled alternative in <see cref="centaurParser.postfix_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIndex([NotNull] centaurParser.IndexContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>getMember</c>
	/// labeled alternative in <see cref="centaurParser.postfix_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGetMember([NotNull] centaurParser.GetMemberContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>increment</c>
	/// labeled alternative in <see cref="centaurParser.postfix_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIncrement([NotNull] centaurParser.IncrementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.mul_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMul_expression([NotNull] centaurParser.Mul_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.add_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAdd_expression([NotNull] centaurParser.Add_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.shift_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitShift_expression([NotNull] centaurParser.Shift_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.relational_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRelational_expression([NotNull] centaurParser.Relational_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.equality_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEquality_expression([NotNull] centaurParser.Equality_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.bitwise_and_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBitwise_and_expression([NotNull] centaurParser.Bitwise_and_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.bitwise_or_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBitwise_or_expression([NotNull] centaurParser.Bitwise_or_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.logical_and_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLogical_and_expression([NotNull] centaurParser.Logical_and_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.logical_or_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLogical_or_expression([NotNull] centaurParser.Logical_or_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.unary_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnary_expression([NotNull] centaurParser.Unary_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.assignment_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignment_expression([NotNull] centaurParser.Assignment_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] centaurParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.primary_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrimary_expression([NotNull] centaurParser.Primary_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDeclaration([NotNull] centaurParser.DeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.compound_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompound_statement([NotNull] centaurParser.Compound_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement([NotNull] centaurParser.StatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.statement_or_declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement_or_declaration([NotNull] centaurParser.Statement_or_declarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.statement_list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement_list([NotNull] centaurParser.Statement_listContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.struct_field"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStruct_field([NotNull] centaurParser.Struct_fieldContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.struct_definition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStruct_definition([NotNull] centaurParser.Struct_definitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.function_definition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunction_definition([NotNull] centaurParser.Function_definitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.expression_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression_statement([NotNull] centaurParser.Expression_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.definition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDefinition([NotNull] centaurParser.DefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="centaurParser.definition_list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDefinition_list([NotNull] centaurParser.Definition_listContext context);
}