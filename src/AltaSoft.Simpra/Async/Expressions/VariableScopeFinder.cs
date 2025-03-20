using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AltaSoft.Simpra.Async.Collections;

namespace AltaSoft.Simpra.Async.Expressions;

internal sealed class VariableScopeFinder : ExpressionVisitor
{
    private readonly Dictionary<ParameterExpression, ImmutableStack<BlockExpression>> _variableBlock = new(ReferenceEqualityComparer<ParameterExpression>.s_default);
    private readonly HashSet<ParameterExpression> _ignore = new(ReferenceEqualityComparer<ParameterExpression>.s_default);
    private readonly HashSet<ParameterExpression> _isRead = new(ReferenceEqualityComparer<ParameterExpression>.s_default);
    private readonly HashSet<BlockExpression> _blockSet = new(ReferenceEqualityComparer<BlockExpression>.s_default);
    private ImmutableStack<BlockExpression> _blockStack = ImmutableStack<BlockExpression>.s_empty;

    public VariableScopeFinder(IEnumerable<ParameterExpression>? ignore)
    {
        if (ignore != null)
        {
            _ignore.UnionWith(ignore);
        }
        IsIgnored = _ignore.Contains;
    }

    public Func<ParameterExpression, bool> IsIgnored
    {
        get;
    }

    public bool IsToRemove(ParameterExpression variable) => _variableBlock.ContainsKey(variable) && !_isRead.Contains(variable);

    protected override Expression VisitBinary(BinaryExpression node)
    {
        var unread = node is { NodeType: ExpressionType.Assign, Left: ParameterExpression variable } &&
                     !_ignore.Contains(variable) && _isRead.Add(variable);
        try
        {
            return base.VisitBinary(node);
        }
        finally
        {
            if (unread)
            {
                _isRead.Remove((ParameterExpression)node.Left);
            }
        }
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (!_ignore.Contains(node))
        {
            if (_variableBlock.TryGetValue(node, out var stack))
            {
                while (!stack.IsEmpty && !_blockSet.Contains(stack.Peek()))
                {
                    stack = stack.Pop();
                }
                _variableBlock[node] = stack;
            }
            else
            {
                _variableBlock.Add(node, _blockStack);
            }
            _isRead.Add(node);
        }
        return node;
    }

    protected override CatchBlock VisitCatchBlock(CatchBlock node)
    {
        var ignored = node.Variable is not null && _ignore.Add(node.Variable);
        try
        {
            return node.Update(node.Variable, Visit(node.Filter), Visit(node.Body));
        }
        finally
        {
            if (ignored)
            {
                _ignore.Remove(node.Variable!);
            }
        }
    }

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        var ignored = node.Parameters.Where(_ignore.Add).ToList();
        try
        {
            return base.VisitLambda(node);
        }
        finally
        {
            _ignore.ExceptWith(ignored);
        }
    }

    protected override Expression VisitBlock(BlockExpression node)
    {
        _blockSet.Add(node);
        _blockStack = _blockStack.Push(node);
        try
        {
            // Note: variable declarations must not be processed
            foreach (var expression in node.Expressions)
            {
                Visit(expression);
            }
            return node;
        }
        finally
        {
            _blockSet.Remove(node);
            _blockStack = _blockStack.Pop();
        }
    }

    public ILookup<BlockExpression?, ParameterExpression> GetBlockVariables() => _variableBlock
            .Where(p => _isRead.Contains(p.Key))
            .ToLookup(p => p.Value.PeekOrDefault(), p => p.Key);
}
