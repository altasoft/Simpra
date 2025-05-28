using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using AltaSoft.Simpra.Types;
using Antlr4.Runtime.Tree;

namespace AltaSoft.Simpra.Visitor;

internal partial class SimpraParserVisitor<TResult, TModel>
    : AbstractParseTreeVisitor<Expression>, ISimpraParserVisitor<Expression>
    where TModel : class
{
    private readonly ParameterExpression _rootParameter;
    private readonly ParameterExpression? _externalFunctionsParameter;
    private readonly ParameterExpression _cancellationTokenParam;

    private readonly ActiveCompilerOptions _compilerOptions;

    private readonly Dictionary<string, (ParameterExpression Variable, Type VariableType)> _variables = [];

    private readonly LabelTarget _endLabel;

    public Type DataModelType { get; }

    private int _localVariableCounter;

    public SimpraParserVisitor(SimpraCompilerOptions? compilerOptions, ParameterExpression rootParameter, ParameterExpression? externalFunctionsParameter, ParameterExpression cancellationTokenParam)
    {
        _rootParameter = rootParameter;
        _externalFunctionsParameter = externalFunctionsParameter;
        _cancellationTokenParam = cancellationTokenParam;

        _compilerOptions = new ActiveCompilerOptions(compilerOptions);

        DataModelType = typeof(TModel);

        _endLabel = Expression.Label(typeof(TResult), "end_label");
    }

    /// <inheritdoc />
    public Expression VisitProgram(SimpraParser.ProgramContext context)
    {
        if (context.Expr is null && context.Main is null)
        {
            throw new SimpraException(context, "Cannot parse source code");
        }

        var exprCaseSensitivity = SetCaseSensitivity(_compilerOptions.IsCaseSensitive);

        if (context.Expr is not null) // Single expression
        {
            return Expression.Block(exprCaseSensitivity, ConvertToType(Visit(context.Expr), typeof(TResult), context));
        }

        // Block of statements
        var expression = Visit(context.Main);

        var exprEndLabel = Expression.Label(_endLabel, Expression.Default(typeof(TResult)));

        var variables = _variables.Values.Select(v => v.Variable).ToList();

        return Expression.Block(variables, exprCaseSensitivity, expression, exprEndLabel);
    }

    /// <inheritdoc />
    public Expression VisitStatementBlock(SimpraParser.StatementBlockContext context)
    {
        var exprStatements = new List<Expression>();
        for (var i = 0; i < context.ChildCount; i++)
        {
            var stmt = context.statement(i);
            exprStatements.Add(Visit(stmt));
        }

        return Expression.Block(exprStatements);
    }

    /// <inheritdoc />
    public Expression VisitBinary(SimpraParser.BinaryContext context)
    {
        var left = Visit(context.Left);
        var right = Visit(context.Right);
        var operatorText = context.Operator.Text;

        return operatorText switch
        {
            "+" => HandleConcat(left, right, context),
            "-" => HandleSubtract(left, right, context),
            "*" => HandleMultiply(left, right, context),
            "/" => HandleDivide(left, right, context),
            "//" => CallBuiltinStaticMethod(typeof(InternalLanguageFunctions), nameof(InternalLanguageFunctions.round), [HandleDivide(left, right, context)], context),

            "is" => HandleEquals(left, right, context),
            "is not" => Expression.Not(HandleEquals(left, right, context)),

            "in" => HandleInOperator(left, right, context),
            "any in" => HandleAnyInOperator(left, right, context),
            "all in" => HandleAllInOperator(left, right, context),

            "not in" => Expression.Not(HandleInOperator(left, right, context)),
            "any not in" => Expression.Not(HandleAnyInOperator(left, right, context)),
            "all not in" => Expression.Not(HandleAllInOperator(left, right, context)),

            "like" => HandleLikeOperator(left, right, context),
            "matches" => HandleMatchesOperator(left, right, context),
            "min" => HandleMin(left, right, context),
            "max" => HandleMax(left, right, context),
            _ => throw new SimpraException(context, $"Unsupported binary operator: {operatorText}")
        };
    }

    /// <inheritdoc />
    public Expression VisitComparison(SimpraParser.ComparisonContext context)
    {
        var left = Visit(context.Left);
        var right = Visit(context.Right);
        var operatorText = context.Operator.Text;

        return HandleComparison(left, right, operatorText, context);
    }

    /// <inheritdoc />
    public Expression VisitChainedComparison(SimpraParser.ChainedComparisonContext context)
    {
        var left = Visit(context.Left);
        var leftOperatorText = context.LeftOperator.Text;
        var expr = Visit(context.Expression);
        var rigOperatorText = context.RightOperator.Text;
        var right = Visit(context.Right);

        return HandleChainedComparison(left, leftOperatorText, expr, rigOperatorText, right);
    }

    /// <inheritdoc />
    public Expression VisitUnary(SimpraParser.UnaryContext context)
    {
        var expr = Visit(context.Expression);
        var operatorText = context.Operator.Text;

        return operatorText switch
        {
            "-" => Expression.Negate(expr),
            "+" => Expression.UnaryPlus(expr),
            "not" => Expression.Not(expr),
            "%" => Expression.Convert(Expression.Multiply(expr, Expression.Constant(new SimpraNumber(0.01m), typeof(SimpraNumber))), typeof(SimpraNumber)),
            _ => throw new SimpraException(context, $"Unsupported unary operator: {operatorText}")
        };
    }

    /// <inheritdoc />
    public Expression VisitBinaryAnd(SimpraParser.BinaryAndContext context)
    {
        var left = Visit(context.Left);
        var right = Visit(context.Right);

        return Expression.AndAlso(ConvertToBool(left), ConvertToBool(right));
    }

    /// <inheritdoc />
    public Expression VisitBinaryOr(SimpraParser.BinaryOrContext context)
    {
        var left = Visit(context.Left);
        var right = Visit(context.Right);

        return Expression.OrElse(ConvertToBool(left), ConvertToBool(right));
    }

    /// <inheritdoc />
    public Expression VisitHasValue(SimpraParser.HasValueContext context)
    {
        // Visit the child expression in the context
        var childExpression = Visit(context.Left);

        if (childExpression.Type.IsSimpraType())
        {
            return Expression.Property(childExpression, childExpression.Type, nameof(ISimpraType.HasValue));
        }

        // Check if the type is nullable
        if (!childExpression.Type.IsValueType || childExpression.Type.IsNullableT())
        {
            // If nullable, check if the value is not null
            return Expression.NotEqual(childExpression, Expression.Constant(null, childExpression.Type));
        }

        // If the type is not nullable, always return true
        return Expression.Constant(true);
    }

    /// <inheritdoc />
    public Expression VisitObjectRef(SimpraParser.ObjectRefContext context)
    {
        return VisitChildren(context);
    }

    /// <inheritdoc />
    public Expression VisitMemberAccess(SimpraParser.MemberAccessContext context)
    {

        var exprBaseObject = Visit(context.Object);

        var propertyName = context.PropertyName.Text;
        if (exprBaseObject.Type.IsSimpraInteropObject())
        {
            exprBaseObject = Expression.Property(exprBaseObject, nameof(SimpraInteropObject<object>.Value));

        }

        var accessType = GetIdentifierAccessType(context);

        // If instance is not a reference type or Nullable<T>, wrap in null check
        if (accessType == IdentifierAccessType.Write || exprBaseObject.Type.IsValueType && Nullable.GetUnderlyingType(exprBaseObject.Type) is null)
        {
            var exprProperty = Expression.Property(exprBaseObject, propertyName);

            return accessType switch
            {
                IdentifierAccessType.Read => ConvertToSimpraType(exprProperty, true, context),
                IdentifierAccessType.Write => exprProperty,
                _ => throw new InvalidEnumArgumentException($"Unknown value for enum '{nameof(IdentifierAccessType)}'")
            };
        }

        var variable = Expression.Variable(exprBaseObject.Type, "instance" + _localVariableCounter++);
        var assignExpr = Expression.Assign(variable, exprBaseObject);

        var exprProperty2 = Expression.Property(variable, propertyName);
        var notNull = Expression.NotEqual(variable, Expression.Constant(null, exprBaseObject.Type));

        var defaultValue = Expression.Default(exprProperty2.Type);

        var expr = Expression.Condition(notNull, exprProperty2, defaultValue);

        var block = Expression.Block([variable], assignExpr, expr);

        return ConvertToSimpraType(block, true, context);

    }

    /// <inheritdoc />
    public Expression VisitIndexAccess(SimpraParser.IndexAccessContext context)
    {
        // Visit the base expression (e.g., array, list, or string)
        var left = Visit(context.Object);

        // Visit the index expression (e.g., the index value)
        var index = Visit(context.Index);

        var objectType = left.Type;

        // Handle strings
        if (objectType == typeof(SimpraString))
        {
            // Call string.Substring(index, 1) to extract the character as a string
            return CallBuiltinInstanceMethod(left, nameof(SimpraString.Substring), [index], context);
        }

        if (objectType.IsSimpraInteropObject())
        {
            left = Expression.Property(left, nameof(SimpraInteropObject<object>.Value));
            objectType = left.Type;
        }

        // Handle IDictionary<TKey, TValue> (including Dictionary<,>)
        var isDictionary = objectType.IsIReadOnlyDictionaryT();

        if (isDictionary)
        {
            return HandleDictionaryIndexAccess(context, objectType, index, left);
        }

        // Handle indexable types (e.g., List<T>)
        var defaultIndexer = objectType
            .GetProperties()
            .FirstOrDefault(p => p.GetIndexParameters().Length > 0);

        if (defaultIndexer is not null)
        {
            var defaultIndexType = defaultIndexer.GetIndexParameters()[0].ParameterType;
            if (index.Type != defaultIndexType)
                index = Expression.Convert(index, defaultIndexType);

            // Generate a property indexer access expression
            return ConvertToSimpraType(Expression.MakeIndex(left, defaultIndexer, [index]), false, context);
        }

        throw new InvalidOperationException($"The type '{objectType.Name}' does not support index access");
    }

    /// <inheritdoc />
    public Expression VisitFunctionCall(SimpraParser.FunctionCallContext context)
    {
        var methodName = context.FunctionName.Text;

        var arguments = context.exp();

        var exprArguments = new List<Expression>(arguments.Length);
        exprArguments.AddRange(arguments.Select(Visit));

        // Special handling for internal functions like 'length'
        if (IsSimpraBuiltinMethod(methodName, exprArguments))
        {
            return CallBuiltinStaticMethod(typeof(InternalLanguageFunctions), methodName, [.. exprArguments], context);
        }

        // Handle external function calls (both sync and async)
        return CallFunction(methodName, [.. exprArguments], context);
    }

    /// <inheritdoc />
    public Expression VisitParenthesis(SimpraParser.ParenthesisContext context)
    {
        return Visit(context.Expression);
    }

    /// <inheritdoc />
    public Expression VisitWhen(SimpraParser.WhenContext context)
    {
        var condition = Visit(context.Condition);
        var exprTrue = Visit(context.ExpressionTrue);
        var exprFalse = Visit(context.ExpressionFalse);

        var extraWhens = context.extraWhen();
        if (extraWhens.Length == 0)
        {
            return Expression.Condition(ConvertToBool(condition), exprTrue, exprFalse);
        }

        // Start building the chain of conditions from the innermost 'else'
        var nestedExpr = exprFalse;

        // Process each extraWhen in reverse order to correctly nest them
        for (var i = extraWhens.Length - 1; i >= 0; i--)
        {
            // Visit the extra when condition and true/false expressions
            if (Visit(context.extraWhen(i)) is not ConditionalExpression exprExtraWhen)
            {
                throw new SimpraException(context, "Extra 'when' expression must be a conditional expression");
            }

            var extraCondition = exprExtraWhen.Test;
            var extraExprTrue = exprExtraWhen.IfTrue;

            // Build the nested condition
            nestedExpr = Expression.Condition(ConvertToBool(extraCondition), extraExprTrue, nestedExpr);
        }

        return Expression.Condition(ConvertToBool(condition), exprTrue, nestedExpr);
    }

    /// <inheritdoc />
    public Expression VisitExtraWhenExpr(SimpraParser.ExtraWhenExprContext context)
    {
        var condition = Visit(context.Condition);
        var exprTrue = Visit(context.ExpressionTrue);

        // IfFalse will be ignored in VisitWhen
        return Expression.Condition(ConvertToBool(condition), exprTrue, exprTrue);
    }

    /// <inheritdoc />
    public Expression VisitIfElseStatement(SimpraParser.IfElseStatementContext context)
    {
        var condition = Visit(context.Condition);
        var blockTrue = Visit(context.BlockTrue);
        var blockFalse = context.BlockFalse is null ? Expression.Default(blockTrue.Type) : Visit(context.BlockFalse);

        var extraIfs = context.extraIf();
        if (extraIfs.Length == 0)
        {
            return Expression.Condition(ConvertToBool(condition), blockTrue, blockFalse);
        }

        // Start building the chain of conditions from the innermost 'else'
        var nestedExpr = blockFalse;

        // Process each extraWhen in reverse order to correctly nest them
        for (var i = extraIfs.Length - 1; i >= 0; i--)
        {
            // Visit the extra when condition and true/false expressions
            if (Visit(context.extraIf(i)) is not ConditionalExpression exprExtraWhen)
            {
                throw new SimpraException(context, "Extra 'if' expression must be a conditional expression");
            }

            var extraCondition = exprExtraWhen.Test;
            var extraExprTrue = exprExtraWhen.IfTrue;

            // Build the nested condition
            nestedExpr = Expression.Condition(ConvertToBool(extraCondition), extraExprTrue, nestedExpr);
        }

        return Expression.Condition(ConvertToBool(condition), blockTrue, nestedExpr);
    }

    /// <inheritdoc />
    public Expression VisitCompoundAssignment(SimpraParser.CompoundAssignmentContext context)
    {
        var left = Visit(context.Left);
        var right = Visit(context.Right);

        return HandleCompoundAssignment(left, right, context);
    }

    /// <inheritdoc />
    public Expression VisitExtraIfExpr(SimpraParser.ExtraIfExprContext context)
    {
        var condition = Visit(context.Condition);
        var exprTrue = Visit(context.BlockTrue);

        // IfFalse will be ignored in VisitWhen
        return Expression.Condition(ConvertToBool(condition), exprTrue, exprTrue);
    }

    /// <inheritdoc />
    public Expression VisitVariableDeclaration(SimpraParser.VariableDeclarationContext context)
    {
        var name = context.Name.Text;
        var right = Visit(context.Right);

        // Check property
        var propertyInfo = GetModelProperty(name);
        if (propertyInfo is not null)
        {
            throw new SimpraException(context, $"Property '{name}' is already declared in model");
        }

        if (_externalFunctionsParameter is not null)
        {
            propertyInfo = GetExternalProperty(_externalFunctionsParameter.Type, _externalFunctionsParameter, name);
            if (propertyInfo is not null)
            {
                throw new SimpraException(context, $"Property '{name}' is already declared in type '{_externalFunctionsParameter.Type.Name}'");
            }
        }

        var variableInfo = GetVariable(name);
        if (variableInfo is not null)
        {
            throw new SimpraException(context, $"Variable '{name}' is already declared");
        }

        return HandleAssignment(CreateVariable(name, right).variable, right, context);
    }

    public Expression VisitAssignment(SimpraParser.AssignmentContext context)
    {
        var left = Visit(context.Left);
        var right = Visit(context.Right);

        return HandleAssignment(left, right, context);
    }

    /// <inheritdoc />
    public Expression VisitReturn2(SimpraParser.Return2Context context)
    {
        var expr = Visit(context.Value);

        return Expression.Return(_endLabel, ConvertToType(expr, typeof(TResult), context));
    }

    /// <inheritdoc />
    public Expression VisitLineComment(SimpraParser.LineCommentContext context)
    {
        return Expression.Empty();
    }

    /// <inheritdoc />
    public Expression VisitBlockComment(SimpraParser.BlockCommentContext context)
    {
        return Expression.Empty();
    }

    /// <inheritdoc />
    public Expression VisitDirective(SimpraParser.DirectiveContext context)
    {
        var directive = context.Name.Text;
        var value = context.OnOff.Text;

        switch (directive)
        {
            case "$case_sensitive":
                var isCaseSensitive = value != "off";
                _compilerOptions.IsCaseSensitive = isCaseSensitive;

                return SetCaseSensitivity(isCaseSensitive);

            case "$mutable":
                var isMutable = value == "on";
                if (isMutable && !_compilerOptions.CanEnableMutable)
                    throw new SimpraException(context, "Cannot enable mutable state in this context");
                _compilerOptions.IsMutable = isMutable;
                break;
        }

        // Directives don't produce an expression, so return Expression.Empty()
        return Expression.Empty();
    }

    /// <inheritdoc />
    public Expression VisitIdentifier(SimpraParser.IdentifierContext context)
    {
        var name = context.Identifier.Text;

        var exprProperty = GetModelProperty(name);
        if (exprProperty is null)
        {
            if (_externalFunctionsParameter is not null)
            {
                exprProperty = GetExternalProperty(_externalFunctionsParameter.Type, _externalFunctionsParameter, name);
            }

            if (exprProperty is null)
            {
                var variableInfo = GetVariable(name);
                if (variableInfo is not null)
                {
                    return variableInfo.Value.variable;
                }

                throw new SimpraException(context, $"Identifier '{name}' not found. Neither property of a model nor local variable");
            }
        }

        return GetIdentifierAccessType(context) switch
        {
            IdentifierAccessType.Read => ConvertToSimpraType(exprProperty, true, context),
            IdentifierAccessType.Write => exprProperty,
            _ => throw new InvalidEnumArgumentException($"Unknown value for enum '{nameof(IdentifierAccessType)}'")
        };
    }

    /// <inheritdoc />
    public Expression VisitNumber(SimpraParser.NumberContext context)
    {
        // If we declared constant, then it's not null
        return Expression.Constant(new SimpraNumber(context.GetText()), typeof(SimpraNumber));
    }

    /// <inheritdoc />
    public Expression VisitBool(SimpraParser.BoolContext context)
    {
        // If we declared constant, then it's not null
        return Expression.Constant(new SimpraBool(context.GetText()), typeof(SimpraBool));
    }

    public Expression VisitPascalString(SimpraParser.PascalStringContext context)
    {
        return Expression.Constant(new SimpraString(context.GetText().Trim('\'')), typeof(SimpraString));
    }
    public Expression VisitCString(SimpraParser.CStringContext context)
    {
        return Expression.Constant(new SimpraString(context.GetText().Trim('"')), typeof(SimpraString));
    }

    /// <inheritdoc />
    public Expression VisitArray(SimpraParser.ArrayContext context)
    {
        return NewSimpraList(context.exp().Select(Visit), context);
    }

    private static Expression HandleDictionaryIndexAccess(SimpraParser.IndexAccessContext context, Type objectType, Expression index, Expression left)
    {
        var keyType = objectType.GetGenericArguments()[0];
        var valueType = objectType.GetGenericArguments()[1];
        var tryGetValueMethod = objectType.GetMethod("TryGetValue", [keyType, valueType.MakeByRefType()])!;

        var keyExpr = index.Type != keyType ? Expression.Convert(index, keyType) : index;
        var valueVar = Expression.Variable(valueType, "dictValue");

        var tryGetValueCall = Expression.Call(left, tryGetValueMethod, keyExpr, valueVar);

        var defaultValue = Expression.Default(valueType);
        var block = Expression.Block([valueVar], Expression.Condition(tryGetValueCall, valueVar, defaultValue));

        return ConvertToSimpraType(block, false, context);
    }
}
