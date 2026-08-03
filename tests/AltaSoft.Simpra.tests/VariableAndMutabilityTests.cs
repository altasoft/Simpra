using AltaSoft.Simpra.Tests.Models;
using static AltaSoft.Simpra.Tests.Models.TestModelFactory;

namespace AltaSoft.Simpra.Tests;

public class VariableAndMutabilityTests
{
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
    public void CompoundAssignment_PlusEquals_OnLetListVariable_ShouldAppendValue()
    {
        const string expressionCode = """
            $mutable on
            let list = [1, 2, 3]
            list += 4
            return list
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<List<int>, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode,
            new SimpraCompilerOptions { MutabilityOption = MutabilityOption.DefaultImmutable });

        Assert.Equal([1, 2, 3, 4], result);
    }

    [Fact]
    public void CompoundAssignment_MinusEquals_OnLetListVariable_ShouldRemoveValue()
    {
        const string expressionCode = """
            $mutable on
            let list = [1, 2, 3]
            list -= 2
            return list
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var result = simpra.Execute<List<int>, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode,
            new SimpraCompilerOptions { MutabilityOption = MutabilityOption.DefaultImmutable });

        Assert.Equal([1, 3], result);
    }

    [Fact]
    public void MutableDirective_WhenCompilerOptionsAreImmutable_ShouldThrowSimpraException()
    {
        const string expressionCode = """
            $mutable on
            return 1
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var f = () => simpra.Execute<int, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode,
            new SimpraCompilerOptions { MutabilityOption = MutabilityOption.Immutable });

        Assert.Throws<SimpraException>(() => f());
    }

    [Fact]
    public void Assignment_WithoutMutableDirective_ShouldThrowSimpraException()
    {
        const string expressionCode = """
            Transfer.Amount = 500
            return Transfer.Amount
            """;

        var simpra = new Simpra();
        var model = GetTestModel();

        var f = () => simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);

        Assert.Throws<SimpraException>(() => f());
    }
}
