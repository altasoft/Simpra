using System;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Expressions;

internal interface IAsyncExpressionVisitor
{
    Expression VisitAsyncLambda<TDelegate>(AsyncLambdaExpression<TDelegate> node) where TDelegate : Delegate;

    Expression VisitAwait(AwaitExpression node);
}
