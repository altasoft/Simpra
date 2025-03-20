using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

// ReSharper disable ObjectCreationAsStatement

namespace AltaSoft.Simpra.Async.Tasks;

internal sealed class ValueTaskCompletionSourceEmitter<TResult> : ICompletionSourceEmitter
{
    private static readonly ConstructorInfo s_ctorValueTaskCompletionSourceOfTResult = Reflect.GetConstructor(() => new ValueTaskCompletionSource<TResult>());
    private static readonly MethodInfo s_methValueTaskCompletionSourceOfTResultSetResult = Reflect<ValueTaskCompletionSource<TResult>>.GetMethod(vt => vt.SetResult(default!));
    private static readonly MethodInfo s_methValueTaskCompletionSourceOfTResultSetException = Reflect<ValueTaskCompletionSource<TResult>>.GetMethod(vt => vt.SetException(default!));
    private static readonly MethodInfo s_methValueTaskCompletionSourceOfTResultGetValueTask = Reflect<ValueTaskCompletionSource<TResult>>.GetMethod(vt => vt.GetValueTask());
    private static readonly ConstructorInfo s_ctorValueTaskOfTResultResult = Reflect.GetConstructor(() => new ValueTask<TResult>(default(TResult)!));
    private static readonly ConstructorInfo s_ctorValueTaskOfTResultTask = Reflect.GetConstructor(() => new ValueTask<TResult>(default(Task<TResult>)!));
    private static readonly MethodInfo s_methTaskFromExceptionOfTResult = Reflect.GetStaticMethod(() => Task.FromException<TResult>(default!));

    public Expression Create() => Expression.New(s_ctorValueTaskCompletionSourceOfTResult);

    public Expression SetResult(ParameterExpression varCompletionSource, Expression result) => Expression.Call(varCompletionSource, s_methValueTaskCompletionSourceOfTResultSetResult, result);

    public Expression SetException(ParameterExpression varCompletionSource, Expression exception) => Expression.Call(varCompletionSource, s_methValueTaskCompletionSourceOfTResultSetException, exception);

    public Expression GetAwaitable(ParameterExpression varCompletionSource) => Expression.Call(varCompletionSource, s_methValueTaskCompletionSourceOfTResultGetValueTask);

    public Expression GetFromResult(Expression result) => Expression.New(s_ctorValueTaskOfTResultResult, result);

    public Expression GetFromException(Expression exception) => Expression.New(s_ctorValueTaskOfTResultTask,
            Expression.Call(s_methTaskFromExceptionOfTResult, exception));
}

internal sealed class ValueTaskCompletionSourceEmitter : ICompletionSourceEmitter
{
    private static readonly ConstructorInfo s_ctorValueTaskCompletionSource = Reflect.GetConstructor(() => new ValueTaskCompletionSource());
    private static readonly MethodInfo s_methValueTaskCompletionSourceSetResult = Reflect<ValueTaskCompletionSource>.GetMethod(vt => vt.SetResult());
    private static readonly MethodInfo s_methValueTaskCompletionSourceSetException = Reflect<ValueTaskCompletionSource>.GetMethod(vt => vt.SetException(default!));
    private static readonly MethodInfo s_methValueTaskCompletionSourceGetValueTask = Reflect<ValueTaskCompletionSource>.GetMethod(vt => vt.GetValueTask());
    private static readonly ConstructorInfo s_ctorValueTaskTask = Reflect.GetConstructor(() => new ValueTask(default!));
    private static readonly MethodInfo s_methTaskFromException = Reflect.GetStaticMethod(() => Task.FromException<object>(default!));

    public Expression Create() => Expression.New(s_ctorValueTaskCompletionSource);

    public Expression SetResult(ParameterExpression varCompletionSource, Expression result) => Expression.Call(varCompletionSource, s_methValueTaskCompletionSourceSetResult);

    public Expression SetException(ParameterExpression varCompletionSource, Expression exception) => Expression.Call(varCompletionSource, s_methValueTaskCompletionSourceSetException, exception);

    public Expression GetAwaitable(ParameterExpression varCompletionSource) => Expression.Call(varCompletionSource, s_methValueTaskCompletionSourceGetValueTask);

    public Expression GetFromResult(Expression result) => Expression.Block(
            result,
            Expression.Default(typeof(ValueTask)));

    public Expression GetFromException(Expression exception) => Expression.New(s_ctorValueTaskTask,
            Expression.Call(s_methTaskFromException, exception));
}
