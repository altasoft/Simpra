using System;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async;

internal interface IStateMachineVariables
{
    ParameterExpression VarException { get; }

    ParameterExpression VarResumeState { get; }

    ParameterExpression VarState { get; }

    LabelTarget LblBreak { get; }

    ParameterExpression VarContinuation { get; }

    // ReSharper disable once UnusedMember.Global
    ParameterExpression VarCurrent { get; }

    ParameterExpression GetVarAwaiter(Type awaiterType);
}
