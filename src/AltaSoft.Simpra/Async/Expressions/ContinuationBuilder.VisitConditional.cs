using System;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Expressions;

internal partial class ContinuationBuilder
{
    protected override Expression VisitConditional(ConditionalExpression node)
    {
        var test = Visit(node.Test);
        var ifTrue = VisitAsFiber(node.IfTrue, FiberMode.Continuous);
        var ifFalse = VisitAsFiber(node.IfFalse, FiberMode.Continuous);
        if (!ifTrue.IsAsync && !ifFalse.IsAsync)
        {
            // no await inside conditional branches, proceed normally
            return node.Update(test, ifTrue.Expression, ifFalse.Expression);
        }

        if (_currentState is null)
        {
            throw new InvalidOperationException("No current state");
        }

        var testExitState = _currentState;
        ifTrue.SetName("Conditional", testExitState.StateId, "True");
        ifFalse.SetName("Conditional", testExitState.StateId, "False");
        _currentState = CreateState(node.Type);
        _currentState.SetName("Conditional", testExitState.StateId, "Merge");
        ifTrue.ContinueWith(_currentState);
        ifFalse.ContinueWith(_currentState);
        testExitState.AddExpression(
            Expression.IfThenElse(
                test,
                ifTrue.EntryState.ToExpression(_vars),
                ifFalse.EntryState.ToExpression(_vars)));
        return _currentState.ResultExpression;
    }
}
