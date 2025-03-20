using System;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Expressions;

internal sealed class AwaitExpression : Expression
{
    public AwaitExpression(Expression awaitable)
    {
        Awaitable = awaitable;
        var methGetAwaiter = Awaitable.Type.GetAwaitableGetAwaiterMethod()
            ?? throw new ArgumentException($"Type '{awaitable.Type}' is not awaitable", nameof(awaitable));
        var methGetResult = methGetAwaiter.ReturnType.GetAwaiterGetResultMethod()
            ?? throw new ArgumentException($"Type '{methGetAwaiter.ReturnType}' is not an awaiter", nameof(awaitable));
        Type = methGetResult.ReturnType;
    }

    public override ExpressionType NodeType => ExpressionType.Extension;

    public override Type Type { get; }

    public Expression Awaitable { get; }

    protected override Expression VisitChildren(ExpressionVisitor visitor) => Update(visitor.Visit(Awaitable));

    public AwaitExpression Update(Expression awaitable) => ReferenceEquals(awaitable, Awaitable)
            ? this
            : new AwaitExpression(awaitable);

    protected override Expression Accept(ExpressionVisitor visitor) => visitor is IAsyncExpressionVisitor asyncExpressionVisitor
            ? asyncExpressionVisitor.VisitAwait(this)
            : Update(visitor.Visit(Awaitable));
}
