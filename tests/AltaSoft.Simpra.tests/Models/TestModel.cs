namespace AltaSoft.Simpra.Tests.Models;

public class ListModel
{
    public required List<int> IntegerList { get; set; }

    public required List<string> StringList { get; set; }
    public required List<Color> EnumList { get; set; }
    public List<Customer>? ComplexList { get; set; }
    public List<List<int>>? ListOfList { get; set; }
    public int[]? IntegerArray { get; set; }
    public IEnumerable<int> IntegerEnumerable { get; set; } = [];
}

public class TestModel
{
    public required Transfer? Transfer { get; set; }
    public required Customer Customer { get; set; }
    public required string Remittance { get; set; }

    public Color Color { get; set; } = Color.Green;
    public ColorX ColorX { get; set; } = ColorX.Green;
    public ColorM ColorM { get; set; } = ColorM.Green;
    public CustomerId CustomerId { get; set; } = 11;
    public int? Nint1 { get; set; }
    public short Nint2 { get; set; } = 20;
    public Xy Xy { get; set; } = new() { X = 1, Y = 2 };
    public string Ccy { get; set; } = "GEL";
    public IEnumerable<short> Values { get; set; } = [1, 2, 3];
    public NonNegativeAmount? NullableAmount { get; set; }
    public int? NullableIntegerProperty { get; set; }
    public List<Customer2> CustomerList { get; set; }

}

public class Iso20022TransferModel
{
    public required TransferAd Transfer { get; set; }

    public static Iso20022TransferModel CreateForCountry(string country)
    {
        return new Iso20022TransferModel
        {
            Transfer = new TransferAd
            {
                RegulatoryReporting = [new() { Authority = new() { Country = country } }]
            }
        };
    }
}
public class TransferAd
{
    public required List<RegulatoryReporting> RegulatoryReporting { get; set; }
}

public class RegulatoryReporting
{
    public required Authority Authority { get; set; }
}

public class Authority
{
    public required string Country { get; set; }
}
