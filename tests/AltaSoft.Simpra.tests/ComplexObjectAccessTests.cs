using AltaSoft.Simpra.Tests.Models;
using static AltaSoft.Simpra.Tests.Models.TestModelFactory;

namespace AltaSoft.Simpra.Tests;

public class ComplexObjectAccessTests
{
    [Fact]
    public void DictionaryIndexer_MissingKey_ReturnsDefaultValueForProperty()
    {
        const string expressionCode =
            "return DictionaryOfObjects['test'].Id";

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
            "return Countries['test'] is 'Test'";

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
            "return Countries['Georgia'] is 'Test'";

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Countries = new Dictionary<string, string> { { "Georgia", "Test" } };
        var result = simpra.Execute<bool, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);
        Assert.True(result);
    }

    [Fact]
    public void ModelWithInheritedClassProperties_ShouldFindPropertyCorrectly()
    {
        const string expressionCode =
            "return Color";

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
            "return Customer.Id";

        var simpra = new Simpra();
        var model = GetTestModel();
        var result = simpra.Execute<int, ITransferModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(1, result);
    }

    [Fact]
    public void Expression_ReturnNullableEnum_ReturnValueMustBeCorrect()
    {
        const string expressionCode =
            "return NullableEnum";

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
            "return NullableEnum is 'Green'";

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
            "return Transfer.RegulatoryReporting[1].Details[1].Information[1]";

        var simpra = new Simpra();
        var model = Iso20022TransferModel.CreateForInformation();

        var result = simpra.Execute<Max35Text, Iso20022TransferModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.NotNull(result);
    }

    [Fact]
    public void Expression_NestedDomainPrimitiveType_ShouldCompareCorrectly()
    {
        const string expressionCode =
            "return Transfer.RegulatoryReporting[1].Details[1].Information[1] is 'Information1'";

        var simpra = new Simpra();
        var model = Iso20022TransferModel.CreateForInformation();

        var result = simpra.Execute<bool, Iso20022TransferModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void ExpressionListOfList_Comparison_ShouldReturnCorrectly()
    {
        const string expressionCode =
            "return ListOfList[2][2] is 4";

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"], ListOfList = [[1, 2], [3, 4]] };

        var result = simpra.Execute<bool, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void ListOfList_ShouldReturnCorrectly()
    {
        const string expressionCode =
            "return ListOfList";

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
            "return IntegerEnumerable";

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"], IntegerEnumerable = [1, 2, 3] };

        var result = simpra.Execute<IEnumerable<int>, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(2, result.ToList()[1]);
    }

    [Fact]
    public void ArrayOfIntegers_ShouldReturnCorrectly()
    {
        const string expressionCode =
            "return IntegerArray";

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"], IntegerArray = [1, 2, 3] };

        var result = simpra.Execute<int[], ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(2, result[1]);
    }

    [Fact]
    public void ListOfComplexObjects_ShouldReturnCorrectly()
    {
        const string expressionCode =
            "return ComplexList";

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
            "return StringList[1] is 'test'";

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"] };

        var result = simpra.Execute<bool, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.True(result);
    }

    [Fact]
    public void StringList_ShouldReturnCorrectValues()
    {
        const string expressionCode =
            "return StringList";

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"] };

        var result = simpra.Execute<List<string>, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal("test2", result[1]);
    }

    [Fact]
    public void EnumList_ShouldReturnCorrectValues()
    {
        const string expressionCode =
            "return EnumList";

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"] };

        var result = simpra.Execute<List<Color>, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Equal(Color.Green, result[1]);
    }

    [Fact]
    public void IntegerList_ShouldReturnCorrectValues()
    {
        const string expressionCode =
            "return IntegerList";

        var simpra = new Simpra();
        var model = new ListModel { EnumList = [Color.Blue, Color.Green], IntegerList = [1, 2, 3], StringList = ["test", "test2"] };

        var result = simpra.Execute<List<int>, ListModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal(2, result[1]);
    }

    [Fact]
    public void Execute_ShouldReturnListOfComplexObject()
    {
        const string expressionCode =
            "return Transfer.RegulatoryReporting";

        var simpra = new Simpra();
        var model = Iso20022TransferModel.CreateForCountry("FR");

        var result = simpra.Execute<List<RegulatoryReporting>, Iso20022TransferModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal("FR", result[0].Authority.Country);
    }

    [Fact]
    public void Execute_ShouldReturnComplexObject_WhenAccessedViaIndex()
    {
        const string expressionCode =
            "return Transfer.RegulatoryReporting[1]";

        var simpra = new Simpra();
        var model = Iso20022TransferModel.CreateForCountry("FR");

        var result = simpra.Execute<RegulatoryReporting, Iso20022TransferModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Equal("FR", result.Authority.Country);
    }

    [Fact]
    public void Execute_ShouldReturnTrue_WhenAuthorityCountryIsFR()
    {
        const string expressionCode =
            "return Transfer.RegulatoryReporting[1].Authority.Country is 'FR'";

        var simpra = new Simpra();
        var model = Iso20022TransferModel.CreateForCountry("FR");

        var result = simpra.Execute<bool, Iso20022TransferModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.True(result);
    }

    [Fact]
    public void Expression_Should_ReturnFalse_When_IndexIsOutOfRangeAndValueCompared()
    {
        const string expressionCode =
            "return Transfer.A[10] is  1";

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
            "return Transfer.OuterList[10].InnerList[1] is  1";

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
            "return CustomerList[1] has value";

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
            "return Transfer.A[1] is  1";

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
            "return Transfer.A[10]";

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
            "return Transfer.Customer.Id is 1";

        var simpra = new Simpra();
        var model = GetTestModel();
        model.Transfer = null;

        var result = simpra.Execute<bool, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.False(result);
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
        const string expression = "    return Test.Test is 'Test1';";
        var simpra = new Simpra();
        var model = new TestModelMain { Test = new TestModel1 { Test = TestModel2.Test1 } };
        var result = simpra.Execute<bool, TestModelMain, TestFunctions>(model, new TestFunctions(), expression);

        Assert.True(result);
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
}
