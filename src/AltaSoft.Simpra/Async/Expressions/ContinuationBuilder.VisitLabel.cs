using System;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Expressions;

internal partial class ContinuationBuilder
{
    protected override Expression VisitLabel(LabelExpression node)
    {
        if (_currentState is null)
        {
            throw new InvalidOperationException("No current state");
        }

        var defaultValue = Visit(node.DefaultValue);
        var defaultValueExitState = _currentState;
        _currentState = GetLabelState(node.Target);
        defaultValueExitState.SetContinuation(_currentState);
        if (defaultValue != null && defaultValue.Type != typeof(void))
        {
            defaultValueExitState.AddExpression(
                Expression.Assign(_currentState.ResultExpression,
                    defaultValue));
        }
        return _currentState.ResultExpression;
    }
}
