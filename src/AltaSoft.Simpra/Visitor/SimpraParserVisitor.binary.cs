using System;
using System.Linq.Expressions;
using AltaSoft.Simpra.Types;
using Antlr4.Runtime;

namespace AltaSoft.Simpra.Visitor;

internal partial class SimpraParserVisitor<TResult, TModel>
{
    private static MethodCallExpression HandleEquals(Expression left, Expression right, ParserRuleContext context)
    {
        return CallBuiltinStaticMethod(left.Type, "op_Equality", [left, right], context);
    }

    private static MethodCallExpression HandleConcat(Expression left, Expression right, ParserRuleContext context)
    {
        return CallBuiltinStaticMethod(left.Type, "op_Addition", [left, right], context);
    }

    private static MethodCallExpression HandleSubtract(Expression left, Expression right, ParserRuleContext context)
    {
        return CallBuiltinStaticMethod(left.Type, "op_Subtraction", [left, right], context);
    }

    private static MethodCallExpression HandleMultiply(Expression left, Expression right, ParserRuleContext context)
    {
        return CallBuiltinStaticMethod(left.Type, "op_Multiply", [left, right], context);
    }

    private static MethodCallExpression HandleDivide(Expression left, Expression right, ParserRuleContext context)
    {
        return CallBuiltinStaticMethod(left.Type, "op_Division", [left, right], context);
    }

    private static BinaryExpression ComparisonOperatorToExpression(Expression left, Expression right, string @operator)
    {
        return @operator switch
        {
            ">" => Expression.GreaterThan(left, right),
            ">=" => Expression.GreaterThanOrEqual(left, right),
            "<" => Expression.LessThan(left, right),
            "<=" => Expression.LessThanOrEqual(left, right),
            _ => throw new InvalidOperationException($"Invalid comparison operator '{@operator}'")
        };
    }

    private static BinaryExpression HandleComparison(Expression left, Expression right, string @operator, ParserRuleContext context)
    {
        var expr = CallBuiltinInstanceMethod(left, nameof(SimpraNumber.CompareTo), [right], context);

        return ComparisonOperatorToExpression(expr, Expression.Default(typeof(int)), @operator);
    }

    private static BinaryExpression HandleChainedComparison(Expression left, string leftOperator, Expression expr, string rightOperator, Expression right)
    {
        left = ComparisonOperatorToExpression(expr, left, leftOperator);
        right = ComparisonOperatorToExpression(expr, right, rightOperator);

        return Expression.AndAlso(left, right);
    }

    private static MethodCallExpression HandleInOperator(Expression left, Expression right, ParserRuleContext context)
    {
        return CallBuiltinStaticMethod(left.Type, nameof(SimpraString.In), [left, right], context);
    }
    private static MethodCallExpression HandleAnyInOperator(Expression left, Expression right, ParserRuleContext context)
    {
        return CallBuiltinStaticMethod(left.Type, nameof(SimpraList<SimpraString, string>.Any), [left, right], context);
    }
    private static MethodCallExpression HandleAllInOperator(Expression left, Expression right, ParserRuleContext context)
    {
        return CallBuiltinStaticMethod(left.Type, nameof(SimpraList<SimpraString, string>.All), [left, right], context);
    }

    private static MethodCallExpression HandleLikeOperator(Expression left, Expression right, ParserRuleContext context)
    {
        return CallBuiltinStaticMethod(left.Type, nameof(SimpraString.Like), [left, right], context);
    }
    private static MethodCallExpression HandleMatchesOperator(Expression left, Expression right, ParserRuleContext context)
    {
        return CallBuiltinStaticMethod(left.Type, nameof(SimpraString.Matches), [left, right], context);
    }

    private static MethodCallExpression HandleMin(Expression left, Expression right, ParserRuleContext context)
    {
        return CallBuiltinInstanceMethod(left, nameof(SimpraNumber.Min), [right], context);
    }

    private static MethodCallExpression HandleMax(Expression left, Expression right, ParserRuleContext context)
    {
        return CallBuiltinInstanceMethod(left, nameof(SimpraNumber.Max), [right], context);
    }

    private static Expression HandleCompoundAssignment(Expression left, Expression right, SimpraParser.CompoundAssignmentContext context)
    {
        //if (!left.Type.IsSimpraList())
        //    throw new SimpraException(context, "Left operand of '+=' or '-=' must be a list");

        var methodName = context.Operator.Text switch
        {
            "+=" => nameof(SimpraList<SimpraNumber, decimal>.AddAndAssign),
            "-=" => nameof(SimpraList<SimpraNumber, decimal>.SubtractAndAssign),
            _ => throw new InvalidOperationException($"Invalid list operation '{context.Operator.Text}'")
        };
        return CallBuiltinInstanceMethod(left, methodName, [right], context);
    }
}
