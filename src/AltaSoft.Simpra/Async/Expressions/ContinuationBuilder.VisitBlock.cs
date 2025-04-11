using System;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Expressions;

internal partial class ContinuationBuilder
{
    protected override Expression VisitBlock(BlockExpression node)
    {
        if (!node.RequiresStateMachine(true))
        {
            return node;
        }
        using var enumerator = node.Expressions.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException("Empty block");
        }
#pragma warning disable S1264
        for (; ; )
#pragma warning restore S1264
        {
            var expression = Visit(enumerator.Current);
            if (enumerator.MoveNext())
            {
                if (_currentState is null)
                {
                    throw new InvalidOperationException("No current state");
                }

                _currentState.AddExpression(expression);
            }
            else
            {
                return expression;
            }
        }
    }
}
