using AltaSoft.Simpra.Tests.Models;
using static AltaSoft.Simpra.Tests.Models.TestModelFactory;

namespace AltaSoft.Simpra.Tests;

public class ErrorHandlingTests
{
    [Fact]
    public void InvalidSimpraSyntax_ShouldThrowException_WhenIncorrectAndSignIsUsedAndReturnStatement()
    {
        const string expressionCode =
            " return Amount is 100 && Amount is 200";

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
            " Amount is 100 && Amount is 200";

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
            " 111 Amount is 100";

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
            "return 111 Amount is 100";

        var simpra = new Simpra();
        var model = GetTestModel();

        // ReSharper disable ConvertToLocalFunction
        var f = () => simpra.Execute<bool, TestModel, IFunctions>(model, new TestFunctions(), expressionCode);
        // ReSharper restore ConvertToLocalFunction
        Assert.Throws<SimpraException>(() => f());
    }

    [Fact]
    public void Division_ByZero_ShouldThrowDivideByZeroException()
    {
        var simpra = new Simpra();
        var model = GetTestModel();

        var f = () => simpra.Execute<decimal, TestModel, TestFunctions>(model, new TestFunctions(), "return 5 / 0");

        Assert.Throws<DivideByZeroException>(() => f());
    }

    [Fact]
    public void CallingUndefinedFunction_ShouldThrowSimpraException()
    {
        const string expressionCode = "return NonExistentFunction()";

        var simpra = new Simpra();
        var model = GetTestModel();

        var f = () => simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Throws<SimpraException>(() => f());
    }

    [Fact]
    public void AccessingUndefinedProperty_ShouldThrowSimpraException()
    {
        const string expressionCode = "return NonExistentProperty";

        var simpra = new Simpra();
        var model = GetTestModel();

        var f = () => simpra.Execute<string, TestModel, TestFunctions>(model, new TestFunctions(), expressionCode);
        Assert.Throws<SimpraException>(() => f());
    }
}
