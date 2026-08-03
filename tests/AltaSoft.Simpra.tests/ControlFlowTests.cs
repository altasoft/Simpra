using AltaSoft.Simpra.Tests.Models;
using static AltaSoft.Simpra.Tests.Models.TestModelFactory;

namespace AltaSoft.Simpra.Tests;

public class ControlFlowTests
{
    [Fact]
    public void Expression_Should_EvaluateNestedWhenConditionAndReturnCorrectResult()
    {
        const string expressionCode =
            """
            let amount = Transfer.Amount
            let result = when amount < 50 then 'Low'
                         when amount >= 50 and amount < 200 then 'Medium'
                         else 'High'
            return result
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = 150;

        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("Medium", result);
    }

    [Fact]
    public void Expression_Should_ReturnEmptyString_When_StringContainsNoMatch()
    {
        const string expressionCode =
            """
            let str = 'foobar'
            return when str like 'f%' then str else ''
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("foobar", result);
    }

    [Fact]
    public void Expression_Should_HandleLargeNestedCondition()
    {
        const string expressionCode =
            """
            let x = Transfer.Amount
            let y = Transfer.Currency
            return when x < 50 then 'Low'
                   when x >= 50 and x <= 150 then 'Medium'
                   when x > 150 and y is 'USD' then 'High - USD'
                   when x > 150 and y is 'EUR' then 'High - EUR'
                   else 'Unknown'
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = 200;
        model.Transfer.Currency = "USD";

        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("High - USD", result);
    }

    [Fact]
    public void ExecuteExpression_Should_ReturnCorrectValue_FromNestedIfWithElse()
    {
        const string expressionCode =
            """
            let x = Transfer.Amount
            if x > 7 then
            return 10
            else if x > 5 then
            return 6
            else if x > 1 then
            return 2
            else
            return round(1000)
            end
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        model.Transfer!.Amount = 8;
        var result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(10, result);

        model.Transfer!.Amount = 6;
        result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(6, result);

        model.Transfer!.Amount = 2;
        result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(2, result);

        model.Transfer!.Amount = -1;
        result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(1000, result);
    }

    [Fact]
    public void ExecuteExpression_Should_ReturnCorrectValue_FromNestedIfWithoutElse()
    {
        const string expressionCode =
            """
            let x = Transfer.Amount
            if x > 7 then
            return 10
            if x > 5 then
            return 6
            if x > 1 then
            return 2
            else
            return 1000
            end
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = 8;
        var result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(10, result);

        model.Transfer!.Amount = 6;
        result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(6, result);

        model.Transfer!.Amount = 2;
        result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(2, result);

        model.Transfer!.Amount = -1;
        result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(1000, result);
    }

    [Fact]
    public void ExecuteExpression_Should_ReturnFirstReturnValue_When_MultipleReturnStatements()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        const string expressionCode =
            """
               return 10
               return 20
            """;
        var result = simpra.Execute<int, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(10, result);
    }

    [Fact]
    public void ExecuteExpression_WithMultipleReturnStatements_ShouldReturnLastValue()
    {
        var simpra = new Simpra();
        var model = new TestModelMain { Test = null };
        const string expression = """
                                  return 10
                                  return true
                                  """;

        var result = simpra.Execute<bool, TestModelMain, TestFunctions>(model, new TestFunctions(), expression);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnTwo_When_XIsSixty()
    {
        const string expressionCode =
            """
            let X = 50 + 10.0
            let Y = when X > 100 then 1 when X > 10 then 2 else 0
            return Y
            """;

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here

        var result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(2, result);
    }

    [Fact]
    public void Expression_Should_ReturnThree_When_bIsThirty()
    {
        const string expressionCode =
            """
            let b = 30;
            let x = 1;
            return when b is 1 then 1 else 3 end
            """;

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here

        var result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(3, result);
    }

    [Fact]
    public void Expression_Should_ReturnOne_When_XIsNotFive()
    {
        const string expressionCode =
            """
            let X = '50'
            return when X is not '5' then 1 else 0 end
            """;

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here
        var result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(1, result);
    }
}
