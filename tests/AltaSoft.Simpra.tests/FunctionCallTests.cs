using AltaSoft.Simpra.Tests.Models;
using static AltaSoft.Simpra.Tests.Models.TestModelFactory;

namespace AltaSoft.Simpra.Tests;

public class FunctionCallTests
{
    [Fact]
    public void CallInterfaceFunctionFromSimpra_ShouldReturnCorrectValue()
    {
        const string expressionCode =
            """
            return Upper('test')
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<string, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal("TEST", result);
    }

    [Fact]
    public void CallBaseInterfaceFunctionFromSimpra_ShouldReturnCorrectValue()
    {
        const string expressionCode =
            """
            return Lower('TEST')
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<string, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal("test", result);
    }

    [Fact]
    public void CallBaseStaticFunctionFromSimpra_ShouldReturnCorrectValue()
    {
        const string expressionCode =
            """
            return CallBaseStaticMethod()
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal("BaseStaticMethod", result);
    }

    [Fact]
    public void CallBaseFunctionFromSimpra_ShouldReturnCorrectValue()
    {
        const string expressionCode =
            """
            return CallBaseMethod()
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal("BaseMethod", result);
    }

    [Fact]
    public void CallFunctionFromSimpra()
    {
        const string expressionCode =
            """
            return ListSomeCountries('GE')
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<List<string>, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.True(result.SequenceEqual(["RU", "BE", "GE"]));
    }

    [Fact]
    public void Expression_Should_HandleNestedFunctionCallsCorrectly()
    {
        const string expressionCode =
            """
            let str = 'hello world'
            let upperStr = Upper(str)
            return Lower(upperStr) is str
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void CallExternalFunction_WithNonDecimalNumericParameter_ShouldSelectOverloadAndConvertArgument()
    {
        const string expressionCode = "return DescribeAsInt(7)";

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal("int:7", result);
    }
}
