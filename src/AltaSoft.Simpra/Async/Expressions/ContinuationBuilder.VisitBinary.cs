using System;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Expressions;

internal partial class ContinuationBuilder
{
    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (_currentState is null)
        {
            throw new InvalidOperationException("No current state");
        }

        var left = Visit(node.Left);
        var right = VisitAsFiber(node.Right, FiberMode.Continuous);
        right.SetName("Binary", _currentState.StateId, "Right");
        if (!right.IsAsync)
        {
            return node.Update(left, node.Conversion, right.Expression);
        }
        var leftExitState = _currentState;
        if (node.NodeType is ExpressionType.AndAlso or ExpressionType.OrElse)
        {
            _currentState = CreateState(node.Type);
            _currentState.SetName("Binary", leftExitState.StateId, "Shortcut Merge");
            right.ContinueWith(_currentState);
            // Short-cutting must be performed
            var exprEvaluate = right.EntryState.ToExpression(_vars);
            var exprShortcut = Expression.Block(
                Expression.Assign(_vars.VarState, Expression.Constant(_currentState.StateId)),
                Expression.Assign(_currentState.ResultExpression, Expression.Constant(node.NodeType != ExpressionType.AndAlso, right.Expression.Type)));
            leftExitState.AddExpression(
                Expression.IfThenElse(
                    left,
                    node.NodeType == ExpressionType.AndAlso ? exprEvaluate : exprShortcut,
                    node.NodeType == ExpressionType.AndAlso ? exprShortcut : exprEvaluate));
            return _currentState.ResultExpression;
        }
        leftExitState.AddExpression(right.EntryState.ToExpression(_vars));
        _currentState = right.ExitState;
        return node.Update(left, node.Conversion, right.Expression);
    }
}
