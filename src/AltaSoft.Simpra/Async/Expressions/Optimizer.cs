using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Expressions;

internal sealed class Optimizer : ExpressionVisitor
{
    private static readonly HashSet<ExpressionType> s_assignmentTypes =
    [
        ExpressionType.Assign,
        ExpressionType.AddAssign,
        ExpressionType.AddAssignChecked,
        ExpressionType.AndAssign,
        ExpressionType.DivideAssign,
        ExpressionType.ExclusiveOrAssign,
        ExpressionType.LeftShiftAssign,
        ExpressionType.ModuloAssign,
        ExpressionType.MultiplyAssign,
        ExpressionType.MultiplyAssignChecked,
        ExpressionType.OrAssign,
        ExpressionType.AddAssign,
        ExpressionType.PowerAssign,
        ExpressionType.PreDecrementAssign,
        ExpressionType.PreIncrementAssign,
        ExpressionType.RightShiftAssign,
        ExpressionType.SubtractAssign,
        ExpressionType.SubtractAssignChecked
    ]; // this list does not contain PostXxxAssign since these operators return a different value as result than the assigned value

    protected override Expression VisitBlock(BlockExpression node)
    {
        var expressions = new List<Expression>(node.Expressions.Count * 2);
        var variables = node.Variables.ToList();
        var queue = new Queue<Expression>();

        foreach (var expr2 in node.Expressions.Select(Visit))
        {
            if (expr2 is not null)
                queue.Enqueue(expr2);

            do
            {
                var expr = queue.Dequeue();
                if (expr is BlockExpression nestedBlock)
                {
                    // Flatten nested block by adding items to the queue
                    foreach (var nested in nestedBlock.Expressions)
                    {
                        queue.Enqueue(nested);
                    }
                    variables.AddRange(nestedBlock.Variables);
                }
                else if (expr is GotoExpression exprGoto)
                {
                    // Stop after the goto, but make sure the result type matches
                    expressions.Add(node.Type == exprGoto.Type
                        ? exprGoto
                        : Expression.MakeGoto(exprGoto.Kind, exprGoto.Target, exprGoto.Value, node.Type));
                    break;
                }
                else if (expr is UnaryExpression { NodeType: ExpressionType.Throw } exprThrow)
                {
                    // Stop after the throw, but make sure the result type matches
                    expressions.Add(node.Type == exprThrow.Type
                        ? expr
                        : Expression.Throw(exprThrow.Operand, node.Type));
                    break;
                }
                else
                {
                    expressions.Add(expr);
                }
            } while (queue.Count > 0);
        }
        // Remove side-effect-free but useless expressions
        for (var i = expressions.Count - 2; i >= 0; i--)
        {
            if (expressions[i] is DefaultExpression or ConstantExpression or ParameterExpression)
            {
                expressions.RemoveAt(i);
            }
        }
        // If the last item is a parameter expression, and the one before an assignment to it, discard last item
        while (expressions is [.., BinaryExpression binary, ParameterExpression param] &&
               s_assignmentTypes.Contains(binary.NodeType) &&
               binary.Left == param
              )
        {
            expressions.RemoveAt(expressions.Count - 1);
        }
        // if we have an x = result, y = x, and x is a local variable, rewrite this as y = result
        while (expressions is [.., BinaryExpression { NodeType: ExpressionType.Assign, Left: ParameterExpression para1 } assign1, BinaryExpression { NodeType: ExpressionType.Assign, Conversion: null } assign2] &&
               assign2.Right == para1 && variables.Contains(para1))
        {
            expressions[^2] = assign1.Update(assign2.Left, assign1.Conversion, assign1.Right);
            expressions.RemoveAt(expressions.Count - 1);
        }
        // Remove redundant void at the end
        if (expressions[^1] is DefaultExpression def && def.Type == typeof(void))
        {
            expressions.RemoveAt(expressions.Count - 1);
        }
        return expressions.Count == 0
            ? Expression.Default(node.Type)
            : expressions.Count == 1 && node.Type == expressions[0].Type && variables.Count == 0
                ? expressions[0]
                : node.Update(variables, expressions);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        var result = base.VisitBinary(node);
        if (result is BinaryExpression { NodeType: ExpressionType.Assign, Left: ParameterExpression } binary && binary.Right == binary.Left)
        {
            // Change assignment to itself to just the variable, since this has the same semantics
            return binary.Right;
        }
        return result;
    }

    protected override Expression VisitConditional(ConditionalExpression node)
    {
        var result = base.VisitConditional(node);
        // Simplify conditionals if the test is side-effect-free and true and false does assign the same constant
        return result is ConditionalExpression
        {
            IfTrue: BinaryExpression
            {
                NodeType: ExpressionType.Assign,
                Right: ConstantExpression ifTrueConst
            } ifTrue,
            IfFalse: BinaryExpression
            {
                NodeType: ExpressionType.Assign,
                Right: ConstantExpression ifFalseConst
            } ifFalse
        } conditional
               && ReferenceEquals(ifTrue.Left, ifFalse.Left)
               && Equals(ifTrueConst.Value, ifFalseConst.Value)
               && conditional.Test.IsSafeCode() // most expensive check last
            ? node.IfTrue
            : result;
    }

    public override Expression? Visit(Expression? node)
    {
        var result = base.Visit(node);
        while (result is { CanReduce: true })
        {
            result = result.Reduce();
        }
        return result;
    }
}
