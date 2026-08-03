using AltaSoft.Simpra.Tests.Models;
using static AltaSoft.Simpra.Tests.Models.TestModelFactory;

namespace AltaSoft.Simpra.Tests;

public class AsyncExecutionTests
{
    [Fact]
    public async Task ExecuteExpression_Should_ReturnTrue_When_CcyIsInListOfCurrencyCodes()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        const string expressionCode = "return Ccy in (ListOfCurrencyCodes('VisaB2B') + 'USD')";
        var result = await simpra.ExecuteAsync<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode, null, CancellationToken.None);
        Assert.True(result);
    }

    [Fact]
    public async Task Expression_Should_EvaluateSanctionedCountriesExpression_Correctly()
    {
        const string expressionCode =
            """
            let russianCountries = BigList('sanctioned_countries')
            let amount = Transfer.Amount
            let ccy = Transfer.Currency

            let isValidCcy = (ccy is '' or ccy not in ['USD', 'EUR']) and (ccy like 'I%' or ccy matches '[a-zA-Z_][a-zA-Z_0-9]')
            let isValidAmount = amount > 1000 and amount < 2000
            let isValidAmount2 = amount > 1000 and < 2000
            let isValidRemittance = length(Remittance) > 4

            return isValidCcy and isValidAmount and isValidRemittance
            """;

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here
        var result = await simpra.ExecuteAsync<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode,
            new SimpraCompilerOptions { MutabilityOption = MutabilityOption.Immutable, StringComparisonOption = StringComparisonOption.IgnoreCase },
            CancellationToken.None);
        Assert.False(result);
    }

    [Fact]
    public async Task ExecuteExpression_Should_EvaluateAsyncCallDirectlyWithinArithmeticBinary()
    {
        const string expressionCode = "return Compute(3, 4) + 1";

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = await simpra.ExecuteAsync<decimal, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode, null, CancellationToken.None);
        Assert.Equal(8m, result);
    }

    [Theory]
    [InlineData("Compute(1, 1) > 1 and Compute(3, 3) > 5", true)]   // left true -> right (async) must be evaluated
    [InlineData("Compute(1, 1) > 5 and Compute(3, 3) > 5", false)]  // left false -> AND short-circuits, right (async) never evaluated
    [InlineData("Compute(1, 1) > 5 or Compute(3, 3) > 5", true)]    // left false -> OR must evaluate right (async)
    [InlineData("Compute(1, 1) > 1 or Compute(3, 3) > 100", true)]  // left true -> OR short-circuits, right (async) never evaluated
    public async Task ExecuteExpression_Should_ShortCircuitCorrectly_When_AsyncCallIsOnRightOfAndOr(string condition, bool expected)
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = await simpra.ExecuteAsync<bool, TestModel, TestFunctions>(model, new TestFunctions(), $"return {condition}", null, CancellationToken.None);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task ExecuteExpression_Should_EvaluateAsyncCallWithinConditional()
    {
        const string expressionCode =
            """
            return when Compute(1, 1) > 1 then 'yes' else 'no' end
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = await simpra.ExecuteAsync<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode, null, CancellationToken.None);
        Assert.Equal("yes", result);
    }

    [Fact]
    public async Task ExecuteExpression_Should_EvaluateMultipleAsyncCallsCombinedInSingleBinaryExpression()
    {
        const string expressionCode = "return Compute(1, 2) + Compute(3, 4)";

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = await simpra.ExecuteAsync<decimal, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode, null, CancellationToken.None);
        Assert.Equal(10m, result);
    }
}
