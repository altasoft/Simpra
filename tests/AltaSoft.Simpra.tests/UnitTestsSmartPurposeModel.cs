using AltaSoft.Simpra.Tests.Models;

namespace AltaSoft.Simpra.Tests;

public class UnitTestsSmartPurposeModel
{
    [Fact]
    public void DictionaryValue_ConcatenatedWithStringLiteral_ReturnsConcatenatedString()
    {
        const string expressionCode = "return P['Key1'] + 'A'";

        var simpra = new Simpra();
        var model = new SmartPurposeModel();
        model.SetValue("Key1", "Value1");

        var result = simpra.Execute<string, SmartPurposeModel>(model, expressionCode, null);
        Assert.Equal("Value1A", result);
    }

    [Fact]
    public void NullCustomer_DomainPrimitiveComparison_ReturnsFalseInsteadOfThrowing()
    {
        const string expressionCode = "return Customer.CustomerId is 1";

        var simpra = new Simpra();
        var model = new SmartPurposeModel { Customer = null };

        var result = simpra.Execute<bool, SmartPurposeModel>(model, expressionCode, null);
        Assert.False(result);
    }

    [Fact]
    public void NullCustomer_DomainPrimitiveNotEqualComparison_ReturnsTrue()
    {
        const string expressionCode = "return Customer.CustomerId is not 1";

        var simpra = new Simpra();
        var model = new SmartPurposeModel { Customer = null };

        var result = simpra.Execute<bool, SmartPurposeModel>(model, expressionCode, null);
        Assert.True(result);
    }

    [Fact]
    public void NullCustomer_HasValue_ReturnsFalse()
    {
        const string expressionCode = "return Customer has value";

        var simpra = new Simpra();
        var model = new SmartPurposeModel { Customer = null };

        var result = simpra.Execute<bool, SmartPurposeModel>(model, expressionCode, null);
        Assert.False(result);
    }

    [Fact]
    public void NullCustomer_DomainPrimitiveValue_ReturnsDefaultInsteadOfThrowing()
    {
        const string expressionCode = "return Customer.CustomerId";

        var simpra = new Simpra();
        var model = new SmartPurposeModel { Customer = null };

        var result = simpra.Execute<int, SmartPurposeModel>(model, expressionCode, null);
        Assert.Equal(0, result);
    }

    [Fact]
    public void NonNullCustomer_DomainPrimitiveComparison_ReturnsTrueWhenEqual()
    {
        const string expressionCode = "return Customer.CustomerId is 1";

        var simpra = new Simpra();
        var model = new SmartPurposeModel { Customer = new CustomerX { CustomerId = 1 } };

        var result = simpra.Execute<bool, SmartPurposeModel>(model, expressionCode, null);
        Assert.True(result);
    }

    [Fact]
    public void NonNullCustomer_DomainPrimitiveComparison_ReturnsFalseWhenNotEqual()
    {
        const string expressionCode = "return Customer.CustomerId is 1";

        var simpra = new Simpra();
        var model = new SmartPurposeModel { Customer = new CustomerX { CustomerId = 2 } };

        var result = simpra.Execute<bool, SmartPurposeModel>(model, expressionCode, null);
        Assert.False(result);
    }

    [Fact]
    public void NonNullCustomer_HasValue_ReturnsTrue()
    {
        const string expressionCode = "return Customer has value";

        var simpra = new Simpra();
        var model = new SmartPurposeModel { Customer = new CustomerX { CustomerId = 1 } };

        var result = simpra.Execute<bool, SmartPurposeModel>(model, expressionCode, null);
        Assert.True(result);
    }

    [Fact]
    public void NonNullCustomer_DomainPrimitiveValue_ReturnsActualValue()
    {
        const string expressionCode = "return Customer.CustomerId";

        var simpra = new Simpra();
        var model = new SmartPurposeModel { Customer = new CustomerX { CustomerId = 42 } };

        var result = simpra.Execute<int, SmartPurposeModel>(model, expressionCode, null);
        Assert.Equal(42, result);
    }
}

public sealed class SmartPurposeModel
{
    public CustomerX? Customer { get; set; }
    public Dictionary<string, string> P { get; } = new();

    public void SetValue(string key, string? value)
    {
        P[key] = value ?? string.Empty;
    }

    public string GetValue(string key) => P.TryGetValue(key, out var result) ? result : string.Empty;
}

public class CustomerX
{
    public CustomerId CustomerId { get; set; }
}
