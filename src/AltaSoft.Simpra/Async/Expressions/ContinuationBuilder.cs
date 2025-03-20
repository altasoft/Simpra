using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using AltaSoft.Simpra.Async.Collections;

namespace AltaSoft.Simpra.Async.Expressions;

internal sealed partial class ContinuationBuilder : ExpressionVisitor, IAsyncExpressionVisitor
{
    public enum FiberMode
    {
        Continuous,
        Finally
    }

    public readonly struct Fiber
    {
        public Fiber(MachineState entryState, MachineState exitState, Expression expression)
        {
            EntryState = entryState;
            ExitState = exitState;
            Expression = expression;
        }

        public MachineState EntryState
        {
            get;
        }

        public MachineState ExitState
        {
            get;
        }

        public Expression Expression
        {
            get;
        }

        public bool IsAsync => EntryState != ExitState;

        [Conditional("DEBUG")]
        internal void SetName(string kind, int groupId, string detail) => EntryState.SetName(kind, groupId, $"Fiber {detail}");

        public void ContinueWith(MachineState state)
        {
            AssignResult(state);
            ExitState.SetContinuation(state);
        }

        public void AssignResult(MachineState state) => ExitState.AddExpression(state.ResultExpression is ParameterExpression parameter
                ? Expression.Assign(parameter, Expression)
                : Expression);
    }

    private readonly IStateMachineVariables _vars;
    private readonly List<MachineState> _states = [];
    private readonly Dictionary<LabelTarget, MachineState> _labelStates = new(ReferenceEqualityComparer<LabelTarget>.s_default);
    private MachineState? _currentState;

    public ContinuationBuilder(IStateMachineVariables vars) => _vars = vars;

    public IReadOnlyCollection<MachineState> States => _states;

    // ReSharper disable once UnusedMember.Local
    public Fiber VisitAsFiber(Expression expression, FiberMode mode, ImmutableStack<TryInfo>? tryInfos = null)
    {
        if (_currentState is null)
        {
            throw new InvalidOperationException("No current state");
        }

        var originState = _currentState;
        try
        {
            var entryState = _currentState = mode == FiberMode.Continuous
                ? new MachineState(-1, _currentState.ResultExpression.Type, tryInfos ?? originState.TryInfos, false)
                : CreateState(_currentState.ResultExpression.Type, tryInfos ?? originState.TryInfos, mode == FiberMode.Finally);
            var exprVisited = Visit(expression);
            var exitState = _currentState;
            var fiber = new Fiber(entryState, exitState, exprVisited);
            // fiber.SetName("Unnamed", entryState.StateId, "");
            return fiber;
        }
        finally
        {
            _currentState = originState;
        }
    }

    private MachineState CreateState(Type result)
    {
        if (_currentState is null)
        {
            throw new InvalidOperationException("No current state");
        }
        var state = new MachineState(_states.Count, result, _currentState.TryInfos, false);
        _states.Add(state);
        return state;
    }

    private MachineState CreateState(Type result, ImmutableStack<TryInfo> tryInfos, bool finallyState = false)
    {
        var state = new MachineState(_states.Count, result, tryInfos, finallyState);
        _states.Add(state);
        return state;
    }

    // ReSharper disable once UnusedMember.Local
    private MachineState GetLabelState(LabelTarget target)
    {
        if (!_labelStates.TryGetValue(target, out var state))
        {
            state = CreateState(target.Type);
            state.SetName("Label Target", state.StateId, target.Name ?? "LabelTarget");
            _labelStates.Add(target, state);
        }
        return state;
    }

    public (MachineState state, Expression expr) Process(Expression node)
    {
        _states.Clear();
        _labelStates.Clear();
        _currentState = CreateState(typeof(void), ImmutableStack<TryInfo>.s_empty);
        _currentState.SetName("Entry", 0, "");
        var exprEnd = Visit(node);
        return (_currentState, exprEnd);
    }
}
