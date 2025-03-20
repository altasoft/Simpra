using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AltaSoft.Simpra.Async.Expressions;

internal sealed class SafeCodeChecker : ExpressionVisitor
{
    private static readonly HashSet<ExpressionType> s_safeNodes =
    [
        ExpressionType.Assign,
        ExpressionType.AddAssign,
        ExpressionType.Add,
        ExpressionType.And,
        ExpressionType.AndAssign,
        ExpressionType.AndAlso,
        ExpressionType.Block,
        ExpressionType.Coalesce,
        ExpressionType.Conditional,
        ExpressionType.DebugInfo,
        ExpressionType.Constant,
        ExpressionType.Default,
        ExpressionType.Decrement,
        ExpressionType.Equal,
        ExpressionType.ExclusiveOr,
        ExpressionType.ExclusiveOrAssign,
        ExpressionType.GreaterThan,
        ExpressionType.GreaterThanOrEqual,
        ExpressionType.Increment,
        ExpressionType.IsTrue,
        ExpressionType.IsFalse,
        ExpressionType.TypeAs,
        ExpressionType.Label,
        ExpressionType.Goto,
        ExpressionType.LeftShift,
        ExpressionType.LeftShiftAssign,
        ExpressionType.LessThan,
        ExpressionType.LessThanOrEqual,
        ExpressionType.Loop,
        ExpressionType.Multiply,
        ExpressionType.MultiplyAssign,
        ExpressionType.Negate,
        ExpressionType.Not,
        ExpressionType.NotEqual,
        ExpressionType.OnesComplement,
        ExpressionType.Or,
        ExpressionType.OrElse,
        ExpressionType.OrAssign,
        ExpressionType.Parameter,
        ExpressionType.PostDecrementAssign,
        ExpressionType.PostIncrementAssign,
        ExpressionType.Power,
        ExpressionType.PreDecrementAssign,
        ExpressionType.PowerAssign,
        ExpressionType.PreIncrementAssign,
        ExpressionType.Quote,
        ExpressionType.RightShift,
        ExpressionType.RightShiftAssign,
        ExpressionType.Subtract,
        ExpressionType.SubtractAssign,
        ExpressionType.Switch,
        ExpressionType.TypeEqual,
        ExpressionType.TypeIs,
        ExpressionType.Try,
        ExpressionType.UnaryPlus,
        ExpressionType.Unbox
    ];

    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (node.Method is not null)
        {
            ContainsUnsafeCode = true;
            return node;
        }

        return node.Left is MemberExpression
        {
            Expression: ParameterExpression para,
            Member: FieldInfo
            {
                Name: nameof(StrongBox<object>.Value)
            } field
        }
            && para.Type.IsStrongBox()
            && field.DeclaringType.IsStrongBox()
            ? node.Update(node.Left, node.Conversion, Visit(node.Right) ?? throw new InvalidOperationException("Unexpected null expression"))
            : base.VisitBinary(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        if (node.Method != null)
        {
            ContainsUnsafeCode = true;
            return node;
        }
        return base.VisitUnary(node);
    }

    protected override Expression VisitSwitch(SwitchExpression node)
    {
        if (node.Comparison != null)
        {
            ContainsUnsafeCode = true;
            return node;
        }
        return base.VisitSwitch(node);
    }

    public override Expression? Visit(Expression? node)
    {
        if (node == null)
        {
            return null;
        }
        if (ContainsUnsafeCode)
        {
            return node;
        }
        if (!s_safeNodes.Contains(node.NodeType))
        {
            ContainsUnsafeCode = true;
            return node;
        }
        return base.Visit(node);
    }

    public bool ContainsUnsafeCode { get; private set; }
}
