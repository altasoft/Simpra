//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from SimpraParser.g4 by ANTLR 4.13.2

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
/// by <see cref="SimpraParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
internal interface ISimpraParserVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="SimpraParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProgram([NotNull] SimpraParser.ProgramContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Identifier</c>
	/// labeled alternative in <see cref="SimpraParser.objectref"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIdentifier([NotNull] SimpraParser.IdentifierContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>MemberAccess</c>
	/// labeled alternative in <see cref="SimpraParser.objectref"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMemberAccess([NotNull] SimpraParser.MemberAccessContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>IndexAccess</c>
	/// labeled alternative in <see cref="SimpraParser.objectref"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIndexAccess([NotNull] SimpraParser.IndexAccessContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>FunctionCall</c>
	/// labeled alternative in <see cref="SimpraParser.objectref"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionCall([NotNull] SimpraParser.FunctionCallContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>BinaryAnd</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinaryAnd([NotNull] SimpraParser.BinaryAndContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Unary</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnary([NotNull] SimpraParser.UnaryContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ChainedComparison</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitChainedComparison([NotNull] SimpraParser.ChainedComparisonContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>CString</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCString([NotNull] SimpraParser.CStringContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>HasValue</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHasValue([NotNull] SimpraParser.HasValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>PascalString</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPascalString([NotNull] SimpraParser.PascalStringContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Array</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArray([NotNull] SimpraParser.ArrayContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Parenthesis</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthesis([NotNull] SimpraParser.ParenthesisContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>When</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhen([NotNull] SimpraParser.WhenContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Number</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumber([NotNull] SimpraParser.NumberContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Bool</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBool([NotNull] SimpraParser.BoolContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Comparison</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparison([NotNull] SimpraParser.ComparisonContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>BinaryOr</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinaryOr([NotNull] SimpraParser.BinaryOrContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Binary</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinary([NotNull] SimpraParser.BinaryContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ObjectRef</c>
	/// labeled alternative in <see cref="SimpraParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObjectRef([NotNull] SimpraParser.ObjectRefContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ExtraWhenExpr</c>
	/// labeled alternative in <see cref="SimpraParser.extraWhen"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExtraWhenExpr([NotNull] SimpraParser.ExtraWhenExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>StatementBlock</c>
	/// labeled alternative in <see cref="SimpraParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatementBlock([NotNull] SimpraParser.StatementBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>VariableDeclaration</c>
	/// labeled alternative in <see cref="SimpraParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableDeclaration([NotNull] SimpraParser.VariableDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Assignment</c>
	/// labeled alternative in <see cref="SimpraParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignment([NotNull] SimpraParser.AssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Return2</c>
	/// labeled alternative in <see cref="SimpraParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitReturn2([NotNull] SimpraParser.Return2Context context);
	/// <summary>
	/// Visit a parse tree produced by the <c>IfElseStatement</c>
	/// labeled alternative in <see cref="SimpraParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfElseStatement([NotNull] SimpraParser.IfElseStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>CompoundAssignment</c>
	/// labeled alternative in <see cref="SimpraParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompoundAssignment([NotNull] SimpraParser.CompoundAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>LineComment</c>
	/// labeled alternative in <see cref="SimpraParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLineComment([NotNull] SimpraParser.LineCommentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>BlockComment</c>
	/// labeled alternative in <see cref="SimpraParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlockComment([NotNull] SimpraParser.BlockCommentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Directive</c>
	/// labeled alternative in <see cref="SimpraParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDirective([NotNull] SimpraParser.DirectiveContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ExtraIfExpr</c>
	/// labeled alternative in <see cref="SimpraParser.extraIf"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExtraIfExpr([NotNull] SimpraParser.ExtraIfExprContext context);
}
