namespace AltaSoft.Simpra.Tests.Models
{
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
}
