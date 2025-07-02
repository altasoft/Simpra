using AltaSoft.Simpra.Tests.Models;

namespace AltaSoft.Simpra.Tests;

public class SimpraExpressionTests
{
    [Fact]
    public void InvalidSimpraSyntax_ShouldThrowException_WhenIncorrectAndSignIsUsedAndReturnStatement()
    {
        const string expressionCode =
            """
             return Amount is 100 && Amount is 200
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        // ReSharper disable ConvertToLocalFunction
        var f = () => simpra.Execute<bool, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);
        // ReSharper restore ConvertToLocalFunction
        Assert.Throws<SimpraException>(() => f());
    }

    [Fact]
    public void InvalidSimpraSyntax_ShouldThrowException_WhenIncorrectAndSignIsUsed()
    {
        const string expressionCode =
            """
             Amount is 100 && Amount is 200
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        // ReSharper disable ConvertToLocalFunction
        var f = () => simpra.Execute<bool, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);
        // ReSharper restore ConvertToLocalFunction
        Assert.Throws<SimpraException>(() => f());
    }

    [Fact]
    public void InvalidSimpraSyntax_ShouldThrowException_WithoutReturn()
    {
        const string expressionCode =
            """
             111 Amount is 100
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        // ReSharper disable ConvertToLocalFunction
        var f = () => simpra.Execute<bool, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);
        // ReSharper restore ConvertToLocalFunction
        Assert.Throws<SimpraException>(() => f());
    }

    [Fact]
    public void InvalidSimpraSyntax_ShouldThrowException_WithReturn()
    {
        const string expressionCode =
            """
            return 111 Amount is 100
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        // ReSharper disable ConvertToLocalFunction
        var f = () => simpra.Execute<bool, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);
        // ReSharper restore ConvertToLocalFunction
        Assert.Throws<SimpraException>(() => f());
    }

    [Fact]
    public void DictionaryIndexer_MissingKey_ReturnsDefaultValueForProperty()
    {
        const string expressionCode =
            """
            return DictionaryOfObjects['test'].Id
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.DictionaryOfObjects = new Dictionary<string, Customer> { { "Georgia", new Customer { Id = 1, Status = 10 } } };

        var result = simpra.Execute<int, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(0, result);
    }

    [Fact]
    public void CallGetValueFromDictionaryWhenKeyDoesNotExist_ShouldReturnDefault()
    {
        const string expressionCode =
            """
            return Countries['test'] is 'Test'
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Countries = new Dictionary<string, string> { { "Georgia", "Test" } };
        var result = simpra.Execute<bool, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);
        Assert.False(result);
    }

    [Fact]
    public void CallGetValueFromDictionaryWhenKeyExist_ShouldReturnValue()
    {
        const string expressionCode =
            """
            return Countries['Georgia'] is 'Test'
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Countries = new Dictionary<string, string> { { "Georgia", "Test" } };
        var result = simpra.Execute<bool, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);
        Assert.True(result);
    }

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
    public void ModelWithInheritedClassProperties_ShouldFindPropertyCorrectly()
    {
        const string expressionCode =
            """
            return Color
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Color = Color.Blue;
        var result = simpra.Execute<Color, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(Color.Blue, result);
    }

    [Fact]
    public void ModelWithInheritedInterfaceProperties_ShouldFindPropertyCorrectly()
    {
        const string expressionCode =
            """
            return Customer.Id
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<int, ITransferModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(1, result);
    }

    [Fact]
    public void Expression_ReturnNullableEnum_ReturnValueMustBeCorrect()
    {
        const string expressionCode =
            """
            return NullableEnum
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.NullableEnum = Color.Green;
        var result = simpra.Execute<Color?, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.NotNull(result);

        model.NullableEnum = null;
        result = simpra.Execute<Color?, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Null(result);
    }

    [Fact]
    public void Expression_ShouldCompareNullableEnum_ReturnValueMustBeCorrect()
    {
        const string expressionCode =
            """
            return NullableEnum is 'Green'
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.NullableEnum = Color.Green;
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.True(result);
    }

    [Fact]
    public void Expression_NestedDomainPrimitiveType_ShouldReturnCorrectly()
    {
        const string expressionCode =
            """
            return Transfer.RegulatoryReporting[1].Details[1].Information[1]
            """;

        var simpra = new Simpra();
        var model = Iso20022TransferModel.CreateForInformation();

        var result = simpra.Execute<Max35Text, Iso20022TransferModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.NotNull(result);
    }

    [Fact]
    public void Expression_NestedDomainPrimitiveType_ShouldCompareCorrectly()
    {
        const string expressionCode =
            """
            return Transfer.RegulatoryReporting[1].Details[1].Information[1] is 'Information1'
            """;

        var simpra = new Simpra();
        var model = Iso20022TransferModel.CreateForInformation();

        var result = simpra.Execute<bool, Iso20022TransferModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void ExpressionListOfList_Comparison_ShouldReturnCorrectly()
    {
        const string expressionCode =
            """
            return ListOfList[2][2] is 4
            """;

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"], ListOfList = [[1, 2], [3, 4]] };

        var result = simpra.Execute<bool, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void ListOfList_ShouldReturnCorrectly()
    {
        const string expressionCode =
            """
            return ListOfList 
            """;

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"], ListOfList = [[1, 2], [3, 4]] };

        var result = simpra.Execute<List<List<int>>, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(4, result[1][1]);
    }

    [Fact]
    public void GetComplexObject_ShouldReturnCorrectly()
    {
        const string expressionCode = "return Customer";

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here

        var result = simpra.Execute<Customer, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void EnumerableOfIntegers_ShouldReturnCorrectly()
    {
        const string expressionCode =
            """
            return IntegerEnumerable
            """;

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"], IntegerEnumerable = [1, 2, 3] };

        var result = simpra.Execute<IEnumerable<int>, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(2, result.ToList()[1]);
    }

    [Fact]
    public void ArrayOfIntegers_ShouldReturnCorrectly()
    {
        const string expressionCode =
            """
            return IntegerArray
            """;

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"], IntegerArray = [1, 2, 3] };

        var result = simpra.Execute<int[], ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(2, result[1]);
    }

    [Fact]
    public void ListOfComplexObjects_ShouldReturnCorrectly()
    {
        const string expressionCode =
            """
            return ComplexList
            """;

        var simpra = new Simpra();
        var model = new ListModel
        {
            EnumList = [Color.Blue, Color.Green],
            IntegerList = [1, 2, 3],
            StringList = ["test", "test2"],
            ComplexList =
            [
                new Customer { Id = 1, Status = 1 },
                new Customer { Id = 2, Status = 2 }
            ]
        };

        var result = simpra.Execute<List<Customer>, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(2, result[1].Id);
    }

    [Fact]
    public void ExpressionStringListEqualsValue_ShouldReturnCorrectValues()
    {
        const string expressionCode =
            """
            return StringList[1] is 'test'
            """;

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"] };

        var result = simpra.Execute<bool, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void StringList_ShouldReturnCorrectValues()
    {
        const string expressionCode =
            """
            return StringList
            """;

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"] };

        var result = simpra.Execute<List<string>, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("test2", result[1]);
    }

    [Fact]
    public void EnumList_ShouldReturnCorrectValues()
    {
        const string expressionCode =
            """
            return EnumList
            """;

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"] };

        var result = simpra.Execute<List<Color>, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(Color.Green, result[1]);
    }

    [Fact]
    public void IntegerList_ShouldReturnCorrectValues()
    {
        const string expressionCode =
            """
            return IntegerList
            """;

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"] };

        var result = simpra.Execute<List<int>, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(2, result[1]);
    }

    [Fact]
    public void Execute_ShouldReturnListOfComplexObject()
    {
        const string expressionCode =
            """
            return Transfer.RegulatoryReporting
            """;

        var simpra = new Simpra();
        var model = Iso20022TransferModel.CreateForCountry("FR");

        var result = simpra.Execute<List<RegulatoryReporting>, Iso20022TransferModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal("FR", result[0].Authority.Country);
    }

    [Fact]
    public void Execute_ShouldReturnComplexObject_WhenAccessedViaIndex()
    {
        const string expressionCode =
            """
            return Transfer.RegulatoryReporting[1]
            """;

        var simpra = new Simpra();
        var model = Iso20022TransferModel.CreateForCountry("FR");

        var result = simpra.Execute<RegulatoryReporting, Iso20022TransferModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal("FR", result.Authority.Country);
    }

    [Fact]
    public void Execute_ShouldReturnTrue_WhenAuthorityCountryIsFR()
    {
        const string expressionCode =
            """
            return Transfer.RegulatoryReporting[1].Authority.Country is 'FR'
            """;

        var simpra = new Simpra();
        var model = Iso20022TransferModel.CreateForCountry("FR");

        var result = simpra.Execute<bool, Iso20022TransferModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnFalse_When_IndexIsOutOfRangeAndValueCompared()
    {
        const string expressionCode =
            """
            return Transfer.A[10] is  1
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.A = [1, 2, 3];
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.False(result);
    }

    [Fact]
    public void Expression_Should_ReturnFalse_When_NestedListIsUsed()
    {
        const string expressionCode =
            """
            return Transfer.OuterList[10].InnerList[1] is  1
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer!.A = [1, 2, 3];
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.False(result);
    }

    [Fact]
    public void Expression_Should_ReturnFalse_When_NestedListIsUsedX()
    {
        const string expressionCode =
            """
            return CustomerList[1] has value
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.CustomerList = new List<Customer2>();
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.False(result);
    }

    [Fact]
    public void Expression_Should_ReturnFalse_When_ArrayIsNullAndValueCompared()
    {
        const string expressionCode =
            """
            return Transfer.A[1] is  1
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer = null;
        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.False(result);
    }

    [Fact]
    public void Expression_Should_ReturnDefault_When_ArrayIsNull()
    {
        const string expressionCode =
            """
            return Transfer.A[10]
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer = null;
        var result = simpra.Execute<int, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(0, result);
    }

    [Fact]
    public void Expression_Should_ReturnFalse_When_TheValueIsNull()
    {
        const string expressionCode =
            """
            return Transfer.Customer.Id is 1
            """;

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer = null;

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.False(result);
    }

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
    public void ExecuteExpression_Should_UpdateCustomerIdAndReturnSum_When_MutabilityIsOn()
    {
        const string expressionCode =
            """
            $mutable on
            let Y = Nint2
            let list = Values
            let X = CustomerId
            CustomerId = 12
            return X + Y
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<int, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode,
            new SimpraCompilerOptions { MutabilityOption = MutabilityOption.DefaultImmutable });

        Assert.Equal(31, result);
        Assert.Equal(12, (int)model.CustomerId);
    }

    [Fact]
    public void ExecuteExpression_Should_ReturnFalse_When_Nint1HasNoValue()
    {
        const string expressionCode =
            """
            let X = CustomerId
            let Y = Nint1
            return Y has value
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.False(result);
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
    public async Task ExecuteExpression_Should_ReturnTrue_When_CcyIsInListOfCurrencyCodes()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        const string expressionCode = "return Ccy in (ListOfCurrencyCodes('VisaB2B') + 'USD')";
        var result = await simpra.ExecuteAsync<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode, null, CancellationToken.None);
        Assert.True(result);
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
    public void ExecuteExpression_WithNullCheck_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new TestModelMain { Test = null };
        const string expression = "return Test has value";

        var result = simpra.Execute<bool, TestModelMain, TestFunctions>(model, new TestFunctions(), expression);

        Assert.False(result);
    }

    [Fact]
    public void ExecuteExpression_Should_ReturnTrue_When_PropertyIsEnum()
    {
        const string expression = """
                                      return Test.Test is 'Test1';
                                  """;
        var simpra = new Simpra();
        var model = new TestModelMain { Test = new TestModel1 { Test = TestModel2.Test1 } };
        var result = simpra.Execute<bool, TestModelMain, TestFunctions>(model, new TestFunctions(), expression);

        Assert.True(result);
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
    public void Expression_Should_ReturnSixty_When_XIsNotFive()
    {
        const string expressionCode =
            """
            let X = '50'
            if X is not '5' then 
                X = '60'
            end
            return X
            """;

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here
        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("60", result);
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

    [Fact]
    public void Expression_Should_UpdateObjectProperties_And_ReturnFalse()
    {
        const string expressionCode = """
                                      $mutable on
                                      Transfer.Amount = 15
                                      Transfer.Currency = 'GBP'
                                      return Transfer.Amount > 100 or Transfer.Currency is 'USD';
                                      """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode,
            new SimpraCompilerOptions { MutabilityOption = MutabilityOption.DefaultImmutable });

        Assert.Equal(15, model.Transfer!.Amount);
        Assert.Equal("GBP", model.Transfer.Currency);
        Assert.False(result);
    }

    [Fact]
    public void ExecuteWitNullableEnum_ShouldReturnValue()
    {
        const string expressionCode = "return Nint1";

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here
        model.Nint1 = 1;
        var result = simpra.Execute<int?, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(1, result);
    }

    [Fact]
    public void Execute_WithValidColorReference_ShouldReturnGreen()
    {
        const string expressionCode = "return Color";

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here

        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("Green", result);
    }

    [Fact]
    public void Execute_WithUnknownColorReference_ShouldReturnGreen()
    {
        const string expressionCode = "return ColorX";

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here

        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("Green", result);
    }

    [Fact]
    public void ExecuteSyntax_WithArithmeticOperations_ShouldReturnCorrectValue()
    {
        const string expressionCode = "return ColorM";

        var simpra = new Simpra();
        var model = GetTestModel(); // Model is irrelevant here

        var result = simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("Green", result);
    }

    private static TestModel GetTestModel()
    {
        return new TestModel { Transfer = new Transfer { Amount = 100, Currency = "USD" }, Customer = new Customer { Id = 1, Status = 1 }, Remittance = "Test" };
    }

    public class BaseFunctions
    {
        public static string CallBaseStaticMethod() => "BaseStaticMethod";

        public static string CallBaseMethod() => "BaseMethod";
    }

    public interface IFunctions : IBaseFunctions
    {
        string Upper(string str);
    }

    public interface IBaseFunctions
    {
        string Lower(string str);
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
}
