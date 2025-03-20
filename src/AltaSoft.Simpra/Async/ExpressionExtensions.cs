using System.Collections.Generic;
using System.Linq.Expressions;
using AltaSoft.Simpra.Async.Expressions;

namespace AltaSoft.Simpra.Async;

internal static class ExpressionExtensions
{
    internal static Expression ReScopeVariables(this Expression expression, IEnumerable<ParameterExpression> unmanagedVariablesAndParameters)
    {
        var finder = new VariableScopeFinder(unmanagedVariablesAndParameters);
        finder.Visit(expression);
        var setter = new VariableScopeSetter(finder.GetBlockVariables(), finder.IsIgnored, finder.IsToRemove);
        return setter.Visit(expression);
    }

    internal static Expression Optimize(this Expression expression)
    {
        var optimizer = new Optimizer();
        return optimizer.Visit(expression) ?? Expression.Empty();
    }

    internal static bool RequiresStateMachine(this Expression expression, bool labelAndGotoAreAsync)
    {
        var awaitCallChecker = new StateMachineChecker(labelAndGotoAreAsync);
        awaitCallChecker.Visit(expression);
        return awaitCallChecker.RequiresStateMachine;
    }

    internal static bool IsSafeCode(this Expression expression)
    {
        var safeCodeChecker = new SafeCodeChecker();
        safeCodeChecker.Visit(expression);
        return !safeCodeChecker.ContainsUnsafeCode;
    }
}
