using System;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Expressions;

internal partial class ContinuationBuilder
{
    public Expression VisitAsyncLambda<TDelegate>(AsyncLambdaExpression<TDelegate> node) where TDelegate : Delegate => node.BuildLambdaExpression();
}
