using AltaSoft.Simpra.Tests.Models;
using static AltaSoft.Simpra.Tests.Models.TestModelFactory;

namespace AltaSoft.Simpra.Tests;

public class BuiltInFunctionsTests
{
    [Theory]
    // basic / within bounds
    [InlineData("USD", 1, 3, "USD")]   // from U
    [InlineData("USD", 1, 2, "US")]
    [InlineData("USD", 2, 2, "SD")]   // from S
    [InlineData("USD", 3, 1, "D")]    // from D

    // length clamping at the end
    [InlineData("USD", 1, 10003, "USD")]
    [InlineData("USD", 3, 10, "D")]

    // start beyond end
    [InlineData("USD", 1000, 1, "")]
    [InlineData("USD", 4, 1, "")]     // beyond "USD"

    // start exactly at end
    [InlineData("USD", 4, 0, "")]
    [InlineData("USD", 4, 10, "")]

    // zero length
    [InlineData("USD", 1, 0, "")]
    [InlineData("USD", 2, 0, "")]
    [InlineData("USD", 3, 0, "")]

    // negative/zero start ? clamp to 1
    [InlineData("USD", 0, 1, "U")]
    [InlineData("USD", -5, 2, "US")]
    [InlineData("USD", -2, 100, "USD")]

    // negative length ? empty
    [InlineData("USD", 1, -1, "")]
    [InlineData("USD", 3, -10, "")]
    [InlineData("USD", -2, -10, "")]

    // empty input
    [InlineData("", 1, 5, "")]
    [InlineData("", 10, 1, "")]
    [InlineData("", -3, 2, "")]
    [InlineData("", 1, 0, "")]

    // whitespace
    [InlineData(" ", 1, 1, " ")]
    [InlineData("   ", 2, 2, "  ")]
    [InlineData("   ", 2, 100, "  ")]

    // non-ASCII (safe checks with multi-byte chars)
    [InlineData("\u0410\u0411\u0412\u0413\u0414\u0415\u0416", 1, 2, "\u0410\u0411")]
    [InlineData("\u0410\u0411\u0412\u0413\u0414\u0415\u0416", 3, 3, "\u0412\u0413\u0414")]
    [InlineData("\u0410\u0411\u0412\u0413\u0414\u0415\u0416", 11, 5, "")]
    [InlineData("\u0410\u0411\u0412\u0413\u0414\u0415\u0416", -3, 100, "\u0410\u0411\u0412\u0413\u0414\u0415\u0416")]

