using System.Linq.Expressions;
using Antlr4.Runtime;

namespace AltaSoft.Simpra.Visitor;

internal sealed partial class SimpraParserVisitor<TResult, TModel>
{
    private BinaryExpression HandleAssignment(Expression left, Expression right, ParserRuleContext context)
    {
        // Check for mutability
        if (!_compilerOptions.IsMutable && left.NodeType == ExpressionType.MemberAccess)
        {
            throw new SimpraException(context, $"Cannot mutate state of '{left}' when it's in immutable state");
        }

        if (left.Type == right.Type)
        {
            return Expression.Assign(left, right);
        }

        return Expression.Assign(left, ConvertToType(right, left.Type, context));
    }
}
