using System;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async;

internal readonly struct CatchInfo
{
    public CatchInfo(MachineState bodyState, ParameterExpression? variable, Type test, Expression? filter)
    {
        BodyState = bodyState;
        Variable = variable;
        Test = test;
        Filter = filter;
    }

    public MachineState BodyState { get; }

    public ParameterExpression? Variable { get; }

    public Type Test { get; }

    public Expression? Filter { get; }
}
