using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using AltaSoft.Simpra.Async.Collections;

namespace AltaSoft.Simpra.Async.Expressions;

internal sealed class AsyncLambdaExpression<TDelegate> : StateMachineLambdaExpression<TDelegate> where TDelegate : Delegate
{
    public AsyncLambdaExpression(string? name, Expression body, ReadOnlyCollection<ParameterExpression> parameters) : base(name, body, parameters) { }

    public override ExpressionType NodeType => ExpressionType.Extension;

    protected override Expression VisitChildren(ExpressionVisitor visitor) => Update(visitor.Visit(Body), Parameters.Select(p => visitor.VisitAndConvert(p, null)));

    public override Expression<TDelegate> BuildLambdaExpression()
    {
        var body = new AsyncStateMachineBuilder(this, typeof(TDelegate).GetDelegateInvokeMethod().ReturnType).CreateStateMachineBody();
        return Lambda<TDelegate>(body, Name, Parameters);
    }

    public AsyncLambdaExpression<TDelegate> Update(Expression body, IEnumerable<ParameterExpression>? parameters)
    {
        var parameterExpressions = (parameters ?? []).AsReadOnlyCollection();
        return body == Body && (ReferenceEquals(parameterExpressions, Parameters) || parameterExpressions.SequenceEqual(Parameters)) ? this : new AsyncLambdaExpression<TDelegate>(Name, body, parameterExpressions);
    }

    protected override Expression Accept(ExpressionVisitor visitor) => visitor is IAsyncExpressionVisitor asyncVisitor
            ? asyncVisitor.VisitAsyncLambda(this)
            : Update(visitor.Visit(Body), visitor.VisitAndConvert(Parameters, nameof(Accept)));
}
