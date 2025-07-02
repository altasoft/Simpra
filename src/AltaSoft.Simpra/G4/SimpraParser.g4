// $antlr-format on
// $antlr-format allowShortBlocksOnASingleLine true, indentWidth 8

parser grammar SimpraParser;

options {
	tokenVocab = SimpraLexer;
}

@namespace { AltaSoft.Simpra.ANTLR }

// Root rule
program: Main = block EOF | Expr = exp EOF;

objectref:
	Identifier = IDENTIFIER										# Identifier
	| FunctionName = IDENTIFIER LPAREN exp? (COMMA exp)* RPAREN	# FunctionCall
	| Object = objectref (DOT PropertyName = IDENTIFIER)		# MemberAccess
	| Object = objectref (LBRACK Index = exp RBRACK)			# IndexAccess;

exp:
	BOOLEAN													# Bool
	| NUMBER												# Number
	| CSTRING												# CString
	| PASCALSTRING											# PascalString
	| LBRACK exp? (COMMA exp)* RBRACK						# Array
	| objectref												# ObjectRef
	| LPAREN Expression = exp RPAREN						# Parenthesis
	| Left = exp HAS VALUE									# HasValue
	| Operator = (NOT | MINUS | PLUS) Expression = exp		# Unary
	| Expression = exp Operator = PERCENT					# Unary
	| Left = exp Operator = (MULT | IDIV | DIV) Right = exp	# Binary
	| Left = exp Operator = (PLUS | MINUS) Right = exp		# Binary
	| Left = exp Operator = (MIN | MAX) Right = exp			# Binary
	| Expression = exp LeftOperator = (LT | GT | LE | GE) Left = exp (
		AND
		| OR
	) RightOperator = (LT | GT | LE | GE) Right = exp		# ChainedComparison
	| Left = exp Operator = (LT | GT | LE | GE) Right = exp	# Comparison
	| Left = exp Operator = (IS_NOT | IS) Right = exp		# Binary
	| Left = exp Operator = (
		NOT_IN
		| ANY_NOT_IN
		| ALL_NOT_IN
		| IN
		| ANY_IN
		| ALL_IN
		| LIKE
		| MATCHES
	) Right = exp					# Binary
	| Left = exp (AND) Right = exp	# BinaryAnd
	| Left = exp (OR) Right = exp	# BinaryOr
	| WHEN Condition = exp THEN ExpressionTrue = exp (extraWhen)* (
		ELSE ExpressionFalse = exp
	) END # When;

extraWhen:
	WHEN Condition = exp THEN ExpressionTrue = exp # ExtraWhenExpr;

block: statement* # StatementBlock;

statement:
	LET Name = IDENTIFIER EQ Right = exp	# VariableDeclaration
	| Left = objectref EQ Right = exp		# Assignment
	| RETURN Value = exp					# Return2
	| IF Condition = exp THEN BlockTrue = block (extraIf)* (
		ELSE BlockFalse = block
	)? END														# IfElseStatement
	| Left = exp Operator = (PLUS_EQ | MINUS_EQ) Right = exp	# CompoundAssignment 
	| COMMENT													# LineComment
	| BLOCK_COMMENT												# BlockComment
	| Name = DIRECTIVE OnOff = ON_OFF							# Directive;

extraIf:
	ELSE? IF Condition = exp THEN BlockTrue = block # ExtraIfExpr;
