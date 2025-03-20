using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AltaSoft.Simpra.Async.Expressions;
using AltaSoft.Simpra.Async.Tasks;

namespace AltaSoft.Simpra.Async;

internal sealed class AsyncStateMachineBuilder : StateMachineBuilderBase
{
    private ICompletionSourceEmitter CompletionSourceEmitter { get; }

    public Expression ExprCreateCompletionSource { get; init; }

    public ParameterExpression VarCompletionSource { get; init; }
    public Expression CreateStateMachineBody()
    {
        var emitter = CompletionSourceEmitterFactory.Get(Lambda.ReturnType);

        var varEx = Expression.Variable(typeof(Exception), "ex");
        var continuationBuilder = new ContinuationBuilder(this);
        var (finalState, finalExpr) = continuationBuilder.Process(Lambda.Body);
        if (finalState.StateId == 0)
        {
            // Nothing async, just wrap into a Task or ValueTask
            return Expression.TryCatch( // return ValueTask
                    emitter.GetFromResult(Lambda.Body),
                    Expression.Catch(varEx,
                        emitter.GetFromException(varEx)))
                .Optimize();
        }
        finalState.AddExpression(emitter.SetResult(VarCompletionSource, finalExpr));
        finalState.AddExpression(Expression.Break(LblBreak));

        var variables = GetVars().ToList();
        Expression stateMachine = Expression.Block(Lambda.ReturnType, variables,
            Expression.Assign(VarCompletionSource, ExprCreateCompletionSource),
            Expression.Assign(VarContinuation!,
                Expression.Lambda<Action>(Expression.TryCatch(
                    Expression.Loop(
                        Expression.Switch(typeof(void), VarState,
                            Expression.Throw(
                                Expression.New(CtorInvalidOperationExpression)),
                            null,
                            continuationBuilder.States.Select(state =>
                                Expression.SwitchCase(
                                    StateBodyExpressionDebug(state, null),
                                    Expression.Constant(state.StateId)))), LblBreak),
                    Expression.Catch(varEx,
                        HandleException(varEx))))),
            Expression.Invoke(VarContinuation!),
            emitter.GetAwaitable(VarCompletionSource));
        stateMachine = stateMachine.Optimize();
        return stateMachine.ReScopeVariables(Lambda.Parameters.Concat(variables));
    }

    public Expression HandleException(ParameterExpression varException) => CompletionSourceEmitter.SetException(VarCompletionSource, varException);

    public AsyncStateMachineBuilder(StateMachineLambdaExpression lambda, Type resultType) : base(lambda, typeof(void))
    {
        try
        {
            CompletionSourceEmitter = CompletionSourceEmitterFactory.Get(resultType);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Only Task<>, Task, ValueTask<> and ValueTask are supported as return types", ex);
        }
        ExprCreateCompletionSource = CompletionSourceEmitter.Create();
        VarCompletionSource = Expression.Variable(ExprCreateCompletionSource.Type, "completionSource");
        VarContinuation = Expression.Variable(typeof(Action), "continuation");
    }

    protected override IEnumerable<ParameterExpression> GetVars() => base.GetVars().Append(VarCompletionSource);
}
