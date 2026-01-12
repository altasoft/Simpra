namespace AltaSoft.Simpra.Tests;

public class UnitTestsSmartPurposeModel
{
    [Fact]
    public void ExecuteExpression_ReturnsExpectedResult()
    {
        const string expressionCode = "return P['Key1'] + 'A'";

        var simpra = new Simpra();
        var model = new SmartPurposeModel();
        model.SetValue("Key1", "Value1");

        var result = simpra.Execute<string, SmartPurposeModel>(model, expressionCode, null);
        Assert.Equal("Value1A", result);
    }
}

public sealed class SmartPurposeModel
{
    public Dictionary<string, string> P { get; } = new();

    public void SetValue(string key, string? value)
    {
        P[key] = value ?? string.Empty;
    }

    public string GetValue(string key) => P.TryGetValue(key, out var result) ? result : string.Empty;
}
