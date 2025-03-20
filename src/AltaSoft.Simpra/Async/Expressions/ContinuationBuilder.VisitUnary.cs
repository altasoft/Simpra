using System.Linq.Expressions;

namespace AltaSoft.Simpra.Async.Expressions;

internal partial class ContinuationBuilder
{
    protected override Expression VisitUnary(UnaryExpression node)
    {
        if (node is { NodeType: ExpressionType.Throw, Operand: null })
        {
            // Expression.Rethrow() has a null operand
            return node.Update(_vars.VarException);
        }
        return base.VisitUnary(node);
    }
}
