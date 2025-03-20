using System;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Expressions;

internal sealed class StateMachineChecker : ExpressionVisitor, IAsyncExpressionVisitor
{
    private readonly bool _labelAndGotoAreStates;

    protected override Expression VisitLabel(LabelExpression node)
    {
        if (!_labelAndGotoAreStates)
            return base.VisitLabel(node);

        // Plain labels must act on the state machine
        RequiresStateMachine = true;
        return node;
    }

    protected override Expression VisitGoto(GotoExpression node)
    {
        if (_labelAndGotoAreStates && node.Kind is not (GotoExpressionKind.Break or GotoExpressionKind.Continue))
        {
            // Goto and Return must act on the state machine
            RequiresStateMachine = true;
            return node;
        }
        return base.VisitGoto(node);
    }

    public override Expression? Visit(Expression? node)
    {
        if (RequiresStateMachine)
        {
            // If we already know that we need a state machine, no need to look deeper
            return node;
        }
        return base.Visit(node);
    }

    public StateMachineChecker(bool labelAndGotoAreStates) => _labelAndGotoAreStates = labelAndGotoAreStates;

    public bool RequiresStateMachine { get; private set; }

    protected override Expression VisitLambda<T>(Expression<T> node) => node; // Don't traverse lambda expressions

    public Expression VisitAsyncLambda<TDelegate>(AsyncLambdaExpression<TDelegate> node) where TDelegate : Delegate => node; // Don't traverse lambda expressions

    public Expression VisitAwait(AwaitExpression node)
    {
        RequiresStateMachine = true;
        return node;
    }
}
