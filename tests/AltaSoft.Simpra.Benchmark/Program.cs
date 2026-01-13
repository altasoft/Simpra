using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace AltaSoft.Simpra.Benchmark;

public class SimpraBenchmark
{
    private readonly Simpra _simpra = new();
    private readonly string _simpraCode =
        """
        if Ccy is 'USD' then
            return Amount * 1.2
        else
            return Amount * 1.5
        end
        """;
    private readonly SimpraCompilerOptions _compilerOptions = new();
    private readonly DataModel _model = new();
    private readonly ExternalFunctions _externalFunctions = new();

    private readonly Func<DataModel, ExternalFunctions, bool> _func;
    private readonly Func<DataModel, ExternalFunctions, CancellationToken, Task<bool>> _asyncFunc;

    public SimpraBenchmark()
    {
        _func = _simpra.CompileCode<bool, DataModel, ExternalFunctions>(_simpraCode, _compilerOptions);
        _asyncFunc = _simpra.CompileAsyncCode<bool, DataModel, ExternalFunctions>(_simpraCode, _compilerOptions);
    }

    [Benchmark]
    public void BenchmarkCompileCode() => _simpra.CompileCode<bool, DataModel, ExternalFunctions>(_simpraCode, _compilerOptions);

    [Benchmark]
    public void BenchmarkExecute() => _simpra.Execute<bool, DataModel, ExternalFunctions>(_model, _externalFunctions, _simpraCode, _compilerOptions);

    [Benchmark]
    public void BenchmarkExecuteCompiled() => _ = _func.Invoke(_model, _externalFunctions);

    [Benchmark]
    public Task<bool> BenchmarkExecuteCompiledAsync() => _asyncFunc.Invoke(_model, _externalFunctions, CancellationToken.None);
}

internal sealed class ExternalFunctions
{
}

internal sealed class DataModel
{
    public string Ccy { get; set; } = "USD";
    public double Amount { get; set; } = 100;
}

public sealed class Program
{
    public static void Main(string[] args)
    {
        _ = BenchmarkRunner.Run<SimpraBenchmark>();
    }
}
