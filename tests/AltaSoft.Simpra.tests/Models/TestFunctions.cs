namespace AltaSoft.Simpra.Tests.Models;

public class BaseFunctions
{
    public static string CallBaseStaticMethod() => "BaseStaticMethod";

    public static string CallBaseMethod() => "BaseMethod";
}

public interface IBaseFunctions
{
    string Lower(string str);
}

public interface IFunctions : IBaseFunctions
{
    string Upper(string str);
}

public class TestFunctions : BaseFunctions, IFunctions
{
    // ReSharper disable UnusedMember.Global
    public static string[] ListSomeCountries(string key)
    {
        return ["RU", "BE", key];
    }

    public static int[] ListOfCustomerIds(string key)
    {
        return [1, 2];
    }

#pragma warning disable S2325
    public string Upper(string str) => str.ToUpper();

    public string Lower(string str) => str.ToLower();

    public ValueTask<List<string>> ListOfCurrencyCodes(string name) => ValueTask.FromResult(new List<string>() { "EUR", "GEL" });

    public ValueTask<decimal> ComputeAsync(decimal a, decimal b) => ValueTask.FromResult(a + b);

    public static string DescribeAsInt(int value) => $"int:{value}";

    public string[] List(string key)
    {
        return ["RU", "BE"];
    }

    public ValueTask<string[]> BigListAsync(string key, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return ValueTask.FromResult(new[] { "RU", "BE" });
    }
#pragma warning restore S2325
    // ReSharper restore UnusedMember.Global
}
