using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AltaSoft.Simpra.Async.Expressions;

internal abstract class StateMachineLambdaExpression : Expression
{
    internal StateMachineLambdaExpression(string? name, Expression body, ReadOnlyCollection<ParameterExpression> parameters)
    {
        Name = name;
        Body = body;
        Parameters = parameters;
    }

    /// <summary>Gets the parameters of the lambda expression.</summary>
    /// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects that represent the parameters of the lambda expression.</returns>
    public ReadOnlyCollection<ParameterExpression> Parameters { get; }

    /// <summary>Gets the name of the lambda expression.</summary>
    /// <returns>The name of the lambda expression.</returns>
    public string? Name { get; }

    /// <summary>Gets the body of the lambda expression.</summary>
    /// <returns>A <see cref="T:System.Linq.Expressions.Expression" /> that represents the body of the lambda expression.</returns>
    public Expression Body { get; }

    /// <summary>Gets the return type of the lambda expression.</summary>
    /// <returns>The <see cref="T:System.Type" /> object representing the type of the lambda expression.</returns>
    public Type ReturnType => Type.GetDelegateInvokeMethod().ReturnType;
}

internal abstract class StateMachineLambdaExpression<TDelegate> : StateMachineLambdaExpression where TDelegate : Delegate
{
    internal StateMachineLambdaExpression(string? name, Expression body, ReadOnlyCollection<ParameterExpression> parameters) : base(name, body, parameters) { }

    public sealed override Type Type => typeof(TDelegate);

    public override bool CanReduce => true;

    public override Expression Reduce() => BuildLambdaExpression();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TDelegate Compile() => (TDelegate)CompileInternal();

    protected virtual Delegate CompileInternal()
    {
        var lambda = BuildLambdaExpression();
        return lambda.Compile();
    }

    public abstract Expression<TDelegate> BuildLambdaExpression();
}
