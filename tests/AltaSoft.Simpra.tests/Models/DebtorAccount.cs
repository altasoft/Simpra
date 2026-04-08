namespace AltaSoft.Simpra.Tests.Models;

public sealed class DebtorAccountModel
{
    public required DebtorAccount DebtorAccount { get; set; }
}

public sealed class DebtorAccount
{
    public Dictionary<string, string?>? Properties { get; set; }
    public Dictionary<string, string?>? Attributes { get; set; }
    public Dictionary<int, string>? IntKeyedProperties { get; set; }
    public Dictionary<string, Customer>? CustomerMap { get; set; }
    public DebtorAccount? Nested { get; set; }
    public string? Name { get; set; }
    public List<string>? Tags { get; set; }
}
