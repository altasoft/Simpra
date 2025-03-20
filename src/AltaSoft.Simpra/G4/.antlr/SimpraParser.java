// Generated from d:/B7/B7.ISO20022/B7.ISO20022/Simpra/AltaSoft.Simpra/G4/SimpraParser.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue"})
public class SimpraParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.13.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		COMMENT=1, BLOCK_COMMENT=2, BOOLEAN=3, NUMBER=4, PASCALSTRING=5, CSTRING=6, 
		PERCENT=7, ON_OFF=8, NULL=9, WS=10, LET=11, WHEN=12, IF=13, THEN=14, ELSE=15, 
		END=16, RETURN=17, IS=18, IS_NOT=19, NOT_IN=20, IN=21, NOT=22, MIN=23, 
		MAX=24, MATCHES=25, LIKE=26, HAS=27, VALUE=28, AND=29, OR=30, EQ=31, PLUS=32, 
		MINUS=33, MULT=34, DIV=35, IDIV=36, LT=37, LE=38, GT=39, GE=40, LPAREN=41, 
		RPAREN=42, LBRACK=43, RBRACK=44, COMMA=45, DOT=46, IDENTIFIER=47, DIRECTIVE=48, 
		ERROR=49;
	public static final int
		RULE_program = 0, RULE_objectref = 1, RULE_exp = 2, RULE_extraWhen = 3, 
		RULE_block = 4, RULE_statement = 5, RULE_extraIf = 6;
	private static String[] makeRuleNames() {
		return new String[] {
			"program", "objectref", "exp", "extraWhen", "block", "statement", "extraIf"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, null, null, null, null, null, "'%'", null, "'null'", null, 
			"'let'", "'when'", "'if'", "'then'", "'else'", "'end'", "'return'", "'is'", 
			"'is not'", "'not in'", "'in'", "'not'", "'min'", "'max'", "'matches'", 
			"'like'", "'has'", "'value'", "'and'", "'or'", "'='", "'+'", "'-'", "'*'", 
			"'/'", "'//'", "'<'", "'<='", "'>'", "'>='", "'('", "')'", "'['", "']'", 
			"','", "'.'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "COMMENT", "BLOCK_COMMENT", "BOOLEAN", "NUMBER", "PASCALSTRING", 
			"CSTRING", "PERCENT", "ON_OFF", "NULL", "WS", "LET", "WHEN", "IF", "THEN", 
			"ELSE", "END", "RETURN", "IS", "IS_NOT", "NOT_IN", "IN", "NOT", "MIN", 
			"MAX", "MATCHES", "LIKE", "HAS", "VALUE", "AND", "OR", "EQ", "PLUS", 
			"MINUS", "MULT", "DIV", "IDIV", "LT", "LE", "GT", "GE", "LPAREN", "RPAREN", 
			"LBRACK", "RBRACK", "COMMA", "DOT", "IDENTIFIER", "DIRECTIVE", "ERROR"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "SimpraParser.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public SimpraParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ProgramContext extends ParserRuleContext {
		public BlockContext Main;
		public ExpContext Expr;
		public TerminalNode EOF() { return getToken(SimpraParser.EOF, 0); }
		public BlockContext block() {
			return getRuleContext(BlockContext.class,0);
		}
		public ExpContext exp() {
			return getRuleContext(ExpContext.class,0);
		}
		public ProgramContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_program; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterProgram(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitProgram(this);
		}
	}

	public final ProgramContext program() throws RecognitionException {
		ProgramContext _localctx = new ProgramContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_program);
		try {
			setState(18);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,0,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(14);
				((ProgramContext)_localctx).Main = block();
				setState(15);
				match(EOF);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(17);
				((ProgramContext)_localctx).Expr = exp(0);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ObjectrefContext extends ParserRuleContext {
		public ObjectrefContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_objectref; }
	 
		public ObjectrefContext() { }
		public void copyFrom(ObjectrefContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class IdentifierContext extends ObjectrefContext {
		public Token Identifier;
		public TerminalNode IDENTIFIER() { return getToken(SimpraParser.IDENTIFIER, 0); }
		public IdentifierContext(ObjectrefContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterIdentifier(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitIdentifier(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class MemberAccessContext extends ObjectrefContext {
		public ObjectrefContext Object;
		public Token PropertyName;
		public ObjectrefContext objectref() {
			return getRuleContext(ObjectrefContext.class,0);
		}
		public TerminalNode DOT() { return getToken(SimpraParser.DOT, 0); }
		public TerminalNode IDENTIFIER() { return getToken(SimpraParser.IDENTIFIER, 0); }
		public MemberAccessContext(ObjectrefContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterMemberAccess(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitMemberAccess(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class IndexAccessContext extends ObjectrefContext {
		public ObjectrefContext Object;
		public ExpContext Index;
		public ObjectrefContext objectref() {
			return getRuleContext(ObjectrefContext.class,0);
		}
		public TerminalNode LBRACK() { return getToken(SimpraParser.LBRACK, 0); }
		public TerminalNode RBRACK() { return getToken(SimpraParser.RBRACK, 0); }
		public ExpContext exp() {
			return getRuleContext(ExpContext.class,0);
		}
		public IndexAccessContext(ObjectrefContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterIndexAccess(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitIndexAccess(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class FunctionCallContext extends ObjectrefContext {
		public Token FunctionName;
		public TerminalNode LPAREN() { return getToken(SimpraParser.LPAREN, 0); }
		public TerminalNode RPAREN() { return getToken(SimpraParser.RPAREN, 0); }
		public TerminalNode IDENTIFIER() { return getToken(SimpraParser.IDENTIFIER, 0); }
		public List<ExpContext> exp() {
			return getRuleContexts(ExpContext.class);
		}
		public ExpContext exp(int i) {
			return getRuleContext(ExpContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(SimpraParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(SimpraParser.COMMA, i);
		}
		public FunctionCallContext(ObjectrefContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterFunctionCall(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitFunctionCall(this);
		}
	}

	public final ObjectrefContext objectref() throws RecognitionException {
		return objectref(0);
	}

	private ObjectrefContext objectref(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ObjectrefContext _localctx = new ObjectrefContext(_ctx, _parentState);
		ObjectrefContext _prevctx = _localctx;
		int _startState = 2;
		enterRecursionRule(_localctx, 2, RULE_objectref, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(35);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,3,_ctx) ) {
			case 1:
				{
				_localctx = new IdentifierContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;

				setState(21);
				((IdentifierContext)_localctx).Identifier = match(IDENTIFIER);
				}
				break;
			case 2:
				{
				_localctx = new FunctionCallContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(22);
				((FunctionCallContext)_localctx).FunctionName = match(IDENTIFIER);
				setState(23);
				match(LPAREN);
				setState(25);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 151741198766712L) != 0)) {
					{
					setState(24);
					exp(0);
					}
				}

				setState(31);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(27);
					match(COMMA);
					setState(28);
					exp(0);
					}
					}
					setState(33);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(34);
				match(RPAREN);
				}
				break;
			}
			_ctx.stop = _input.LT(-1);
			setState(47);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,5,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(45);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,4,_ctx) ) {
					case 1:
						{
						_localctx = new MemberAccessContext(new ObjectrefContext(_parentctx, _parentState));
						((MemberAccessContext)_localctx).Object = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_objectref);
						setState(37);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						{
						setState(38);
						match(DOT);
						setState(39);
						((MemberAccessContext)_localctx).PropertyName = match(IDENTIFIER);
						}
						}
						break;
					case 2:
						{
						_localctx = new IndexAccessContext(new ObjectrefContext(_parentctx, _parentState));
						((IndexAccessContext)_localctx).Object = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_objectref);
						setState(40);
						if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
						{
						setState(41);
						match(LBRACK);
						setState(42);
						((IndexAccessContext)_localctx).Index = exp(0);
						setState(43);
						match(RBRACK);
						}
						}
						break;
					}
					} 
				}
				setState(49);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,5,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ExpContext extends ParserRuleContext {
		public ExpContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_exp; }
	 
		public ExpContext() { }
		public void copyFrom(ExpContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class NullContext extends ExpContext {
		public TerminalNode NULL() { return getToken(SimpraParser.NULL, 0); }
		public NullContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterNull(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitNull(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class BinaryAndContext extends ExpContext {
		public ExpContext Left;
		public ExpContext Right;
		public List<ExpContext> exp() {
			return getRuleContexts(ExpContext.class);
		}
		public ExpContext exp(int i) {
			return getRuleContext(ExpContext.class,i);
		}
		public TerminalNode AND() { return getToken(SimpraParser.AND, 0); }
		public BinaryAndContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterBinaryAnd(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitBinaryAnd(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class UnaryContext extends ExpContext {
		public ExpContext Expression;
		public Token Operator;
		public ExpContext exp() {
			return getRuleContext(ExpContext.class,0);
		}
		public TerminalNode NOT() { return getToken(SimpraParser.NOT, 0); }
		public TerminalNode MINUS() { return getToken(SimpraParser.MINUS, 0); }
		public TerminalNode PERCENT() { return getToken(SimpraParser.PERCENT, 0); }
		public UnaryContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterUnary(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitUnary(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ChainedComparisonContext extends ExpContext {
		public ExpContext Expression;
		public Token LeftOperator;
		public ExpContext Left;
		public Token RightOperator;
		public ExpContext Right;
		public List<ExpContext> exp() {
			return getRuleContexts(ExpContext.class);
		}
		public ExpContext exp(int i) {
			return getRuleContext(ExpContext.class,i);
		}
		public TerminalNode AND() { return getToken(SimpraParser.AND, 0); }
		public TerminalNode OR() { return getToken(SimpraParser.OR, 0); }
		public List<TerminalNode> LT() { return getTokens(SimpraParser.LT); }
		public TerminalNode LT(int i) {
			return getToken(SimpraParser.LT, i);
		}
		public List<TerminalNode> GT() { return getTokens(SimpraParser.GT); }
		public TerminalNode GT(int i) {
			return getToken(SimpraParser.GT, i);
		}
		public List<TerminalNode> LE() { return getTokens(SimpraParser.LE); }
		public TerminalNode LE(int i) {
			return getToken(SimpraParser.LE, i);
		}
		public List<TerminalNode> GE() { return getTokens(SimpraParser.GE); }
		public TerminalNode GE(int i) {
			return getToken(SimpraParser.GE, i);
		}
		public ChainedComparisonContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterChainedComparison(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitChainedComparison(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class CStringContext extends ExpContext {
		public TerminalNode CSTRING() { return getToken(SimpraParser.CSTRING, 0); }
		public CStringContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterCString(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitCString(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class PascalStringContext extends ExpContext {
		public TerminalNode PASCALSTRING() { return getToken(SimpraParser.PASCALSTRING, 0); }
		public PascalStringContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterPascalString(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitPascalString(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ArrayContext extends ExpContext {
		public TerminalNode LBRACK() { return getToken(SimpraParser.LBRACK, 0); }
		public TerminalNode RBRACK() { return getToken(SimpraParser.RBRACK, 0); }
		public List<ExpContext> exp() {
			return getRuleContexts(ExpContext.class);
		}
		public ExpContext exp(int i) {
			return getRuleContext(ExpContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(SimpraParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(SimpraParser.COMMA, i);
		}
		public ArrayContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterArray(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitArray(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ParenthesisContext extends ExpContext {
		public ExpContext Expression;
		public TerminalNode LPAREN() { return getToken(SimpraParser.LPAREN, 0); }
		public TerminalNode RPAREN() { return getToken(SimpraParser.RPAREN, 0); }
		public ExpContext exp() {
			return getRuleContext(ExpContext.class,0);
		}
		public ParenthesisContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterParenthesis(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitParenthesis(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class WhenContext extends ExpContext {
		public ExpContext Condition;
		public ExpContext ExpressionTrue;
		public ExpContext ExpressionFalse;
		public TerminalNode WHEN() { return getToken(SimpraParser.WHEN, 0); }
		public TerminalNode THEN() { return getToken(SimpraParser.THEN, 0); }
		public TerminalNode END() { return getToken(SimpraParser.END, 0); }
		public List<ExpContext> exp() {
			return getRuleContexts(ExpContext.class);
		}
		public ExpContext exp(int i) {
			return getRuleContext(ExpContext.class,i);
		}
		public TerminalNode ELSE() { return getToken(SimpraParser.ELSE, 0); }
		public List<ExtraWhenContext> extraWhen() {
			return getRuleContexts(ExtraWhenContext.class);
		}
		public ExtraWhenContext extraWhen(int i) {
			return getRuleContext(ExtraWhenContext.class,i);
		}
		public WhenContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterWhen(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitWhen(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class NumberContext extends ExpContext {
		public TerminalNode NUMBER() { return getToken(SimpraParser.NUMBER, 0); }
		public NumberContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterNumber(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitNumber(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class BoolContext extends ExpContext {
		public TerminalNode BOOLEAN() { return getToken(SimpraParser.BOOLEAN, 0); }
		public BoolContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterBool(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitBool(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ComparisonContext extends ExpContext {
		public ExpContext Left;
		public Token Operator;
		public ExpContext Right;
		public List<ExpContext> exp() {
			return getRuleContexts(ExpContext.class);
		}
		public ExpContext exp(int i) {
			return getRuleContext(ExpContext.class,i);
		}
		public TerminalNode LT() { return getToken(SimpraParser.LT, 0); }
		public TerminalNode GT() { return getToken(SimpraParser.GT, 0); }
		public TerminalNode LE() { return getToken(SimpraParser.LE, 0); }
		public TerminalNode GE() { return getToken(SimpraParser.GE, 0); }
		public ComparisonContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterComparison(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitComparison(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class BinaryOrContext extends ExpContext {
		public ExpContext Left;
		public ExpContext Right;
		public List<ExpContext> exp() {
			return getRuleContexts(ExpContext.class);
		}
		public ExpContext exp(int i) {
			return getRuleContext(ExpContext.class,i);
		}
		public TerminalNode OR() { return getToken(SimpraParser.OR, 0); }
		public BinaryOrContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterBinaryOr(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitBinaryOr(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class BinaryContext extends ExpContext {
		public ExpContext Left;
		public Token Operator;
		public ExpContext Right;
		public List<ExpContext> exp() {
			return getRuleContexts(ExpContext.class);
		}
		public ExpContext exp(int i) {
			return getRuleContext(ExpContext.class,i);
		}
		public TerminalNode MULT() { return getToken(SimpraParser.MULT, 0); }
		public TerminalNode IDIV() { return getToken(SimpraParser.IDIV, 0); }
		public TerminalNode DIV() { return getToken(SimpraParser.DIV, 0); }
		public TerminalNode PLUS() { return getToken(SimpraParser.PLUS, 0); }
		public TerminalNode MINUS() { return getToken(SimpraParser.MINUS, 0); }
		public TerminalNode MIN() { return getToken(SimpraParser.MIN, 0); }
		public TerminalNode MAX() { return getToken(SimpraParser.MAX, 0); }
		public TerminalNode IS_NOT() { return getToken(SimpraParser.IS_NOT, 0); }
		public TerminalNode IS() { return getToken(SimpraParser.IS, 0); }
		public TerminalNode NOT_IN() { return getToken(SimpraParser.NOT_IN, 0); }
		public TerminalNode IN() { return getToken(SimpraParser.IN, 0); }
		public TerminalNode LIKE() { return getToken(SimpraParser.LIKE, 0); }
		public TerminalNode MATCHES() { return getToken(SimpraParser.MATCHES, 0); }
		public BinaryContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterBinary(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitBinary(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ObjectRefContext extends ExpContext {
		public ObjectrefContext objectref() {
			return getRuleContext(ObjectrefContext.class,0);
		}
		public ObjectRefContext(ExpContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterObjectRef(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitObjectRef(this);
		}
	}

	public final ExpContext exp() throws RecognitionException {
		return exp(0);
	}

	private ExpContext exp(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ExpContext _localctx = new ExpContext(_ctx, _parentState);
		ExpContext _prevctx = _localctx;
		int _startState = 4;
		enterRecursionRule(_localctx, 4, RULE_exp, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(90);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case NULL:
				{
				_localctx = new NullContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;

				setState(51);
				match(NULL);
				}
				break;
			case BOOLEAN:
				{
				_localctx = new BoolContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(52);
				match(BOOLEAN);
				}
				break;
			case NUMBER:
				{
				_localctx = new NumberContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(53);
				match(NUMBER);
				}
				break;
			case CSTRING:
				{
				_localctx = new CStringContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(54);
				match(CSTRING);
				}
				break;
			case PASCALSTRING:
				{
				_localctx = new PascalStringContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(55);
				match(PASCALSTRING);
				}
				break;
			case LBRACK:
				{
				_localctx = new ArrayContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(56);
				match(LBRACK);
				setState(58);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 151741198766712L) != 0)) {
					{
					setState(57);
					exp(0);
					}
				}

				setState(64);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(60);
					match(COMMA);
					setState(61);
					exp(0);
					}
					}
					setState(66);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(67);
				match(RBRACK);
				}
				break;
			case IDENTIFIER:
				{
				_localctx = new ObjectRefContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(68);
				objectref(0);
				}
				break;
			case LPAREN:
				{
				_localctx = new ParenthesisContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(69);
				match(LPAREN);
				setState(70);
				((ParenthesisContext)_localctx).Expression = exp(0);
				setState(71);
				match(RPAREN);
				}
				break;
			case NOT:
			case MINUS:
				{
				_localctx = new UnaryContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(73);
				((UnaryContext)_localctx).Operator = _input.LT(1);
				_la = _input.LA(1);
				if ( !(_la==NOT || _la==MINUS) ) {
					((UnaryContext)_localctx).Operator = (Token)_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(74);
				((UnaryContext)_localctx).Expression = exp(12);
				}
				break;
			case WHEN:
				{
				_localctx = new WhenContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(75);
				match(WHEN);
				setState(76);
				((WhenContext)_localctx).Condition = exp(0);
				setState(77);
				match(THEN);
				setState(78);
				((WhenContext)_localctx).ExpressionTrue = exp(0);
				setState(82);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==WHEN) {
					{
					{
					setState(79);
					extraWhen();
					}
					}
					setState(84);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				{
				setState(85);
				match(ELSE);
				setState(86);
				((WhenContext)_localctx).ExpressionFalse = exp(0);
				}
				setState(88);
				match(END);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			_ctx.stop = _input.LT(-1);
			setState(127);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,11,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(125);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,10,_ctx) ) {
					case 1:
						{
						_localctx = new BinaryContext(new ExpContext(_parentctx, _parentState));
						((BinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_exp);
						setState(92);
						if (!(precpred(_ctx, 10))) throw new FailedPredicateException(this, "precpred(_ctx, 10)");
						setState(93);
						((BinaryContext)_localctx).Operator = _input.LT(1);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 120259084288L) != 0)) ) {
							((BinaryContext)_localctx).Operator = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(94);
						((BinaryContext)_localctx).Right = exp(11);
						}
						break;
					case 2:
						{
						_localctx = new BinaryContext(new ExpContext(_parentctx, _parentState));
						((BinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_exp);
						setState(95);
						if (!(precpred(_ctx, 9))) throw new FailedPredicateException(this, "precpred(_ctx, 9)");
						setState(96);
						((BinaryContext)_localctx).Operator = _input.LT(1);
						_la = _input.LA(1);
						if ( !(_la==PLUS || _la==MINUS) ) {
							((BinaryContext)_localctx).Operator = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(97);
						((BinaryContext)_localctx).Right = exp(10);
						}
						break;
					case 3:
						{
						_localctx = new BinaryContext(new ExpContext(_parentctx, _parentState));
						((BinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_exp);
						setState(98);
						if (!(precpred(_ctx, 8))) throw new FailedPredicateException(this, "precpred(_ctx, 8)");
						setState(99);
						((BinaryContext)_localctx).Operator = _input.LT(1);
						_la = _input.LA(1);
						if ( !(_la==MIN || _la==MAX) ) {
							((BinaryContext)_localctx).Operator = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(100);
						((BinaryContext)_localctx).Right = exp(9);
						}
						break;
					case 4:
						{
						_localctx = new ChainedComparisonContext(new ExpContext(_parentctx, _parentState));
						((ChainedComparisonContext)_localctx).Expression = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_exp);
						setState(101);
						if (!(precpred(_ctx, 7))) throw new FailedPredicateException(this, "precpred(_ctx, 7)");
						setState(102);
						((ChainedComparisonContext)_localctx).LeftOperator = _input.LT(1);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 2061584302080L) != 0)) ) {
							((ChainedComparisonContext)_localctx).LeftOperator = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(103);
						((ChainedComparisonContext)_localctx).Left = exp(0);
						setState(104);
						_la = _input.LA(1);
						if ( !(_la==AND || _la==OR) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(105);
						((ChainedComparisonContext)_localctx).RightOperator = _input.LT(1);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 2061584302080L) != 0)) ) {
							((ChainedComparisonContext)_localctx).RightOperator = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(106);
						((ChainedComparisonContext)_localctx).Right = exp(8);
						}
						break;
					case 5:
						{
						_localctx = new ComparisonContext(new ExpContext(_parentctx, _parentState));
						((ComparisonContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_exp);
						setState(108);
						if (!(precpred(_ctx, 6))) throw new FailedPredicateException(this, "precpred(_ctx, 6)");
						setState(109);
						((ComparisonContext)_localctx).Operator = _input.LT(1);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 2061584302080L) != 0)) ) {
							((ComparisonContext)_localctx).Operator = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(110);
						((ComparisonContext)_localctx).Right = exp(7);
						}
						break;
					case 6:
						{
						_localctx = new BinaryContext(new ExpContext(_parentctx, _parentState));
						((BinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_exp);
						setState(111);
						if (!(precpred(_ctx, 5))) throw new FailedPredicateException(this, "precpred(_ctx, 5)");
						setState(112);
						((BinaryContext)_localctx).Operator = _input.LT(1);
						_la = _input.LA(1);
						if ( !(_la==IS || _la==IS_NOT) ) {
							((BinaryContext)_localctx).Operator = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(113);
						((BinaryContext)_localctx).Right = exp(6);
						}
						break;
					case 7:
						{
						_localctx = new BinaryContext(new ExpContext(_parentctx, _parentState));
						((BinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_exp);
						setState(114);
						if (!(precpred(_ctx, 4))) throw new FailedPredicateException(this, "precpred(_ctx, 4)");
						setState(115);
						((BinaryContext)_localctx).Operator = _input.LT(1);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 103809024L) != 0)) ) {
							((BinaryContext)_localctx).Operator = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(116);
						((BinaryContext)_localctx).Right = exp(5);
						}
						break;
					case 8:
						{
						_localctx = new BinaryAndContext(new ExpContext(_parentctx, _parentState));
						((BinaryAndContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_exp);
						setState(117);
						if (!(precpred(_ctx, 3))) throw new FailedPredicateException(this, "precpred(_ctx, 3)");
						{
						setState(118);
						match(AND);
						}
						setState(119);
						((BinaryAndContext)_localctx).Right = exp(4);
						}
						break;
					case 9:
						{
						_localctx = new BinaryOrContext(new ExpContext(_parentctx, _parentState));
						((BinaryOrContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_exp);
						setState(120);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						{
						setState(121);
						match(OR);
						}
						setState(122);
						((BinaryOrContext)_localctx).Right = exp(3);
						}
						break;
					case 10:
						{
						_localctx = new UnaryContext(new ExpContext(_parentctx, _parentState));
						((UnaryContext)_localctx).Expression = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_exp);
						setState(123);
						if (!(precpred(_ctx, 11))) throw new FailedPredicateException(this, "precpred(_ctx, 11)");
						setState(124);
						((UnaryContext)_localctx).Operator = match(PERCENT);
						}
						break;
					}
					} 
				}
				setState(129);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,11,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ExtraWhenContext extends ParserRuleContext {
		public ExtraWhenContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_extraWhen; }
	 
		public ExtraWhenContext() { }
		public void copyFrom(ExtraWhenContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ExtraWhenExprContext extends ExtraWhenContext {
		public ExpContext Condition;
		public ExpContext ExpressionTrue;
		public TerminalNode WHEN() { return getToken(SimpraParser.WHEN, 0); }
		public TerminalNode THEN() { return getToken(SimpraParser.THEN, 0); }
		public List<ExpContext> exp() {
			return getRuleContexts(ExpContext.class);
		}
		public ExpContext exp(int i) {
			return getRuleContext(ExpContext.class,i);
		}
		public ExtraWhenExprContext(ExtraWhenContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterExtraWhenExpr(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitExtraWhenExpr(this);
		}
	}

	public final ExtraWhenContext extraWhen() throws RecognitionException {
		ExtraWhenContext _localctx = new ExtraWhenContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_extraWhen);
		try {
			_localctx = new ExtraWhenExprContext(_localctx);
			enterOuterAlt(_localctx, 1);
			{
			setState(130);
			match(WHEN);
			setState(131);
			((ExtraWhenExprContext)_localctx).Condition = exp(0);
			setState(132);
			match(THEN);
			setState(133);
			((ExtraWhenExprContext)_localctx).ExpressionTrue = exp(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BlockContext extends ParserRuleContext {
		public BlockContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_block; }
	 
		public BlockContext() { }
		public void copyFrom(BlockContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class StatementBlockContext extends BlockContext {
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public StatementBlockContext(BlockContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterStatementBlock(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitStatementBlock(this);
		}
	}

	public final BlockContext block() throws RecognitionException {
		BlockContext _localctx = new BlockContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_block);
		try {
			int _alt;
			_localctx = new StatementBlockContext(_localctx);
			enterOuterAlt(_localctx, 1);
			{
			setState(138);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,12,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(135);
					statement();
					}
					} 
				}
				setState(140);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,12,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StatementContext extends ParserRuleContext {
		public StatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement; }
	 
		public StatementContext() { }
		public void copyFrom(StatementContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class AssignmentContext extends StatementContext {
		public ObjectrefContext Left;
		public ExpContext Right;
		public TerminalNode EQ() { return getToken(SimpraParser.EQ, 0); }
		public ObjectrefContext objectref() {
			return getRuleContext(ObjectrefContext.class,0);
		}
		public ExpContext exp() {
			return getRuleContext(ExpContext.class,0);
		}
		public AssignmentContext(StatementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterAssignment(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitAssignment(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class Return2Context extends StatementContext {
		public ExpContext Value;
		public TerminalNode RETURN() { return getToken(SimpraParser.RETURN, 0); }
		public ExpContext exp() {
			return getRuleContext(ExpContext.class,0);
		}
		public Return2Context(StatementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterReturn2(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitReturn2(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class BlockCommentContext extends StatementContext {
		public TerminalNode BLOCK_COMMENT() { return getToken(SimpraParser.BLOCK_COMMENT, 0); }
		public BlockCommentContext(StatementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterBlockComment(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitBlockComment(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class IfElseStatementContext extends StatementContext {
		public ExpContext Condition;
		public BlockContext BlockTrue;
		public BlockContext BlockFalse;
		public TerminalNode IF() { return getToken(SimpraParser.IF, 0); }
		public TerminalNode THEN() { return getToken(SimpraParser.THEN, 0); }
		public TerminalNode END() { return getToken(SimpraParser.END, 0); }
		public ExpContext exp() {
			return getRuleContext(ExpContext.class,0);
		}
		public List<BlockContext> block() {
			return getRuleContexts(BlockContext.class);
		}
		public BlockContext block(int i) {
			return getRuleContext(BlockContext.class,i);
		}
		public List<ExtraIfContext> extraIf() {
			return getRuleContexts(ExtraIfContext.class);
		}
		public ExtraIfContext extraIf(int i) {
			return getRuleContext(ExtraIfContext.class,i);
		}
		public TerminalNode ELSE() { return getToken(SimpraParser.ELSE, 0); }
		public IfElseStatementContext(StatementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterIfElseStatement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitIfElseStatement(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class VariableDeclarationContext extends StatementContext {
		public Token Name;
		public ExpContext Right;
		public TerminalNode LET() { return getToken(SimpraParser.LET, 0); }
		public TerminalNode EQ() { return getToken(SimpraParser.EQ, 0); }
		public TerminalNode IDENTIFIER() { return getToken(SimpraParser.IDENTIFIER, 0); }
		public ExpContext exp() {
			return getRuleContext(ExpContext.class,0);
		}
		public VariableDeclarationContext(StatementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterVariableDeclaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitVariableDeclaration(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class LineCommentContext extends StatementContext {
		public TerminalNode COMMENT() { return getToken(SimpraParser.COMMENT, 0); }
		public LineCommentContext(StatementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterLineComment(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitLineComment(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class DirectiveContext extends StatementContext {
		public Token Name;
		public Token OnOff;
		public TerminalNode DIRECTIVE() { return getToken(SimpraParser.DIRECTIVE, 0); }
		public TerminalNode ON_OFF() { return getToken(SimpraParser.ON_OFF, 0); }
		public DirectiveContext(StatementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterDirective(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitDirective(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class HasValueContext extends StatementContext {
		public ExpContext Left;
		public TerminalNode HAS() { return getToken(SimpraParser.HAS, 0); }
		public TerminalNode VALUE() { return getToken(SimpraParser.VALUE, 0); }
		public ExpContext exp() {
			return getRuleContext(ExpContext.class,0);
		}
		public HasValueContext(StatementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterHasValue(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitHasValue(this);
		}
	}

	public final StatementContext statement() throws RecognitionException {
		StatementContext _localctx = new StatementContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_statement);
		int _la;
		try {
			int _alt;
			setState(175);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,15,_ctx) ) {
			case 1:
				_localctx = new VariableDeclarationContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(141);
				match(LET);
				setState(142);
				((VariableDeclarationContext)_localctx).Name = match(IDENTIFIER);
				setState(143);
				match(EQ);
				setState(144);
				((VariableDeclarationContext)_localctx).Right = exp(0);
				}
				break;
			case 2:
				_localctx = new HasValueContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(145);
				((HasValueContext)_localctx).Left = exp(0);
				setState(146);
				match(HAS);
				setState(147);
				match(VALUE);
				}
				break;
			case 3:
				_localctx = new AssignmentContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(149);
				((AssignmentContext)_localctx).Left = objectref(0);
				setState(150);
				match(EQ);
				setState(151);
				((AssignmentContext)_localctx).Right = exp(0);
				}
				break;
			case 4:
				_localctx = new Return2Context(_localctx);
				enterOuterAlt(_localctx, 4);
				{
				setState(153);
				match(RETURN);
				setState(154);
				((Return2Context)_localctx).Value = exp(0);
				}
				break;
			case 5:
				_localctx = new IfElseStatementContext(_localctx);
				enterOuterAlt(_localctx, 5);
				{
				setState(155);
				match(IF);
				setState(156);
				((IfElseStatementContext)_localctx).Condition = exp(0);
				setState(157);
				match(THEN);
				setState(158);
				((IfElseStatementContext)_localctx).BlockTrue = block();
				setState(162);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,13,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(159);
						extraIf();
						}
						} 
					}
					setState(164);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,13,_ctx);
				}
				setState(167);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==ELSE) {
					{
					setState(165);
					match(ELSE);
					setState(166);
					((IfElseStatementContext)_localctx).BlockFalse = block();
					}
				}

				setState(169);
				match(END);
				}
				break;
			case 6:
				_localctx = new LineCommentContext(_localctx);
				enterOuterAlt(_localctx, 6);
				{
				setState(171);
				match(COMMENT);
				}
				break;
			case 7:
				_localctx = new BlockCommentContext(_localctx);
				enterOuterAlt(_localctx, 7);
				{
				setState(172);
				match(BLOCK_COMMENT);
				}
				break;
			case 8:
				_localctx = new DirectiveContext(_localctx);
				enterOuterAlt(_localctx, 8);
				{
				setState(173);
				((DirectiveContext)_localctx).Name = match(DIRECTIVE);
				setState(174);
				((DirectiveContext)_localctx).OnOff = match(ON_OFF);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ExtraIfContext extends ParserRuleContext {
		public ExtraIfContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_extraIf; }
	 
		public ExtraIfContext() { }
		public void copyFrom(ExtraIfContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ExtraIfExprContext extends ExtraIfContext {
		public ExpContext Condition;
		public BlockContext BlockTrue;
		public TerminalNode IF() { return getToken(SimpraParser.IF, 0); }
		public TerminalNode THEN() { return getToken(SimpraParser.THEN, 0); }
		public ExpContext exp() {
			return getRuleContext(ExpContext.class,0);
		}
		public BlockContext block() {
			return getRuleContext(BlockContext.class,0);
		}
		public TerminalNode ELSE() { return getToken(SimpraParser.ELSE, 0); }
		public ExtraIfExprContext(ExtraIfContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).enterExtraIfExpr(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof SimpraParserListener ) ((SimpraParserListener)listener).exitExtraIfExpr(this);
		}
	}

	public final ExtraIfContext extraIf() throws RecognitionException {
		ExtraIfContext _localctx = new ExtraIfContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_extraIf);
		int _la;
		try {
			_localctx = new ExtraIfExprContext(_localctx);
			enterOuterAlt(_localctx, 1);
			{
			setState(178);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ELSE) {
				{
				setState(177);
				match(ELSE);
				}
			}

			setState(180);
			match(IF);
			setState(181);
			((ExtraIfExprContext)_localctx).Condition = exp(0);
			setState(182);
			match(THEN);
			setState(183);
			((ExtraIfExprContext)_localctx).BlockTrue = block();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 1:
			return objectref_sempred((ObjectrefContext)_localctx, predIndex);
		case 2:
			return exp_sempred((ExpContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean objectref_sempred(ObjectrefContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 2);
		case 1:
			return precpred(_ctx, 1);
		}
		return true;
	}
	private boolean exp_sempred(ExpContext _localctx, int predIndex) {
		switch (predIndex) {
		case 2:
			return precpred(_ctx, 10);
		case 3:
			return precpred(_ctx, 9);
		case 4:
			return precpred(_ctx, 8);
		case 5:
			return precpred(_ctx, 7);
		case 6:
			return precpred(_ctx, 6);
		case 7:
			return precpred(_ctx, 5);
		case 8:
			return precpred(_ctx, 4);
		case 9:
			return precpred(_ctx, 3);
		case 10:
			return precpred(_ctx, 2);
		case 11:
			return precpred(_ctx, 11);
		}
		return true;
	}

	public static final String _serializedATN =
		"\u0004\u00011\u00ba\u0002\u0000\u0007\u0000\u0002\u0001\u0007\u0001\u0002"+
		"\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004\u0007\u0004\u0002"+
		"\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0001\u0000\u0001\u0000\u0001"+
		"\u0000\u0001\u0000\u0003\u0000\u0013\b\u0000\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0003\u0001\u001a\b\u0001\u0001\u0001\u0001"+
		"\u0001\u0005\u0001\u001e\b\u0001\n\u0001\f\u0001!\t\u0001\u0001\u0001"+
		"\u0003\u0001$\b\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0005\u0001.\b\u0001"+
		"\n\u0001\f\u00011\t\u0001\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0003\u0002;\b\u0002"+
		"\u0001\u0002\u0001\u0002\u0005\u0002?\b\u0002\n\u0002\f\u0002B\t\u0002"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002"+
		"\u0001\u0002\u0005\u0002Q\b\u0002\n\u0002\f\u0002T\t\u0002\u0001\u0002"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0003\u0002[\b\u0002"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0005\u0002~\b\u0002\n\u0002\f\u0002"+
		"\u0081\t\u0002\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0003"+
		"\u0001\u0004\u0005\u0004\u0089\b\u0004\n\u0004\f\u0004\u008c\t\u0004\u0001"+
		"\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001"+
		"\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001"+
		"\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001"+
		"\u0005\u0005\u0005\u00a1\b\u0005\n\u0005\f\u0005\u00a4\t\u0005\u0001\u0005"+
		"\u0001\u0005\u0003\u0005\u00a8\b\u0005\u0001\u0005\u0001\u0005\u0001\u0005"+
		"\u0001\u0005\u0001\u0005\u0001\u0005\u0003\u0005\u00b0\b\u0005\u0001\u0006"+
		"\u0003\u0006\u00b3\b\u0006\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006"+
		"\u0001\u0006\u0001\u0006\u0000\u0002\u0002\u0004\u0007\u0000\u0002\u0004"+
		"\u0006\b\n\f\u0000\b\u0002\u0000\u0016\u0016!!\u0001\u0000\"$\u0001\u0000"+
		" !\u0001\u0000\u0017\u0018\u0001\u0000%(\u0001\u0000\u001d\u001e\u0001"+
		"\u0000\u0012\u0013\u0002\u0000\u0014\u0015\u0019\u001a\u00d9\u0000\u0012"+
		"\u0001\u0000\u0000\u0000\u0002#\u0001\u0000\u0000\u0000\u0004Z\u0001\u0000"+
		"\u0000\u0000\u0006\u0082\u0001\u0000\u0000\u0000\b\u008a\u0001\u0000\u0000"+
		"\u0000\n\u00af\u0001\u0000\u0000\u0000\f\u00b2\u0001\u0000\u0000\u0000"+
		"\u000e\u000f\u0003\b\u0004\u0000\u000f\u0010\u0005\u0000\u0000\u0001\u0010"+
		"\u0013\u0001\u0000\u0000\u0000\u0011\u0013\u0003\u0004\u0002\u0000\u0012"+
		"\u000e\u0001\u0000\u0000\u0000\u0012\u0011\u0001\u0000\u0000\u0000\u0013"+
		"\u0001\u0001\u0000\u0000\u0000\u0014\u0015\u0006\u0001\uffff\uffff\u0000"+
		"\u0015$\u0005/\u0000\u0000\u0016\u0017\u0005/\u0000\u0000\u0017\u0019"+
		"\u0005)\u0000\u0000\u0018\u001a\u0003\u0004\u0002\u0000\u0019\u0018\u0001"+
		"\u0000\u0000\u0000\u0019\u001a\u0001\u0000\u0000\u0000\u001a\u001f\u0001"+
		"\u0000\u0000\u0000\u001b\u001c\u0005-\u0000\u0000\u001c\u001e\u0003\u0004"+
		"\u0002\u0000\u001d\u001b\u0001\u0000\u0000\u0000\u001e!\u0001\u0000\u0000"+
		"\u0000\u001f\u001d\u0001\u0000\u0000\u0000\u001f \u0001\u0000\u0000\u0000"+
		" \"\u0001\u0000\u0000\u0000!\u001f\u0001\u0000\u0000\u0000\"$\u0005*\u0000"+
		"\u0000#\u0014\u0001\u0000\u0000\u0000#\u0016\u0001\u0000\u0000\u0000$"+
		"/\u0001\u0000\u0000\u0000%&\n\u0002\u0000\u0000&\'\u0005.\u0000\u0000"+
		"\'.\u0005/\u0000\u0000()\n\u0001\u0000\u0000)*\u0005+\u0000\u0000*+\u0003"+
		"\u0004\u0002\u0000+,\u0005,\u0000\u0000,.\u0001\u0000\u0000\u0000-%\u0001"+
		"\u0000\u0000\u0000-(\u0001\u0000\u0000\u0000.1\u0001\u0000\u0000\u0000"+
		"/-\u0001\u0000\u0000\u0000/0\u0001\u0000\u0000\u00000\u0003\u0001\u0000"+
		"\u0000\u00001/\u0001\u0000\u0000\u000023\u0006\u0002\uffff\uffff\u0000"+
		"3[\u0005\t\u0000\u00004[\u0005\u0003\u0000\u00005[\u0005\u0004\u0000\u0000"+
		"6[\u0005\u0006\u0000\u00007[\u0005\u0005\u0000\u00008:\u0005+\u0000\u0000"+
		"9;\u0003\u0004\u0002\u0000:9\u0001\u0000\u0000\u0000:;\u0001\u0000\u0000"+
		"\u0000;@\u0001\u0000\u0000\u0000<=\u0005-\u0000\u0000=?\u0003\u0004\u0002"+
		"\u0000><\u0001\u0000\u0000\u0000?B\u0001\u0000\u0000\u0000@>\u0001\u0000"+
		"\u0000\u0000@A\u0001\u0000\u0000\u0000AC\u0001\u0000\u0000\u0000B@\u0001"+
		"\u0000\u0000\u0000C[\u0005,\u0000\u0000D[\u0003\u0002\u0001\u0000EF\u0005"+
		")\u0000\u0000FG\u0003\u0004\u0002\u0000GH\u0005*\u0000\u0000H[\u0001\u0000"+
		"\u0000\u0000IJ\u0007\u0000\u0000\u0000J[\u0003\u0004\u0002\fKL\u0005\f"+
		"\u0000\u0000LM\u0003\u0004\u0002\u0000MN\u0005\u000e\u0000\u0000NR\u0003"+
		"\u0004\u0002\u0000OQ\u0003\u0006\u0003\u0000PO\u0001\u0000\u0000\u0000"+
		"QT\u0001\u0000\u0000\u0000RP\u0001\u0000\u0000\u0000RS\u0001\u0000\u0000"+
		"\u0000SU\u0001\u0000\u0000\u0000TR\u0001\u0000\u0000\u0000UV\u0005\u000f"+
		"\u0000\u0000VW\u0003\u0004\u0002\u0000WX\u0001\u0000\u0000\u0000XY\u0005"+
		"\u0010\u0000\u0000Y[\u0001\u0000\u0000\u0000Z2\u0001\u0000\u0000\u0000"+
		"Z4\u0001\u0000\u0000\u0000Z5\u0001\u0000\u0000\u0000Z6\u0001\u0000\u0000"+
		"\u0000Z7\u0001\u0000\u0000\u0000Z8\u0001\u0000\u0000\u0000ZD\u0001\u0000"+
		"\u0000\u0000ZE\u0001\u0000\u0000\u0000ZI\u0001\u0000\u0000\u0000ZK\u0001"+
		"\u0000\u0000\u0000[\u007f\u0001\u0000\u0000\u0000\\]\n\n\u0000\u0000]"+
		"^\u0007\u0001\u0000\u0000^~\u0003\u0004\u0002\u000b_`\n\t\u0000\u0000"+
		"`a\u0007\u0002\u0000\u0000a~\u0003\u0004\u0002\nbc\n\b\u0000\u0000cd\u0007"+
		"\u0003\u0000\u0000d~\u0003\u0004\u0002\tef\n\u0007\u0000\u0000fg\u0007"+
		"\u0004\u0000\u0000gh\u0003\u0004\u0002\u0000hi\u0007\u0005\u0000\u0000"+
		"ij\u0007\u0004\u0000\u0000jk\u0003\u0004\u0002\bk~\u0001\u0000\u0000\u0000"+
		"lm\n\u0006\u0000\u0000mn\u0007\u0004\u0000\u0000n~\u0003\u0004\u0002\u0007"+
		"op\n\u0005\u0000\u0000pq\u0007\u0006\u0000\u0000q~\u0003\u0004\u0002\u0006"+
		"rs\n\u0004\u0000\u0000st\u0007\u0007\u0000\u0000t~\u0003\u0004\u0002\u0005"+
		"uv\n\u0003\u0000\u0000vw\u0005\u001d\u0000\u0000w~\u0003\u0004\u0002\u0004"+
		"xy\n\u0002\u0000\u0000yz\u0005\u001e\u0000\u0000z~\u0003\u0004\u0002\u0003"+
		"{|\n\u000b\u0000\u0000|~\u0005\u0007\u0000\u0000}\\\u0001\u0000\u0000"+
		"\u0000}_\u0001\u0000\u0000\u0000}b\u0001\u0000\u0000\u0000}e\u0001\u0000"+
		"\u0000\u0000}l\u0001\u0000\u0000\u0000}o\u0001\u0000\u0000\u0000}r\u0001"+
		"\u0000\u0000\u0000}u\u0001\u0000\u0000\u0000}x\u0001\u0000\u0000\u0000"+
		"}{\u0001\u0000\u0000\u0000~\u0081\u0001\u0000\u0000\u0000\u007f}\u0001"+
		"\u0000\u0000\u0000\u007f\u0080\u0001\u0000\u0000\u0000\u0080\u0005\u0001"+
		"\u0000\u0000\u0000\u0081\u007f\u0001\u0000\u0000\u0000\u0082\u0083\u0005"+
		"\f\u0000\u0000\u0083\u0084\u0003\u0004\u0002\u0000\u0084\u0085\u0005\u000e"+
		"\u0000\u0000\u0085\u0086\u0003\u0004\u0002\u0000\u0086\u0007\u0001\u0000"+
		"\u0000\u0000\u0087\u0089\u0003\n\u0005\u0000\u0088\u0087\u0001\u0000\u0000"+
		"\u0000\u0089\u008c\u0001\u0000\u0000\u0000\u008a\u0088\u0001\u0000\u0000"+
		"\u0000\u008a\u008b\u0001\u0000\u0000\u0000\u008b\t\u0001\u0000\u0000\u0000"+
		"\u008c\u008a\u0001\u0000\u0000\u0000\u008d\u008e\u0005\u000b\u0000\u0000"+
		"\u008e\u008f\u0005/\u0000\u0000\u008f\u0090\u0005\u001f\u0000\u0000\u0090"+
		"\u00b0\u0003\u0004\u0002\u0000\u0091\u0092\u0003\u0004\u0002\u0000\u0092"+
		"\u0093\u0005\u001b\u0000\u0000\u0093\u0094\u0005\u001c\u0000\u0000\u0094"+
		"\u00b0\u0001\u0000\u0000\u0000\u0095\u0096\u0003\u0002\u0001\u0000\u0096"+
		"\u0097\u0005\u001f\u0000\u0000\u0097\u0098\u0003\u0004\u0002\u0000\u0098"+
		"\u00b0\u0001\u0000\u0000\u0000\u0099\u009a\u0005\u0011\u0000\u0000\u009a"+
		"\u00b0\u0003\u0004\u0002\u0000\u009b\u009c\u0005\r\u0000\u0000\u009c\u009d"+
		"\u0003\u0004\u0002\u0000\u009d\u009e\u0005\u000e\u0000\u0000\u009e\u00a2"+
		"\u0003\b\u0004\u0000\u009f\u00a1\u0003\f\u0006\u0000\u00a0\u009f\u0001"+
		"\u0000\u0000\u0000\u00a1\u00a4\u0001\u0000\u0000\u0000\u00a2\u00a0\u0001"+
		"\u0000\u0000\u0000\u00a2\u00a3\u0001\u0000\u0000\u0000\u00a3\u00a7\u0001"+
		"\u0000\u0000\u0000\u00a4\u00a2\u0001\u0000\u0000\u0000\u00a5\u00a6\u0005"+
		"\u000f\u0000\u0000\u00a6\u00a8\u0003\b\u0004\u0000\u00a7\u00a5\u0001\u0000"+
		"\u0000\u0000\u00a7\u00a8\u0001\u0000\u0000\u0000\u00a8\u00a9\u0001\u0000"+
		"\u0000\u0000\u00a9\u00aa\u0005\u0010\u0000\u0000\u00aa\u00b0\u0001\u0000"+
		"\u0000\u0000\u00ab\u00b0\u0005\u0001\u0000\u0000\u00ac\u00b0\u0005\u0002"+
		"\u0000\u0000\u00ad\u00ae\u00050\u0000\u0000\u00ae\u00b0\u0005\b\u0000"+
		"\u0000\u00af\u008d\u0001\u0000\u0000\u0000\u00af\u0091\u0001\u0000\u0000"+
		"\u0000\u00af\u0095\u0001\u0000\u0000\u0000\u00af\u0099\u0001\u0000\u0000"+
		"\u0000\u00af\u009b\u0001\u0000\u0000\u0000\u00af\u00ab\u0001\u0000\u0000"+
		"\u0000\u00af\u00ac\u0001\u0000\u0000\u0000\u00af\u00ad\u0001\u0000\u0000"+
		"\u0000\u00b0\u000b\u0001\u0000\u0000\u0000\u00b1\u00b3\u0005\u000f\u0000"+
		"\u0000\u00b2\u00b1\u0001\u0000\u0000\u0000\u00b2\u00b3\u0001\u0000\u0000"+
		"\u0000\u00b3\u00b4\u0001\u0000\u0000\u0000\u00b4\u00b5\u0005\r\u0000\u0000"+
		"\u00b5\u00b6\u0003\u0004\u0002\u0000\u00b6\u00b7\u0005\u000e\u0000\u0000"+
		"\u00b7\u00b8\u0003\b\u0004\u0000\u00b8\r\u0001\u0000\u0000\u0000\u0011"+
		"\u0012\u0019\u001f#-/:@RZ}\u007f\u008a\u00a2\u00a7\u00af\u00b2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}