using System;
using System.Linq.Expressions;
using AltaSoft.Simpra.Async.Collections;

namespace AltaSoft.Simpra.Async.Expressions;

internal partial class ContinuationBuilder
{
    protected override Expression VisitGoto(GotoExpression node)
    {
        if (_currentState is null)
        {
            throw new InvalidOperationException("No current state");
        }

        var value = Visit(node.Value);
        if (value is null)
        {
            throw new InvalidOperationException("Goto value is null");
        }

        var targetState = GetLabelState(node.Target);
        if (node.Target.Type != typeof(void))
        {
            _currentState.AddExpression(Expression.Assign(targetState.ResultExpression, value));
        }
        _currentState.SetContinuation(targetState);
        _currentState = new MachineState(-1, node.Type, ImmutableStack<TryInfo>.s_empty, false);
        _currentState.SetName("Goto", targetState.StateId, "Virtual");
        return Expression.Default(node.Type);
    }
}
