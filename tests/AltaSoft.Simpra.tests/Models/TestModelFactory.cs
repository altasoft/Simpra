namespace AltaSoft.Simpra.Tests.Models;

internal static class TestModelFactory
{
    public static TestModel GetTestModel()
    {
        return new TestModel { Transfer = new Transfer { Amount = 100, Currency = "USD" }, Customer = new Customer { Id = 1, Status = 1 }, Remittance = "Test" };
    }
}
