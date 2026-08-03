using AltaSoft.Simpra.Tests.Models;
using static AltaSoft.Simpra.Tests.Models.TestModelFactory;

namespace AltaSoft.Simpra.Tests;

public class CompositeExpressionTests
{
    [Fact]
    public void Expression_Should_ReturnMultipleValues_When_AggregateListValues()
    {
        const string expressionCode =
            """
            let values = [1, 2, 3, 4, 5]
            let sum = sum(values)
            let avg = sum(values) / length(values)
            return sum + avg
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(18m, result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_AmountIsGreaterThanMaxOfList()
    {
        const string expressionCode =
            """
            let amounts = [10, 20, 30, 40]
            let maxAmount = amounts[3]
            return Transfer.Amount > maxAmount
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = 50;

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_CheckingForNullValue()
    {
        const string expressionCode =
            """
            let transfer = Transfer
            return transfer.Currency has value
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Currency = "USD";

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_AmountInCurrencyIsGreaterThanThresholdAndMatchesPattern()
    {
        const string expressionCode =
            """
            let transfer = Transfer
            let amount = transfer.Amount
            let ccy = transfer.Currency
            return amount > 100 and (ccy matches '^[A-Z]{3}$') and ccy is 'USD'
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = 150;
        model.Transfer.Currency = "USD";

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnFalse_When_DividingByZeroHandledProperly()
    {
        const string expressionCode =
            """
            let transfer = Transfer
            let amount = transfer.Amount
            let divisor = 0
            let safeDivision = when divisor is not 0 then amount / divisor else 0 end
            return safeDivision is 0
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_DynamicStringListContainsMatchingItem()
    {
        const string expressionCode =
            """
            let dynamicList = ListSomeCountries('countries')
            return 'RU' in dynamicList or 'US' in dynamicList
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_DynamicIntListContainsMatchingItem()
    {
        const string expressionCode =
            """
            let dynamicList = ListOfCustomerIds('Good')
            return 1 in dynamicList
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnFalse_When_AmountIsNegativeAndCurrencyIsEUR()
    {
        const string expressionCode =
            """
            let transfer = Transfer
            let amount = transfer.Amount
            let ccy = transfer.Currency
            return amount < 0 and ccy is 'EUR'
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = -50;
        model.Transfer.Currency = "EUR";

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_SomeComplexConditionIsMet()
    {
        const string expressionCode =
            """
            let x = 100
            let y = 'USD'
            let transfer = Transfer
            let amount = transfer.Amount
            return amount < 500 and y is 'USD'
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = 100;

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnComplexCalculation_When_CombinedWithArithmetic()
    {
        const string expressionCode =
            """
            let amount = Transfer.Amount
            let fee = amount * 0.05
            let totalAmount = amount - fee
            return totalAmount
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = 200;

        var result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(190, result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_AmountIsWithinRange_And_CurrencyIsValid_And_AmountIncreasedByCustomLogic()
    {
        const string expressionCode =
            """
            let transfer = Transfer
            let amount = transfer.Amount
            let currency = transfer.Currency
            let threshold = 500
            let validCurrencies = ['USD', 'EUR', 'GBP']
            let increaseAmountByPercentage = amount + (amount * (5 / 100))
            let increasedAmount = when currency in validCurrencies then increaseAmountByPercentage else amount end
            return increasedAmount > 180 and increasedAmount < threshold
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = 180;
        model.Transfer.Currency = "USD";

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnCorrectResult_When_NestedListOperationsAndStringManipulationsAreUsed()
    {
        const string expressionCode =
            """
            let items = ['USD', 'EUR', 'LIRA']
            let priceList = [2.8, 3.0, 0.75]
            let targetItem = 'USD'
            let targetPrice = when targetItem in items then 3 else 2 end
            let discountedPrice = when targetPrice > 1.0 then targetPrice * 0.9 else targetPrice end
            let result = targetItem + ' costs ' + discountedPrice
            return result
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("USD costs 2.7", result);
    }

    [Fact]
    public void ExecuteExpression_Should_ReturnSumOfXAndY_When_XyPropertiesAreUsed()
    {
        const string expressionCode =
            """
            let X = Xy.X
            let Y = Xy.Y
            return X + Y
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(3, result);
    }
}
