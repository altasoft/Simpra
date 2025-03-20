using System;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Expressions;

internal partial class ContinuationBuilder
{
    public Expression VisitAwait(AwaitExpression node)
    {
        if (_currentState is null)
        {
            throw new InvalidOperationException("No current state");
        }

        var nextState = CreateState(node.Type);
        nextState.SetName("Await", _currentState.StateId, "GetResult");
        _currentState.SetContinuation(nextState);

        var exprAwaitable = Visit(node.Awaitable);
        if (exprAwaitable is null)
        {
            throw new InvalidOperationException("Awaitable expression is null");
        }

        var methGetAwaiter = exprAwaitable.Type.GetAwaitableGetAwaiterMethod()
            ?? throw new InvalidOperationException($"Awaitable expression of type {exprAwaitable.Type} does not have GetAwaiter method");

        var methAwaitable = exprAwaitable.Type.GetAwaitableGetAwaiterMethod()
            ?? throw new InvalidOperationException($"Awaiter of type {exprAwaitable.Type} does not have IsCompleted property");

        var varAwaiter = _vars.GetVarAwaiter(methGetAwaiter.ReturnType);
        var methGetIsComplete = varAwaiter.Type.GetAwaiterIsCompletedProperty()
            ?? throw new InvalidOperationException($"Awaiter of type {varAwaiter.Type} does not have IsCompleted property");

        var methOnCompleted = varAwaiter.Type.GetAwaiterOnCompletedMethod()
            ?? throw new InvalidOperationException($"Awaiter of type {varAwaiter.Type} does not have OnCompleted method");

        var methGetResult = varAwaiter.Type.GetAwaiterGetResultMethod()
            ?? throw new InvalidOperationException($"Awaiter of type {varAwaiter.Type} does not have GetResult method");

        _currentState.AddExpression(
            Expression.IfThen(
                Expression.Not(
                    Expression.Property(
                        Expression.Assign(varAwaiter, Expression.Call(exprAwaitable, methAwaitable)), methGetIsComplete)),
                Expression.Block(
                    Expression.Call(varAwaiter, methOnCompleted, _vars.VarContinuation),
                    Expression.Break(_vars.LblBreak, Expression.Default(_vars.LblBreak.Type)))));

        nextState.AddExpression(methGetResult.ReturnType == typeof(void)
            ? Expression.Call(varAwaiter, methGetResult)
            : Expression.Assign(nextState.ResultExpression, Expression.Call(varAwaiter, methGetResult)));
        _currentState = nextState;
        return _currentState.ResultExpression;
    }
}
