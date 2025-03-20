using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Tasks;

internal interface ICompletionSourceEmitter
{
    Expression Create();

    Expression SetResult(ParameterExpression varCompletionSource, Expression result);

    Expression SetException(ParameterExpression varCompletionSource, Expression exception);

    Expression GetAwaitable(ParameterExpression varCompletionSource);

    Expression GetFromResult(Expression result);

    Expression GetFromException(Expression exception);
}