using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

// ReSharper disable ObjectCreationAsStatement

namespace AltaSoft.Simpra.Async.Tasks;

internal class TaskCompletionSourceEmitter<TResult> : ICompletionSourceEmitter
{
    private static readonly ConstructorInfo s_ctorTaskCompletionSourceOfTResult = Reflect.GetConstructor(() => new TaskCompletionSource<TResult>(default));
    private static readonly MethodInfo s_methTaskCompletionSourceOfTResultSetResult = Reflect<TaskCompletionSource<TResult>>.GetMethod(tcs => tcs.SetResult(default!));
    private static readonly MethodInfo s_methTaskCompletionSourceOfTResultSetException = Reflect<TaskCompletionSource<TResult>>.GetMethod(tcs => tcs.SetException(default(Exception)!));
    private static readonly PropertyInfo s_propTaskCompletionSourceOfTResultTask = Reflect<TaskCompletionSource<TResult>>.GetProperty(tcs => tcs.Task);
    internal static readonly MethodInfo s_methTaskFromResult = Reflect.GetStaticMethod(() => Task.FromResult<TResult>(default!));
    private static readonly MethodInfo s_methTaskFromException = Reflect.GetStaticMethod(() => Task.FromException<TResult>(default!));

    public Expression Create() => Expression.New(s_ctorTaskCompletionSourceOfTResult,
            Expression.Constant(TaskCreationOptions.RunContinuationsAsynchronously));

    public virtual Expression SetResult(ParameterExpression varCompletionSource, Expression result) => Expression.Call(varCompletionSource, s_methTaskCompletionSourceOfTResultSetResult, result);

    public Expression SetException(ParameterExpression varCompletionSource, Expression exception) => Expression.Call(varCompletionSource, s_methTaskCompletionSourceOfTResultSetException, exception);

    public virtual Expression GetAwaitable(ParameterExpression varCompletionSource) => Expression.Property(varCompletionSource, s_propTaskCompletionSourceOfTResultTask);

    public virtual Expression GetFromResult(Expression result) => Expression.Call(s_methTaskFromResult, result);

    public virtual Expression GetFromException(Expression exception) => Expression.Call(s_methTaskFromException, exception);
}

internal sealed class TaskCompletionSourceEmitter : TaskCompletionSourceEmitter<object>
{
    private static readonly PropertyInfo s_propTaskCompletedTask = Reflect.GetStaticProperty(() => Task.CompletedTask);
    private static readonly MethodInfo s_methTaskFromException = Reflect.GetStaticMethod(() => Task.FromException(default!));

    public override Expression SetResult(ParameterExpression varCompletionSource, Expression result) => base.SetResult(varCompletionSource, Expression.Default(typeof(object)));

    public override Expression GetAwaitable(ParameterExpression varCompletionSource) => Expression.Convert(base.GetAwaitable(varCompletionSource), typeof(Task));

    public override Expression GetFromResult(Expression result) => Expression.Block(
            result,
            Expression.Property(null, s_propTaskCompletedTask));

    public override Expression GetFromException(Expression exception) => Expression.Call(s_methTaskFromException, exception);
}
