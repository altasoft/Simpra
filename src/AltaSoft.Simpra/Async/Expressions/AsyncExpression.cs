using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AltaSoft.Simpra.Async.Tasks;

namespace AltaSoft.Simpra.Async.Expressions;

internal static class AsyncExpression
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static AsyncLambdaExpression<TDelegate> AsyncLambda<TDelegate>(Expression body, params ParameterExpression[] parameters) where TDelegate : Delegate
        => AsyncLambda<TDelegate>(body, default, parameters);

    internal static AsyncLambdaExpression<TDelegate> AsyncLambda<TDelegate>(Expression body, string? name, IEnumerable<ParameterExpression> parameters) where TDelegate : Delegate
    {
        // Using the Emitter.GetFromResult to wraps the body into a result, thereby making sure that the method signature is correct
        var emitter = CompletionSourceEmitterFactory.Get(typeof(TDelegate).GetDelegateInvokeMethod().ReturnType);
        // Using the Expression.Lambda<> in order to perform type and parameter checks
        var lambda = Expression.Lambda<TDelegate>(emitter.GetFromResult(body ?? throw new ArgumentNullException(nameof(body))), name, parameters);
        // The AsyncLambdaExpression<> constructor does not perform checks
        return new AsyncLambdaExpression<TDelegate>(name, body, lambda.Parameters);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static AwaitExpression AwaitConfigured(this Expression awaitable, bool continueOnCapturedContext) => new(ConfigureAwaitInternal(awaitable, Expression.Constant(continueOnCapturedContext, typeof(bool)), false));

    private static Expression ConfigureAwaitInternal(Expression awaitable, Expression continueOnCapturedContext, bool ignoreMissingConfigureAwaitMethod)
    {
        var methConfigureAwait = (awaitable ?? throw new ArgumentNullException(nameof(awaitable))).Type.GetAwaitableGetConfigureAwaitMethod();
        return methConfigureAwait is not null
            ? Expression.Call(awaitable, methConfigureAwait, continueOnCapturedContext ?? throw new ArgumentNullException(nameof(continueOnCapturedContext)))
            : ignoreMissingConfigureAwaitMethod
                ? awaitable
                : throw new ArgumentException($"The type {awaitable.Type} does not have a ConfigureAwait(bool) method");
    }
}
