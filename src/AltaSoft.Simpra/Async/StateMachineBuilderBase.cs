using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using AltaSoft.Simpra.Async.Collections;

namespace AltaSoft.Simpra.Async;
#pragma warning disable CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
internal abstract class StateMachineBuilderBase : IStateMachineVariables
{
    protected static readonly ConstructorInfo CtorInvalidOperationExpression = typeof(InvalidOperationException).GetConstructor(Type.EmptyTypes)
        ?? throw new InvalidOperationException("Cannot get ctor of InvalidOperationException");

    private readonly ConcurrentDictionary<Type, ParameterExpression> _varAwaiter = new();

    protected StateMachineBuilderBase(Expressions.StateMachineLambdaExpression lambda, Type? breakType)
    {
        Lambda = lambda;
        VarState = Expression.Variable(typeof(int), "state");
        VarResumeState = Expression.Variable(typeof(int), "resumeState");
        VarException = Expression.Variable(typeof(Exception), "exception");
        LblBreak = Expression.Label(breakType ?? typeof(void), ":break");
    }

    public ParameterExpression GetVarAwaiter(Type awaiterType)
    {
        System.Diagnostics.Debug.Assert(awaiterType.IsAwaiter());
        return _varAwaiter.GetOrAdd(awaiterType, static t => Expression.Variable(t, "awaiter"));
    }

    public ParameterExpression VarException { get; }

    public ParameterExpression VarResumeState { get; }

    public ParameterExpression VarState { get; }

    ParameterExpression IStateMachineVariables.VarCurrent => VarCurrent ?? throw new InvalidOperationException("Yield Return is not supported here");

    public ParameterExpression? VarCurrent { get; init; }

    ParameterExpression IStateMachineVariables.VarContinuation => VarContinuation ?? throw new InvalidOperationException("Await is not supported here");

    public ParameterExpression? VarContinuation { get; init; }

    public LabelTarget LblBreak { get; }

    protected Expressions.StateMachineLambdaExpression Lambda { get; }

    protected virtual IEnumerable<ParameterExpression> GetVars() => _varAwaiter
            .Values
            .Append(VarState)
            .Append(VarResumeState)
            .Append(VarContinuation!)
            .Append(VarException);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected Expression StateBodyExpressionDebug(MachineState state, Expression? finallyOnly)
    {
        var result = StateBodyExpression(state, finallyOnly);
        return result;
    }

    private Expression StateBodyExpression(MachineState state, Expression? finallyOnly)
    {
        var bodyExpression = state.ToExpression(this);
        if (state.TryInfos.IsEmpty || bodyExpression.IsSafeCode())
        {
            return WrapForFinallyOnly(bodyExpression);
        }
        var finallyState = state.TryInfos.Peek().FinallyState;
        var rethrowState = state.TryInfos.Peek().RethrowState;
        var catchBlocks = new List<CatchBlock>();
        var catchBody = new List<Expression>();
        ParameterExpression varEx;
        var exceptionTypes = new TypeAssignableSet();
        for (var tryInfo = state.TryInfos; !tryInfo.IsEmpty; tryInfo = tryInfo.Pop())
        {
            foreach (var handler in tryInfo.Peek().Handlers)
            {
                if (handler.Filter is not null || exceptionTypes.Add(handler.Test))
                {
                    varEx = Expression.Variable(handler.Test, "ex");
                    catchBody.Clear();
                    catchBody.Add(Expression.Assign(VarException, varEx));
                    if (handler.Variable != null)
                    {
                        catchBody.Add(Expression.Assign(handler.Variable, varEx));
                    }
                    if (tryInfo == state.TryInfos || finallyState is null)
                    {
                        catchBody.Add(Expression.Assign(VarState, Expression.Constant(handler.BodyState.StateId)));
                    }
                    else
                    {
                        catchBody.Add(Expression.Assign(VarResumeState, Expression.Constant(handler.BodyState.StateId)));
                        catchBody.Add(Expression.Assign(VarState, Expression.Constant(finallyState.StateId)));
                    }
                    catchBlocks.Add(Expression.Catch(varEx, Expression.Block(catchBody), handler.Filter));
                }
            }
        }

        if (!exceptionTypes.Contains(typeof(Exception)))
        {
            varEx = Expression.Variable(typeof(Exception), "ex");
            catchBody.Clear();
            catchBody.Add(Expression.Assign(VarException, varEx));

            if (state.TryInfos.TryFirstNotNull(ti => ti.FinallyState, out finallyState))

            {
                catchBody.Add(Expression.Assign(VarResumeState, Expression.Constant(rethrowState.StateId)));
                catchBody.Add(Expression.Assign(VarState, Expression.Constant(finallyState.StateId)));
            }
            else
            {
                catchBody.Add(Expression.Assign(VarState, Expression.Constant(rethrowState.StateId)));
            }
            catchBlocks.Add(Expression.Catch(varEx, Expression.Block(catchBody)));
        }
        return WrapForFinallyOnly(Expression.MakeTry(typeof(void), bodyExpression, null, null, catchBlocks));

        Expression WrapForFinallyOnly(Expression expr) => finallyOnly != null && !state.FinallyState
                ? Expression.IfThenElse(
                    finallyOnly,
                    state.TryInfos.TryFirstNotNull(ti => ti.FinallyState, out var nextFinallyState)
                        ? Expression.Block(
                            Expression.Assign(VarResumeState, Expression.Constant(-1)),
                            Expression.Assign(VarState, Expression.Constant(nextFinallyState.StateId)))
                        : Expression.Assign(VarState, Expression.Constant(-1)),
                    expr)
                : expr;
    }
}
