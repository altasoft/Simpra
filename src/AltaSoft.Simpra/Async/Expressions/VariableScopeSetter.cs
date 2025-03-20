using System;
using System.Linq;
using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Expressions;

internal sealed class VariableScopeSetter : ExpressionVisitor
{
    private readonly ILookup<BlockExpression?, ParameterExpression> _blockVariables;
    private readonly Func<ParameterExpression, bool> _unmanaged;
    private readonly Func<ParameterExpression, bool> _toRemove;

    public VariableScopeSetter(ILookup<BlockExpression?, ParameterExpression> blockVariables, Func<ParameterExpression, bool> unmanaged, Func<ParameterExpression, bool> toRemove)
    {
        _blockVariables = blockVariables;
        _unmanaged = unmanaged;
        _toRemove = toRemove;
    }

    protected override Expression VisitBinary(BinaryExpression node) => node is { NodeType: ExpressionType.Assign, Left: ParameterExpression variable } && _toRemove(variable)
            ? node.Right
            : base.VisitBinary(node);

    protected override Expression VisitBlock(BlockExpression node)
    {
        var block = node.Update(
            node.Variables.Where(_unmanaged).Concat(_blockVariables[node]),
            node.Expressions.Select(Visit).Where(x => x is not null)!);

        if (block.Variables.Count > 0)
        {
            return block;
        }

        // Eliminate now-redundant blocks
        return block.Expressions.Count switch
        {
            0 => Expression.Default(node.Type),
            1 => block.Expressions[0],
            _ => block
        };
    }
}
