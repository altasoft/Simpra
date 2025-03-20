using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using AltaSoft.Simpra.Async.Collections;

namespace AltaSoft.Simpra.Async;

internal sealed class MachineState
{
    private readonly List<Expression> _expressions = new(1);
    private readonly List<ParameterExpression> _variables = new(1);

    public bool FinallyState { get; }

#if DEBUG
    internal string? Name { get; private set; }
#endif
    [Conditional("DEBUG")]
    internal void SetName(string kind, int groupId, string detail)
    {
#if DEBUG
        Debug.Assert(string.IsNullOrEmpty(Name));
        Name = $"{kind} {groupId} {detail}".TrimEnd();
#endif
    }

    public ImmutableStack<TryInfo> TryInfos { get; }

    public int StateId { get; }

    public Expression ResultExpression { get; }

    public MachineState? Continuation { get; private set; }

    public MachineState(int stateId, Type? result, ImmutableStack<TryInfo> tryInfos, bool finallyState)
    {
        TryInfos = tryInfos;
        FinallyState = finallyState;
        StateId = stateId;
        if (result is null || result == typeof(void))
        {
            ResultExpression = Expression.Empty();
        }
        else
        {
            var varResult = Expression.Variable(result, $"result:{stateId}");
            ResultExpression = varResult;
            _variables.Add(varResult);
        }
    }

    public void SetContinuation(MachineState state)
    {
        Debug.Assert(Continuation == null);
        Continuation = state;
    }

    public void AddExpression(Expression expression) => _expressions.Add(expression);

    public BlockExpression ToExpression(IStateMachineVariables vars)
    {
        Debug.Assert(vars != null);
        var expressions = _expressions.ToList();
        if (Continuation != null)
        {
            expressions.Insert(0,
                Expression.Assign(
                    vars.VarState,
                    Expression.Constant(Continuation.StateId)));
            if (Continuation == TryInfos.PeekOrDefault().FinallyState)
            {
                expressions.Insert(0,
                    Expression.Assign(
                        vars.VarResumeState,
                        Expression.Constant(TryInfos.Peek().ExitState.StateId)));
            }
        }
        return Expression.Block(_variables, expressions);
    }

    public override string ToString() => Continuation == null ? StateId.ToString(CultureInfo.InvariantCulture) : $"{StateId} => {Continuation.StateId}";
}