    public void BuiltInFunction_Substring_EdgeCases(string input, int start, int length, string expected)
    {
        var simpra = new Simpra();
        var model = GetTestModel();
        model.Ccy = input;
        var expressionCode = $"return substring(Ccy,{start},{length})";
        var result = simpra.Execute<string, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void BuiltInFunction_Substring_ShouldMatchExamplesFromPrompt()
    {
        var simpra = new Simpra();
        var model = GetTestModel();
        model.Ccy = "USD";

        var result = simpra.Execute<string, TestModel, IFunctions>(model, new TestFunctions(), "return substring(Ccy,0,3)");
        Assert.Equal("USD", result);

        result = simpra.Execute<string, TestModel, IFunctions>(model, new TestFunctions(), "return substring(Ccy,0,10003)");
        Assert.Equal("USD", result);

        result = simpra.Execute<string, TestModel, IFunctions>(model, new TestFunctions(), "return substring(Ccy,1000,1)");
        Assert.Equal("", result);

        result = simpra.Execute<string, TestModel, IFunctions>(model, new TestFunctions(), "return substring(Ccy,1000,0)");
        Assert.Equal("", result);

        result = simpra.Execute<string, TestModel, IFunctions>(model, new TestFunctions(), "return substring(Ccy,1,0)");
        Assert.Equal("", result);
    }
    [Theory]
    // basic in-bounds
    [InlineData("USD", 1, "U")]
    [InlineData("USD", 2, "S")]
    [InlineData("USD", 3, "D")]

    // at/after end => empty
    [InlineData("USD", 4, "")]
    [InlineData("USD", 1000, "")]
    [InlineData("U", 2, "")]
    [InlineData("", 1, "")]
    [InlineData("", 5, "")]

    // zero/negative start ? clamp to 1
    [InlineData("USD", 0, "U")]
    [InlineData("USD", -1, "U")]
    [InlineData("USD", -5, "U")]
    [InlineData("", -3, "")]

    // whitespace
    [InlineData(" X ", 1, " ")]
    [InlineData(" X ", 2, "X")]
    [InlineData(" X ", 3, " ")]

    // non-ASCII (single UTF-16 code units)
    [InlineData("???????", 1, "?")]
    [InlineData("???????", 3, "?")]
    [InlineData("???????", 6, "?")]
    [InlineData("???????", 7, "?")]
    [InlineData("???????", 8, "")]

    // very large index
    [InlineData("USD", int.MaxValue, "")]
    public void BuiltInFunction_Substring_StartOnly_EdgeCases(string input, int start, string expected)
    {
        var simpra = new Simpra();
        var model = GetTestModel();
        model.Ccy = input;
        var expressionCode = $"return substring(Ccy,{start})";
        var result = simpra.Execute<string, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void BuiltInFunction_Substring_StartOnly_NullSource_ShouldBeEmpty()
    {
        var simpra = new Simpra();
        var model = GetTestModel();
        model.Ccy = null;

        var result = simpra.Execute<string, TestModel, IFunctions>(model, new TestFunctions(), "return substring(Ccy,1)");
        Assert.Null(result);
    }

    [Fact]
    public void BuiltInFunction_Substring_NullSource_ShouldBeNull()
    {
        // If your DSL defines a behavior for nulls, keep this.
        // If it should throw instead, change to Assert.Throws.
        var simpra = new Simpra();
        var model = GetTestModel();
        model.Ccy = null!;

        var result = simpra.Execute<string, TestModel, IFunctions>(model, new TestFunctions(), "return substring(Ccy,0,3)");
        Assert.Null(result);
    }

    [Fact]
    public void BuiltInFunction_Substring_ShouldReturnCorrectSubstringOfLength1()
    {
        const string expressionCode = "return substring(Ccy,1)";

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Ccy = "USD";

        var result = simpra.Execute<string, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal("U", result);
    }

    [Fact]
    public void BuiltInFunction_Round_WithDecimalsArgument_ShouldRoundToGivenPrecision()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return round(3.14159, 2)");
        Assert.Equal(3.14m, result);

        result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return round(3.14159, 3)");
        Assert.Equal(3.142m, result);

        result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return round(-3.14159, 2)");
        Assert.Equal(-3.14m, result);
    }

    [Fact]
    public void BuiltInFunction_Round_AtMidpoint_ShouldRoundToNearestEvenInteger()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return round(2.5)");
        Assert.Equal(2m, result);

        result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return round(3.5)");
        Assert.Equal(4m, result);
    }

    [Theory]
    [InlineData(-5, 5)]
    [InlineData(5, 5)]
    [InlineData(0, 0)]
    public void BuiltInFunction_Abs_ShouldReturnAbsoluteValue(int input, int expected)
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), $"return abs({input})");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void BuiltInFunction_Abs_WithFractionalValue_ShouldReturnAbsoluteValue()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return abs(-3.5)");
        Assert.Equal(3.5m, result);
    }

    [Fact]
    public void BuiltInFunction_String_FromNumber_ShouldReturnDecimalStringRepresentation()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), "return string(42.5)");
        Assert.Equal("42.5", result);
    }

    [Fact]
    public void BuiltInFunction_String_FromBool_ShouldReturnCapitalizedBooleanText()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), "return string(true)");
        Assert.Equal("True", result);
    }

    [Fact]
    public void BuiltInFunction_Number_FromString_ShouldParseDecimalValue()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return number('123.45')");
        Assert.Equal(123.45m, result);
    }

    [Fact]
    public void BuiltInFunction_Number_FromBool_ShouldReturnOneForTrueAndZeroForFalse()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return number(true)");
        Assert.Equal(1m, result);

        result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return number(false)");
        Assert.Equal(0m, result);
    }

    [Fact]
    public void BuiltInFunction_Number_FromNullString_ShouldReturnNullInsteadOfThrowing()
    {
        var simpra = new Simpra();
        var model = GetTestModel();
        model.Ccy = null;

        var result = simpra.Execute<decimal?, TestModel, TestFunctions>(model, new TestFunctions(), "return number(Ccy)");
        Assert.Null(result);
    }

    [Fact]
    public void BuiltInFunction_Date_FromString_ShouldParseCorrectDate()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<DateTime, TestModel, TestFunctions>(model, new TestFunctions(), "return date('2024-03-15')");
        Assert.Equal(new DateTime(2024, 3, 15), result);
    }

    [Fact]
    public void BuiltInFunction_Length_OfBooleanListLiteral_ShouldReturnElementCount()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return length([true, false, true])");
        Assert.Equal(3m, result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_XIsNotFive_And_LengthIsTwo()
    {
        const string expressionCode =
            """
            let X = '50'
            return X is not '5'
                and length(X) is 2;
            """;

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }
}
