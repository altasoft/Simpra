using AltaSoft.Simpra.Tests.Models;
using static AltaSoft.Simpra.Tests.Models.TestModelFactory;

namespace AltaSoft.Simpra.Tests;

public class OperatorTests
{
    [Fact]
    public void TaskExpression_Should_ReturnTrue_When_ValueMatchesRegexPattern()
    {
        const string expressionCode =
            """
            let Value = 'nbas568fq'
            return Value matches '[a-zA-Z_][a-zA-Z_0-9]'
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode,
            new SimpraCompilerOptions { MutabilityOption = MutabilityOption.Immutable, StringComparisonOption = StringComparisonOption.IgnoreCase });
        Assert.True(result);
    }

    [Fact]
    public void Execute_NullableAmount_LessThanTen_ShouldReturnTrue()
    {
        const string expressionCode = "NullableAmount < 10";

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel>(model, expressionCode, null);
        Assert.True(result);
    }

    [Fact]
    public void Execute_NullableIntegerProperty_LessThanZero_ShouldReturnFalse()
    {
        const string expressionCode = "NullableIntegerProperty < 0";

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel>(model, expressionCode, null);
        Assert.False(result);
    }

    [Fact]
    public void ExecuteExpression_Should_ReturnFalse_When_TransferAmountIsGreaterThanTen()
    {
        const string expressionCode = "Transfer.Amount < 10";

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = 11;

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.False(result);
    }

    [Fact]
    public void ExecuteExpression_WithLogicalOperations_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new TestModelMain { Test = null };
        const string expression = "return (false or false) and (true or true)";

        var result = simpra.Execute<bool, TestModelMain, TestFunctions>(model, new TestFunctions(), expression);

        Assert.False(result);
    }

    [Fact]
    public void ExecuteExpression_WithArithmeticOperations_ShouldReturnCorrectValue()
    {
        var simpra = new Simpra();
        var model = new TestModelMain { Test = null };
        const string expression = "return (10 + 10) * 10";
        const int expected = 200;

        var result = simpra.Execute<int, TestModelMain, TestFunctions>(model, new TestFunctions(), expression);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ExecuteExpression_Should_ReturnZero_When_AddingOneAndNegativeOne()
    {
        const string expression = """
                                  let x = 1
                                  let y = -1
                                  return x + y
                                  """;
        var simpra = new Simpra();
        var model = new TestModelMain { Test = new TestModel1 { Test = TestModel2.Test2 } };
        var result = simpra.Execute<int, TestModelMain, TestFunctions>(model, new TestFunctions(), expression);
        Assert.Equal(0, result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_StrMatchesPattern()
    {
        const string expressionCode =
            """
            let str = 'abc';
            return str like 'a%'
            """;

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_CaseInsensitivePatternMatches()
    {
        const string expressionCode =
            """
            let str = 'abc'
            $case_sensitive off
            return str like 'A%'
            """;

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnFalse_When_CaseSensitivePatternDoesNotMatch()
    {
        const string expressionCode =
            """
            let str = 'abc'
            $case_sensitive on
            return str like 'A%'
            """;

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.False(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_AmountIsGreaterThanHundred()
    {
        const string expressionCode =
            """
            let transfer = Transfer
            let amount = transfer.Amount
            return amount > 100 * 2 / 1.1
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = 200;
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_AmountIsGreaterThanFifty_And_CurrencyIsUSD()
    {
        const string expressionCode = "return Transfer.Amount > 50 and Transfer.Currency is 'USD'";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnFalse_When_AmountIsNotGreaterThanFifty_And_CurrencyIsNotUSD()
    {
        const string expressionCode = "return not (Transfer.Amount > 50 and Transfer.Currency is 'USD')";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.False(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_CurrencyIsInList()
    {
        const string expressionCode = "return Transfer.Currency in ['USD', 'EUR']";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnFalse_When_CurrencyIsNotInList()
    {
        const string expressionCode = "return Transfer.Currency not in ['USD', 'EUR']";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.False(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_CurrencyIsConcatenatedString()
    {
        const string expressionCode = "return Transfer.Currency is 'US' + 'D'";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_CurrencyIsInConcatenatedList()
    {
        const string expressionCode = "return Transfer.Currency in ['USD', 'EUR'] + 'GEL'";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_AmountIsEqualToFifty()
    {
        const string expressionCode = "return Transfer.Amount is 50";

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = 50;
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_NestedConditionsAreMet()
    {
        const string expressionCode =
            "return (Transfer.Amount > 50 and Transfer.Currency is 'USD') or (Transfer.Amount < 20 and Transfer.Currency is 'EUR')";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_CurrencyIsEmpty()
    {
        const string expressionCode = "return Transfer.Currency is ''";

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Currency = "";
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnHelloWorld_When_StringsAreConcatenated()
    {
        const string expressionCode = "return 'Hello' + ' ' + 'World!'";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("Hello World!", result);
    }

    [Fact]
    public void Expression_Should_ReturnCurrencyWithString_When_Concatenated()
    {
        const string expressionCode = "return Transfer.Currency + ' is strong'";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("USD is strong", result);
    }

    [Fact]
    public void Expression_Should_ReturnCurrencyAndAmount_When_Concatenated()
    {
        const string expressionCode = "return Transfer.Currency + ' - Amount: ' + Transfer.Amount";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("USD - Amount: 100", result);
    }

    [Fact]
    public void Expression_Should_ReturnTrue_When_CurrencyIsInSubtractedList()
    {
        const string expressionCode = "return Transfer.Currency in ['USD', 'EUR', 'GEL'] - ['GEL']";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);

        model.Transfer!.Currency = "GEL";
        result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.False(result);
    }

    [Fact]
    public void Expression_Should_ReturnEmptyArray_When_SubtractingIdenticalLists()
    {
        const string expressionCode = "return ['USD', 'EUR'] - ['USD', 'EUR']";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<string[], TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Empty(result);
    }

    [Fact]
    public void Expression_Should_ReturnOriginalArray_When_SubtractingNonOverlappingLists()
    {
        const string expressionCode = "return ['USD', 'EUR'] - ['GEL']";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<string[], TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Contains("USD", result);
        Assert.Contains("EUR", result);
        Assert.DoesNotContain("GEL", result);
    }

    [Theory]
    [InlineData("2024-01-01", "2023-01-01", true)]
    [InlineData("2023-01-01", "2024-01-01", false)]
    [InlineData("2024-01-01", "2024-01-01", false)]
    public void SimpraDate_GreaterThanComparison_ShouldCompareChronologically(string left, string right, bool expected)
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), $"return date('{left}') > date('{right}')");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SimpraDate_Equality_ShouldCompareByValue()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), "return date('2024-01-01') is date('2024-01-01')");
        Assert.True(result);

        result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), "return date('2024-01-01') is not date('2024-01-02')");
        Assert.True(result);
    }

    [Fact]
    public void SimpraDate_Min_ShouldReturnEarlierDate()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<DateTime, TestModel, TestFunctions>(model, new TestFunctions(), "return date('2024-01-01') min date('2023-01-01')");
        Assert.Equal(new DateTime(2023, 1, 1), result);
    }

    [Fact]
    public void SimpraDate_Max_ShouldReturnLaterDate()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<DateTime, TestModel, TestFunctions>(model, new TestFunctions(), "return date('2024-01-01') max date('2023-01-01')");
        Assert.Equal(new DateTime(2024, 1, 1), result);
    }

    [Fact]
    public void SimpraDate_In_ShouldReturnTrue_When_DateExistsInList()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(),
            "return date('2024-01-01') in [date('2024-01-01'), date('2024-06-01')]");
        Assert.True(result);

        result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(),
            "return date('2024-12-25') in [date('2024-01-01'), date('2024-06-01')]");
        Assert.False(result);
    }

    [Theory]
    [InlineData(true, false, true)]    // AND NOT: true && !false = true
    [InlineData(true, true, false)]    // true && !true = false
    [InlineData(false, true, false)]   // false && !true = false
    [InlineData(false, false, false)]  // false && !false = false
    public void BooleanOperator_Subtract_ShouldComputeAndNot(bool left, bool right, bool expected)
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), $"return {(left ? "true" : "false")} - {(right ? "true" : "false")}");
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    [InlineData(false, false, false)]
    public void BooleanOperator_Multiply_ShouldComputeLogicalAnd(bool left, bool right, bool expected)
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), $"return {(left ? "true" : "false")} * {(right ? "true" : "false")}");
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(true, true, false)]   // NAND: not(true && true) = false
    [InlineData(true, false, true)]
    [InlineData(false, false, true)]
    public void BooleanOperator_Divide_ShouldComputeLogicalNand(bool left, bool right, bool expected)
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), $"return {(left ? "true" : "false")} / {(right ? "true" : "false")}");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void BooleanOperator_MinMax_ShouldComputeLogicalAndOr()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), "return true min false");
        Assert.False(result);

        result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), "return true max false");
        Assert.True(result);
    }

    [Theory]
    [InlineData(10, 3, 3)]     // 10 / 3 = 3.33.. -> 3
    [InlineData(7, 2, 4)]      // 7 / 2 = 3.5 -> rounds to nearest even (4)
    [InlineData(9, 2, 4)]      // 9 / 2 = 4.5 -> rounds to nearest even (4)
    [InlineData(-7, 2, -4)]    // -7 / 2 = -3.5 -> rounds to nearest even (-4)
    public void BinaryOperator_IntegerDivision_ShouldRoundQuotientToNearestEvenInteger(int left, int right, int expected)
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), $"return {left} // {right}");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void BinaryOperator_IntegerDivision_ByZero_ShouldThrowDivideByZeroException()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var f = () => simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), "return 5 // 0");
        Assert.Throws<DivideByZeroException>(() => f());
    }

    [Theory]
    [InlineData(10, 3, 3)]
    [InlineData(3, 10, 3)]
    [InlineData(-5, -2, -5)]
    public void BinaryOperator_Min_ShouldReturnSmallerNumber(int left, int right, int expected)
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), $"return {left} min {right}");
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(10, 3, 10)]
    [InlineData(3, 10, 10)]
    [InlineData(-5, -2, -2)]
    public void BinaryOperator_Max_ShouldReturnLargerNumber(int left, int right, int expected)
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), $"return {left} max {right}");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void UnaryOperator_Plus_ShouldReturnSameNumericValue()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<long, TestModel, TestFunctions>(model, new TestFunctions(), "return +5");
        Assert.Equal(5, result);
    }

    [Fact]
    public void UnaryOperator_Percent_ShouldConvertToHundredth()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return 50%");
        Assert.Equal(0.5m, result);
    }

    [Fact]
    public void UnaryOperator_Percent_AppliedWithinArithmetic_ShouldComputePercentageOfAmount()
    {
        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = 200;

        var result = simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return Transfer.Amount * 5%");
        Assert.Equal(10m, result);
    }

    [Theory]
    [InlineData(150, true)]
    [InlineData(1000, false)]
    [InlineData(50, false)]
    [InlineData(100, false)]   // lower bound is exclusive
    [InlineData(101, true)]
    public void ChainedComparison_WithAndKeyword_ShouldCheckValueIsWithinRange(int amount, bool expected)
    {
        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = amount;

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), "return Transfer.Amount > 100 and < 1000");
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(150, true)]
    [InlineData(1000, false)]
    [InlineData(50, false)]
    public void ChainedComparison_WithOrKeyword_StillCombinesBothSidesWithAnd(int amount, bool expected)
    {
        // The grammar accepts 'and'/'or' between the two comparison halves of a chained comparison,
        // but the visitor (HandleChainedComparison) always combines them with AndAlso regardless of
        // which keyword was written. This test documents that actual (surprising) behavior.
        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.Amount = amount;

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), "return Transfer.Amount > 100 or < 1000");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void BinaryOperator_AnyIn_ShouldReturnTrue_When_AtLeastOneElementExistsInOtherList()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), "return [1, 2, 3] any in [5, 6, 3]");
        Assert.True(result);
    }

    [Fact]
    public void BinaryOperator_AnyIn_ShouldReturnFalse_When_NoElementsExistInOtherList()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), "return [1, 2, 3] any in [5, 6, 7]");
        Assert.False(result);
    }

    [Fact]
    public void BinaryOperator_AllIn_ShouldReturnTrue_When_EveryElementExistsInOtherList()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), "return [1, 2] all in [1, 2, 3]");
        Assert.True(result);
    }

    [Fact]
    public void BinaryOperator_AllIn_ShouldReturnFalse_When_NotEveryElementExistsInOtherList()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), "return [1, 2, 4] all in [1, 2, 3]");
        Assert.False(result);
    }

    [Fact]
    public void BinaryOperator_AnyNotIn_ShouldReturnTrue_When_NoElementsExistInOtherList()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), "return [1, 2, 3] any not in [5, 6, 7]");
        Assert.True(result);
    }

    [Fact]
    public void BinaryOperator_AllNotIn_ShouldReturnFalse_When_AllElementsExistInOtherList()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), "return [1, 2] all not in [1, 2, 3]");
        Assert.False(result);
    }

    [Fact]
    public void ListOperator_Subtract_OnNumberLists_ShouldRemoveMatchingElements()
    {
        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<decimal[], TestModel, TestFunctions>(model, new TestFunctions(), "return [1, 2, 3] - [2, 3]");
        Assert.Equal([1m], result);
    }

    [Fact]
    public void ListOperator_Multiply_ShouldReturnIntersectionOfLists()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<string[], TestModel, TestFunctions>(model, new TestFunctions(), "return ['USD', 'EUR', 'GEL'] * ['EUR', 'GEL', 'EUR']");

        Assert.Equal(2, result.Length);
        Assert.Contains("EUR", result);
        Assert.Contains("GEL", result);
        Assert.DoesNotContain("USD", result);
    }

    [Fact]
    public void ListOperator_Divide_ShouldThrowInvalidOperationException()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var f = () => simpra.Execute<string[], TestModel, TestFunctions>(model, new TestFunctions(), "return ['USD', 'EUR'] / ['EUR']");
        Assert.Throws<InvalidOperationException>(() => f());
    }

    [Fact]
    public void StringOperator_Multiply_ShouldRepeatStringGivenNumberOfTimes()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), "return 'ab' * 3");
        Assert.Equal("ababab", result);
    }

    [Fact]
    public void StringOperator_Divide_ShouldSplitStringBySeparatorIntoList()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<string[], TestModel, TestFunctions>(model, new TestFunctions(), "return 'a,b,c' / ','");
        Assert.Equal(["a", "b", "c"], result);
    }
}
