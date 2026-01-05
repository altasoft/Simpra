namespace AltaSoft.Simpra.Tests.Models;

public class Transfer
{
    public decimal Amount { get; set; }
    public Customer2? Customer { get; set; }
    public required string Currency { get; set; }
    public List<int>? A { get; set; }
    public List<TypeWithInnerList>? OuterList { get; set; }
}

public class TypeWithInnerList
{
    public required List<int> InnerList { get; set; }
}
public class Customer2
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
