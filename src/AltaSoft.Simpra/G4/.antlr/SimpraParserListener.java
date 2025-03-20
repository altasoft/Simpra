// Generated from d:/B7/B7.ISO20022/B7.ISO20022/Simpra/AltaSoft.Simpra/G4/SimpraParser.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link SimpraParser}.
 */
public interface SimpraParserListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link SimpraParser#program}.
	 * @param ctx the parse tree
	 */
	void enterProgram(SimpraParser.ProgramContext ctx);
	/**
	 * Exit a parse tree produced by {@link SimpraParser#program}.
	 * @param ctx the parse tree
	 */
	void exitProgram(SimpraParser.ProgramContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Identifier}
	 * labeled alternative in {@link SimpraParser#objectref}.
	 * @param ctx the parse tree
	 */
	void enterIdentifier(SimpraParser.IdentifierContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Identifier}
	 * labeled alternative in {@link SimpraParser#objectref}.
	 * @param ctx the parse tree
	 */
	void exitIdentifier(SimpraParser.IdentifierContext ctx);
	/**
	 * Enter a parse tree produced by the {@code MemberAccess}
	 * labeled alternative in {@link SimpraParser#objectref}.
	 * @param ctx the parse tree
	 */
	void enterMemberAccess(SimpraParser.MemberAccessContext ctx);
	/**
	 * Exit a parse tree produced by the {@code MemberAccess}
	 * labeled alternative in {@link SimpraParser#objectref}.
	 * @param ctx the parse tree
	 */
	void exitMemberAccess(SimpraParser.MemberAccessContext ctx);
	/**
	 * Enter a parse tree produced by the {@code IndexAccess}
	 * labeled alternative in {@link SimpraParser#objectref}.
	 * @param ctx the parse tree
	 */
	void enterIndexAccess(SimpraParser.IndexAccessContext ctx);
	/**
	 * Exit a parse tree produced by the {@code IndexAccess}
	 * labeled alternative in {@link SimpraParser#objectref}.
	 * @param ctx the parse tree
	 */
	void exitIndexAccess(SimpraParser.IndexAccessContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FunctionCall}
	 * labeled alternative in {@link SimpraParser#objectref}.
	 * @param ctx the parse tree
	 */
	void enterFunctionCall(SimpraParser.FunctionCallContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FunctionCall}
	 * labeled alternative in {@link SimpraParser#objectref}.
	 * @param ctx the parse tree
	 */
	void exitFunctionCall(SimpraParser.FunctionCallContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Null}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterNull(SimpraParser.NullContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Null}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitNull(SimpraParser.NullContext ctx);
	/**
	 * Enter a parse tree produced by the {@code BinaryAnd}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterBinaryAnd(SimpraParser.BinaryAndContext ctx);
	/**
	 * Exit a parse tree produced by the {@code BinaryAnd}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitBinaryAnd(SimpraParser.BinaryAndContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Unary}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterUnary(SimpraParser.UnaryContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Unary}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitUnary(SimpraParser.UnaryContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ChainedComparison}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterChainedComparison(SimpraParser.ChainedComparisonContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ChainedComparison}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitChainedComparison(SimpraParser.ChainedComparisonContext ctx);
	/**
	 * Enter a parse tree produced by the {@code CString}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterCString(SimpraParser.CStringContext ctx);
	/**
	 * Exit a parse tree produced by the {@code CString}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitCString(SimpraParser.CStringContext ctx);
	/**
	 * Enter a parse tree produced by the {@code PascalString}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterPascalString(SimpraParser.PascalStringContext ctx);
	/**
	 * Exit a parse tree produced by the {@code PascalString}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitPascalString(SimpraParser.PascalStringContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Array}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterArray(SimpraParser.ArrayContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Array}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitArray(SimpraParser.ArrayContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Parenthesis}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterParenthesis(SimpraParser.ParenthesisContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Parenthesis}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitParenthesis(SimpraParser.ParenthesisContext ctx);
	/**
	 * Enter a parse tree produced by the {@code When}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterWhen(SimpraParser.WhenContext ctx);
	/**
	 * Exit a parse tree produced by the {@code When}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitWhen(SimpraParser.WhenContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Number}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterNumber(SimpraParser.NumberContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Number}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitNumber(SimpraParser.NumberContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Bool}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterBool(SimpraParser.BoolContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Bool}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitBool(SimpraParser.BoolContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Comparison}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterComparison(SimpraParser.ComparisonContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Comparison}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitComparison(SimpraParser.ComparisonContext ctx);
	/**
	 * Enter a parse tree produced by the {@code BinaryOr}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterBinaryOr(SimpraParser.BinaryOrContext ctx);
	/**
	 * Exit a parse tree produced by the {@code BinaryOr}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitBinaryOr(SimpraParser.BinaryOrContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Binary}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterBinary(SimpraParser.BinaryContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Binary}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitBinary(SimpraParser.BinaryContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ObjectRef}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void enterObjectRef(SimpraParser.ObjectRefContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ObjectRef}
	 * labeled alternative in {@link SimpraParser#exp}.
	 * @param ctx the parse tree
	 */
	void exitObjectRef(SimpraParser.ObjectRefContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ExtraWhenExpr}
	 * labeled alternative in {@link SimpraParser#extraWhen}.
	 * @param ctx the parse tree
	 */
	void enterExtraWhenExpr(SimpraParser.ExtraWhenExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ExtraWhenExpr}
	 * labeled alternative in {@link SimpraParser#extraWhen}.
	 * @param ctx the parse tree
	 */
	void exitExtraWhenExpr(SimpraParser.ExtraWhenExprContext ctx);
	/**
	 * Enter a parse tree produced by the {@code StatementBlock}
	 * labeled alternative in {@link SimpraParser#block}.
	 * @param ctx the parse tree
	 */
	void enterStatementBlock(SimpraParser.StatementBlockContext ctx);
	/**
	 * Exit a parse tree produced by the {@code StatementBlock}
	 * labeled alternative in {@link SimpraParser#block}.
	 * @param ctx the parse tree
	 */
	void exitStatementBlock(SimpraParser.StatementBlockContext ctx);
	/**
	 * Enter a parse tree produced by the {@code VariableDeclaration}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterVariableDeclaration(SimpraParser.VariableDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code VariableDeclaration}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitVariableDeclaration(SimpraParser.VariableDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code HasValue}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterHasValue(SimpraParser.HasValueContext ctx);
	/**
	 * Exit a parse tree produced by the {@code HasValue}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitHasValue(SimpraParser.HasValueContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Assignment}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterAssignment(SimpraParser.AssignmentContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Assignment}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitAssignment(SimpraParser.AssignmentContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Return2}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterReturn2(SimpraParser.Return2Context ctx);
	/**
	 * Exit a parse tree produced by the {@code Return2}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitReturn2(SimpraParser.Return2Context ctx);
	/**
	 * Enter a parse tree produced by the {@code IfElseStatement}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterIfElseStatement(SimpraParser.IfElseStatementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code IfElseStatement}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitIfElseStatement(SimpraParser.IfElseStatementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code LineComment}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterLineComment(SimpraParser.LineCommentContext ctx);
	/**
	 * Exit a parse tree produced by the {@code LineComment}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitLineComment(SimpraParser.LineCommentContext ctx);
	/**
	 * Enter a parse tree produced by the {@code BlockComment}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterBlockComment(SimpraParser.BlockCommentContext ctx);
	/**
	 * Exit a parse tree produced by the {@code BlockComment}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitBlockComment(SimpraParser.BlockCommentContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Directive}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterDirective(SimpraParser.DirectiveContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Directive}
	 * labeled alternative in {@link SimpraParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitDirective(SimpraParser.DirectiveContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ExtraIfExpr}
	 * labeled alternative in {@link SimpraParser#extraIf}.
	 * @param ctx the parse tree
	 */
	void enterExtraIfExpr(SimpraParser.ExtraIfExprContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ExtraIfExpr}
	 * labeled alternative in {@link SimpraParser#extraIf}.
	 * @param ctx the parse tree
	 */
	void exitExtraIfExpr(SimpraParser.ExtraIfExprContext ctx);
}