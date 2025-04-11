using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AltaSoft.Simpra.Async.Expressions;
using AltaSoft.Simpra.Visitor;
using Antlr4.Runtime;

namespace AltaSoft.Simpra;
/// <summary>
/// Entry point for compiling and executing Simpra DSL code with support for synchronous and asynchronous evaluation,
/// external function injection, and customizable compiler options.
/// </summary>
public class Simpra
{
    private const string RootParameterName = "_m";
    private const string ExternalFunctionsParameterName = "_f";

    /// <summary>
    /// Executes the Simpra code synchronously using the specified data model and compiler options.
    /// </summary>
    public TResult Execute<TResult, TDataModel>(TDataModel model, string simpraCode, SimpraCompilerOptions? compilerOptions)
        where TDataModel : class
    {
        var @delegate = CompileCode<TResult, TDataModel>(simpraCode, compilerOptions);
        return @delegate(model);
    }
    /// <summary>
    /// Executes the Simpra code asynchronously using the specified data model and compiler options.
    /// </summary>
    public Task<TResult> ExecuteAsync<TResult, TDataModel>(TDataModel model, string simpraCode, SimpraCompilerOptions? compilerOptions, CancellationToken cancellationToken)
        where TDataModel : class
    {
        var @delegate = CompileAsyncCode<TResult, TDataModel>(simpraCode, compilerOptions);
        return @delegate(model, cancellationToken);
    }
    /// <summary>
    /// Executes the Simpra code synchronously using the specified data model, external functions, and compiler options.
    /// </summary>
    public TResult Execute<TResult, TDataModel, TExternalFunctions>(TDataModel model, TExternalFunctions externalFunctions, string simpraCode, SimpraCompilerOptions? compilerOptions = default)
        where TDataModel : class
        where TExternalFunctions : class
    {
        var @delegate = CompileCode<TResult, TDataModel, TExternalFunctions>(simpraCode, compilerOptions);
        return @delegate(model, externalFunctions);
    }

    /// <summary>
    /// Executes the Simpra code asynchronously using the specified data model, external functions, and compiler options.
    /// </summary>
    public Task<TResult> ExecuteAsync<TResult, TDataModel, TExternalFunctions>(TDataModel model, TExternalFunctions externalFunctions, string simpraCode, SimpraCompilerOptions? compilerOptions = default, CancellationToken cancellationToken = default)
       where TDataModel : class
       where TExternalFunctions : class
    {
        var @delegate = CompileAsyncCode<TResult, TDataModel, TExternalFunctions>(simpraCode, compilerOptions);
        return @delegate(model, externalFunctions, cancellationToken);
    }
    /// <summary>
    /// Compiles Simpra code into a delegate for synchronous execution using the specified data model and compiler options.
    /// </summary>
    public Func<TDataModel, TResult> CompileCode<TResult, TDataModel>(string simpraCode, SimpraCompilerOptions? compilerOptions)
        where TDataModel : class
    {
        var expression = Generate<TResult, TDataModel>(simpraCode, compilerOptions, null, out var rootParameter, out _, out var _);

        var lambda = Expression.Lambda<Func<TDataModel, TResult>>(expression, rootParameter);

        return lambda.Compile();
    }
    /// <summary>
    /// Compiles Simpra code into a delegate for synchronous execution using the specified data model, external functions, and compiler options.
    /// </summary>
    public Func<TDataModel, TExternalFunctions, TResult> CompileCode<TResult, TDataModel, TExternalFunctions>(string simpraCode, SimpraCompilerOptions? compilerOptions)
        where TDataModel : class
        where TExternalFunctions : class
    {
        var expression = Generate<TResult, TDataModel>(simpraCode, compilerOptions, typeof(TExternalFunctions), out var rootParameter, out var externalFunctionsParameter, out var _);

        Debug.Assert(externalFunctionsParameter is not null);

        var lambda = Expression.Lambda<Func<TDataModel, TExternalFunctions, TResult>>(expression, rootParameter, externalFunctionsParameter);

        return lambda.Compile();
    }
    /// <summary>
    /// Compiles Simpra code into a delegate for asynchronous execution using the specified data model and compiler options.
    /// </summary>
    public Func<TDataModel, CancellationToken, Task<TResult>> CompileAsyncCode<TResult, TDataModel>(string simpraCode, SimpraCompilerOptions? compilerOptions)
        where TDataModel : class
    {
        var expression = Generate<TResult, TDataModel>(simpraCode, compilerOptions, null, out var rootParameter, out _, out var cancellationTokenParam);

        var lambda = AsyncExpression.AsyncLambda<Func<TDataModel, CancellationToken, Task<TResult>>>(expression, rootParameter, cancellationTokenParam);

        return lambda.Compile();
    }
    /// <summary>
    /// Compiles Simpra code into a delegate for asynchronous execution using the specified data model, external functions, and compiler options.
    /// </summary>
    public Func<TDataModel, TExternalFunctions, CancellationToken, Task<TResult>> CompileAsyncCode<TResult, TDataModel, TExternalFunctions>(string simpraCode, SimpraCompilerOptions? compilerOptions)
        where TDataModel : class
        where TExternalFunctions : class
    {
        var expression = Generate<TResult, TDataModel>(simpraCode, compilerOptions, typeof(TExternalFunctions), out var rootParameter, out var externalFunctionsParameter, out var cancellationTokenParam);

        var lambda = AsyncExpression.AsyncLambda<Func<TDataModel, TExternalFunctions, CancellationToken, Task<TResult>>>(expression, rootParameter, externalFunctionsParameter!, cancellationTokenParam);

        return lambda.Compile();
    }

#if DEBUG
    // For testing
    public Expression Generate<TResult, TDataModel>(string simpraCode, SimpraCompilerOptions? compilerOptions, Type externalFunctionsType)
        where TDataModel : class
    {
        return Generate<TResult, TDataModel>(simpraCode, compilerOptions, externalFunctionsType, out _, out _, out _);
    }
#endif

    private static Expression Generate<TResult, TDataModel>(
        string simpraCode,
        SimpraCompilerOptions? compilerOptions,
        Type? externalFunctionsType,
        out ParameterExpression rootParameter,
        out ParameterExpression? externalFunctionsParameter,
        out ParameterExpression cancellationTokenParam)
        where TDataModel : class
    {
        var expression = Parse<TResult, TDataModel>(simpraCode, compilerOptions, externalFunctionsType,
            out rootParameter, out externalFunctionsParameter, out cancellationTokenParam);

        while (expression.CanReduce)
        {
            expression = expression.Reduce();
        }
        return expression;
    }

    private static Expression Parse<TResult, TDataModel>(string simpraCode, SimpraCompilerOptions? compilerOptions,
        Type? externalFunctionsType,
        out ParameterExpression rootParameter, out ParameterExpression? externalFunctionsParameter, out ParameterExpression cancellationTokenParam)
        where TDataModel : class
    {
        rootParameter = Expression.Parameter(typeof(TDataModel), RootParameterName);
        externalFunctionsParameter = externalFunctionsType is null ? null : Expression.Parameter(externalFunctionsType, ExternalFunctionsParameterName);
        cancellationTokenParam = Expression.Parameter(typeof(CancellationToken), "cancellationToken");

        var lexer = new SimpraLexer(new AntlrInputStream(simpraCode));
        var tokens = new CommonTokenStream(lexer);
        var parser = new SimpraParser(tokens);

        var tree = parser.program();
        var visitor = new SimpraParserVisitor<TResult, TDataModel>(compilerOptions, rootParameter, externalFunctionsParameter, cancellationTokenParam);
        return visitor.Visit(tree);
    }
}
