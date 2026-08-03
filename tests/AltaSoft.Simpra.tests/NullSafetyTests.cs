using AltaSoft.Simpra.Tests.Models;

namespace AltaSoft.Simpra.Tests;

public class NullSafetyTests
{

    [Fact]
    public void NullDictionary_SingleKeyLookup_IsComparison_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel { DebtorAccount = new DebtorAccount() };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\"");

        Assert.False(result);
    }

    [Fact]
    public void NullDictionary_MultipleKeyLookups_AndOperator_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel { DebtorAccount = new DebtorAccount() };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" and DebtorAccount.Properties[\"AccSubType\"] is \"8\"");

        Assert.False(result);
    }

    [Fact]
    public void NullDictionary_MultipleKeyLookups_OrOperator_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel { DebtorAccount = new DebtorAccount() };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" or DebtorAccount.Properties[\"AccSubType\"] is \"8\"");

        Assert.False(result);
    }

    [Fact]
    public void NullDictionary_ReturnValue_ShouldReturnNull()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel { DebtorAccount = new DebtorAccount() };

        var result = simpra.Execute<string, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "return DebtorAccount.Properties[\"AccType\"]");

        Assert.Null(result);
    }

    [Fact]
    public void NullDictionary_HasValue_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel { DebtorAccount = new DebtorAccount() };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] has value");

        Assert.False(result);
    }

    // ── Non-null dictionary: missing key should return default ──

    [Fact]
    public void NonNullDictionary_MissingKey_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "Other", "value" } }
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\"");

        Assert.False(result);
    }

    [Fact]
    public void NonNullDictionary_ExistingKey_ShouldReturnTrue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", "200" } }
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\"");

        Assert.True(result);
    }

    [Fact]
    public void NonNullDictionary_ExistingKey_ReturnValue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", "200" } }
            }
        };

        var result = simpra.Execute<string, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "return DebtorAccount.Properties[\"AccType\"]");

        Assert.Equal("200", result);
    }

    [Fact]
    public void NonNullDictionary_KeyWithNullValue_HasValue_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", null } }
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] has value");

        Assert.False(result);
    }

    // ── Null dictionary with complex value type ──

    [Fact]
    public void NullDictionary_ComplexValueType_PropertyAccess_ShouldReturnDefault()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel { DebtorAccount = new DebtorAccount() };

        var result = simpra.Execute<int, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "return DebtorAccount.CustomerMap[\"key\"].Id");

        Assert.Equal(0, result);
    }

    [Fact]
    public void NullDictionary_ComplexValueType_Comparison_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel { DebtorAccount = new DebtorAccount() };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.CustomerMap[\"key\"].Id is 1");

        Assert.False(result);
    }

    // ── Non-null dict with complex values – existing vs missing key ──

    [Fact]
    public void NonNullDictionary_ComplexValueType_ExistingKey_ShouldReturnCorrectValue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                CustomerMap = new Dictionary<string, Customer> { { "vip", new Customer { Id = 42, Status = 10 } } }
            }
        };

        var result = simpra.Execute<int, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "return DebtorAccount.CustomerMap[\"vip\"].Id");

        Assert.Equal(42, result);
    }

    [Fact]
    public void NonNullDictionary_ComplexValueType_MissingKey_ShouldReturnDefault()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                CustomerMap = new Dictionary<string, Customer> { { "vip", new Customer { Id = 42, Status = 10 } } }
            }
        };

        var result = simpra.Execute<int, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "return DebtorAccount.CustomerMap[\"unknown\"].Id");

        Assert.Equal(0, result);
    }

    // ── Null parent object in chain before dictionary ──

    [Fact]
    public void NullParentObject_DictionaryAccess_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount { Nested = null }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Nested.Properties[\"Key\"] is \"value\"");

        Assert.False(result);
    }

    [Fact]
    public void NullParentObject_DictionaryAccess_ReturnValue_ShouldReturnNull()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount { Nested = null }
        };

        var result = simpra.Execute<string, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "return DebtorAccount.Nested.Properties[\"Key\"]");

        Assert.Null(result);
    }

    // ── Nested dictionary on non-null parent ──

    [Fact]
    public void NestedObject_NullDictionary_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Nested = new DebtorAccount() // Nested.Properties is null
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Nested.Properties[\"Key\"] is \"value\"");

        Assert.False(result);
    }

    [Fact]
    public void NestedObject_NonNullDictionary_ExistingKey_ShouldReturnTrue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Nested = new DebtorAccount
                {
                    Properties = new Dictionary<string, string?> { { "Key", "value" } }
                }
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Nested.Properties[\"Key\"] is \"value\"");

        Assert.True(result);
    }

    // ── Null list access (non-dictionary indexable) ──

    [Fact]
    public void NullList_IndexAccess_ReturnValue_ShouldReturnNull()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount { Tags = null }
        };

        var result = simpra.Execute<string, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "return DebtorAccount.Tags[1]");

        Assert.Null(result);
    }

    [Fact]
    public void NullList_IndexAccess_Comparison_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount { Tags = null }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Tags[1] is \"hello\"");

        Assert.False(result);
    }

    // ── Null property on parent (non-dictionary, non-list) ──

    [Fact]
    public void NullProperty_MemberAccess_Comparison_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount { Name = null }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Name is \"Test\"");

        Assert.False(result);
    }

    [Fact]
    public void NullProperty_MemberAccess_HasValue_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount { Name = null }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Name has value");

        Assert.False(result);
    }

    [Fact]
    public void NonNullProperty_MemberAccess_HasValue_ShouldReturnTrue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount { Name = "Test" }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Name has value");

        Assert.True(result);
    }

    // ── Mixed: null dict combined with non-null property in same expression ──

    [Fact]
    public void NullDictionary_And_NonNullProperty_MixedExpression_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount { Name = "Test" }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" and DebtorAccount.Name is \"Test\"");

        Assert.False(result);
    }

    [Fact]
    public void NullDictionary_Or_NonNullProperty_MixedExpression_ShouldReturnTrue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount { Name = "Test" }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" or DebtorAccount.Name is \"Test\"");

        Assert.True(result);
    }

    // ── Null dictionary with "is not" operator ──

    [Fact]
    public void NullDictionary_IsNotComparison_ShouldReturnTrue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel { DebtorAccount = new DebtorAccount() };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is not \"200\"");

        Assert.True(result);
    }

    // ── Null dictionary with "in" operator ──

    [Fact]
    public void NullDictionary_InOperator_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel { DebtorAccount = new DebtorAccount() };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] in [\"200\", \"300\"]");

        Assert.False(result);
    }

    // ── Null dictionary in when/conditional expressions ──

    [Fact]
    public void NullDictionary_WhenExpression_ShouldEvaluateElseBranch()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel { DebtorAccount = new DebtorAccount() };

        var result = simpra.Execute<string, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "return when DebtorAccount.Properties[\"AccType\"] is \"200\" then \"match\" else \"no match\"");

        Assert.Equal("no match", result);
    }

    // ── Existing dictionary tests (TestModel.Countries) with null dict ──

    [Fact]
    public void TestModel_NullCountriesDictionary_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new TestModel
        {
            Transfer = new Transfer { Amount = 100, Currency = "USD" },
            Customer = new Customer { Id = 1, Status = 1 },
            Remittance = "Test",
            Countries = null
        };

        var result = simpra.Execute<bool, TestModel, TestFunctions>(
            model, new TestFunctions(),
            "Countries[\"Georgia\"] is \"Test\"");

        Assert.False(result);
    }

    [Fact]
    public void TestModel_NullCountriesDictionary_ReturnValue_ShouldReturnNull()
    {
        var simpra = new Simpra();
        var model = new TestModel
        {
            Transfer = new Transfer { Amount = 100, Currency = "USD" },
            Customer = new Customer { Id = 1, Status = 1 },
            Remittance = "Test",
            Countries = null
        };

        var result = simpra.Execute<string, TestModel, TestFunctions>(
            model, new TestFunctions(),
            "return Countries[\"Georgia\"]");

        Assert.Null(result);
    }

    [Fact]
    public void TestModel_NullDictionaryOfObjects_PropertyAccess_ShouldReturnDefault()
    {
        var simpra = new Simpra();
        var model = new TestModel
        {
            Transfer = new Transfer { Amount = 100, Currency = "USD" },
            Customer = new Customer { Id = 1, Status = 1 },
            Remittance = "Test",
            DictionaryOfObjects = null
        };

        var result = simpra.Execute<int, TestModel, TestFunctions>(
            model, new TestFunctions(),
            "return DictionaryOfObjects[\"test\"].Id");

        Assert.Equal(0, result);
    }

    [Fact]
    public void TestModel_NullDictionaryOfObjects_Comparison_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new TestModel
        {
            Transfer = new Transfer { Amount = 100, Currency = "USD" },
            Customer = new Customer { Id = 1, Status = 1 },
            Remittance = "Test",
            DictionaryOfObjects = null
        };

        var result = simpra.Execute<bool, TestModel, TestFunctions>(
            model, new TestFunctions(),
            "DictionaryOfObjects[\"test\"].Id is 1");

        Assert.False(result);
    }

    // ── Null Transfer (parent object) with sub-property access ──

    [Fact]
    public void NullTransfer_PropertyAccess_ShouldReturnDefault()
    {
        var simpra = new Simpra();
        var model = new TestModel
        {
            Transfer = null,
            Customer = new Customer { Id = 1, Status = 1 },
            Remittance = "Test"
        };

        var result = simpra.Execute<decimal, TestModel, TestFunctions>(
            model, new TestFunctions(),
            "return Transfer.Amount");

        Assert.Equal(0m, result);
    }

    [Fact]
    public void NullTransfer_CurrencyComparison_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new TestModel
        {
            Transfer = null,
            Customer = new Customer { Id = 1, Status = 1 },
            Remittance = "Test"
        };

        var result = simpra.Execute<bool, TestModel, TestFunctions>(
            model, new TestFunctions(),
            "Transfer.Currency is \"USD\"");

        Assert.False(result);
    }

    [Fact]
    public void NullTransfer_HasValue_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new TestModel
        {
            Transfer = null,
            Customer = new Customer { Id = 1, Status = 1 },
            Remittance = "Test"
        };

        var result = simpra.Execute<bool, TestModel, TestFunctions>(
            model, new TestFunctions(),
            "Transfer has value");

        Assert.False(result);
    }

    // ── Cross-dictionary and/or logic: Properties + Attributes ──

    [Fact]
    public void BothDictsPopulated_And_BothMatch_ShouldReturnTrue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", "200" } },
                Attributes = new Dictionary<string, string?> { { "Region", "EU" } }
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" and DebtorAccount.Attributes[\"Region\"] is \"EU\"");

        Assert.True(result);
    }

    [Fact]
    public void BothDictsPopulated_And_OneMismatch_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", "200" } },
                Attributes = new Dictionary<string, string?> { { "Region", "US" } }
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" and DebtorAccount.Attributes[\"Region\"] is \"EU\"");

        Assert.False(result);
    }

    [Fact]
    public void BothDictsPopulated_Or_OneMismatch_ShouldReturnTrue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", "200" } },
                Attributes = new Dictionary<string, string?> { { "Region", "US" } }
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" or DebtorAccount.Attributes[\"Region\"] is \"EU\"");

        Assert.True(result);
    }

    [Fact]
    public void BothDictsPopulated_Or_BothMismatch_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", "100" } },
                Attributes = new Dictionary<string, string?> { { "Region", "US" } }
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" or DebtorAccount.Attributes[\"Region\"] is \"EU\"");

        Assert.False(result);
    }

    [Fact]
    public void PropertiesNull_AttributesPopulated_And_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = null,
                Attributes = new Dictionary<string, string?> { { "Region", "EU" } }
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" and DebtorAccount.Attributes[\"Region\"] is \"EU\"");

        Assert.False(result);
    }

    [Fact]
    public void PropertiesNull_AttributesPopulated_Or_ShouldReturnTrue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = null,
                Attributes = new Dictionary<string, string?> { { "Region", "EU" } }
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" or DebtorAccount.Attributes[\"Region\"] is \"EU\"");

        Assert.True(result);
    }

    [Fact]
    public void PropertiesPopulated_AttributesNull_And_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", "200" } },
                Attributes = null
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" and DebtorAccount.Attributes[\"Region\"] is \"EU\"");

        Assert.False(result);
    }

    [Fact]
    public void PropertiesPopulated_AttributesNull_Or_ShouldReturnTrue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", "200" } },
                Attributes = null
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" or DebtorAccount.Attributes[\"Region\"] is \"EU\"");

        Assert.True(result);
    }

    [Fact]
    public void BothDictsNull_And_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel { DebtorAccount = new DebtorAccount() };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" and DebtorAccount.Attributes[\"Region\"] is \"EU\"");

        Assert.False(result);
    }

    [Fact]
    public void BothDictsNull_Or_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel { DebtorAccount = new DebtorAccount() };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" or DebtorAccount.Attributes[\"Region\"] is \"EU\"");

        Assert.False(result);
    }

    [Fact]
    public void BothDictsPopulated_MixedMissingKeys_And_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", "200" } },
                Attributes = new Dictionary<string, string?> { { "Other", "X" } }
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" and DebtorAccount.Attributes[\"Region\"] is \"EU\"");

        Assert.False(result);
    }

    [Fact]
    public void BothDictsPopulated_MixedMissingKeys_Or_ShouldReturnTrue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", "200" } },
                Attributes = new Dictionary<string, string?> { { "Other", "X" } }
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" or DebtorAccount.Attributes[\"Region\"] is \"EU\"");

        Assert.True(result);
    }

    [Fact]
    public void ThreeConditions_And_AllMatch_ShouldReturnTrue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", "200" } },
                Attributes = new Dictionary<string, string?> { { "Region", "EU" } },
                Name = "VIP"
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" and DebtorAccount.Attributes[\"Region\"] is \"EU\" and DebtorAccount.Name is \"VIP\"");

        Assert.True(result);
    }

    [Fact]
    public void ThreeConditions_And_MiddleNull_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = new Dictionary<string, string?> { { "AccType", "200" } },
                Attributes = null,
                Name = "VIP"
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" and DebtorAccount.Attributes[\"Region\"] is \"EU\" and DebtorAccount.Name is \"VIP\"");

        Assert.False(result);
    }

    [Fact]
    public void ThreeConditions_Or_OnlyLastMatches_ShouldReturnTrue()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = null,
                Attributes = null,
                Name = "VIP"
            }
        };

        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "DebtorAccount.Properties[\"AccType\"] is \"200\" or DebtorAccount.Attributes[\"Region\"] is \"EU\" or DebtorAccount.Name is \"VIP\"");

        Assert.True(result);
    }

    [Fact]
    public void MixedAndOr_NullDict_And_PopulatedDict_Or_Property_ShouldEvaluateCorrectly()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = null,
                Attributes = new Dictionary<string, string?> { { "Region", "EU" } },
                Name = "VIP"
            }
        };

        // (null_dict is "200" and attr is "EU") or name is "VIP" → (false and true) or true → true
        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "(DebtorAccount.Properties[\"AccType\"] is \"200\" and DebtorAccount.Attributes[\"Region\"] is \"EU\") or DebtorAccount.Name is \"VIP\"");

        Assert.True(result);
    }

    [Fact]
    public void MixedAndOr_NullDict_Or_PopulatedDict_And_Property_ShouldEvaluateCorrectly()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = null,
                Attributes = new Dictionary<string, string?> { { "Region", "EU" } },
                Name = "VIP"
            }
        };

        // (null_dict is "200" or attr is "EU") and name is "VIP" → (false or true) and true → true
        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "(DebtorAccount.Properties[\"AccType\"] is \"200\" or DebtorAccount.Attributes[\"Region\"] is \"EU\") and DebtorAccount.Name is \"VIP\"");

        Assert.True(result);
    }

    [Fact]
    public void MixedAndOr_AllNull_And_Property_ShouldReturnFalse()
    {
        var simpra = new Simpra();
        var model = new DebtorAccountModel
        {
            DebtorAccount = new DebtorAccount
            {
                Properties = null,
                Attributes = null,
                Name = null
            }
        };

        // (null is "200" or null is "EU") and null_name is "VIP" → (false or false) and false → false
        var result = simpra.Execute<bool, DebtorAccountModel, IFunctions>(
            model, new TestFunctions(),
            "(DebtorAccount.Properties[\"AccType\"] is \"200\" or DebtorAccount.Attributes[\"Region\"] is \"EU\") and DebtorAccount.Name is \"VIP\"");

        Assert.False(result);
    }
}
