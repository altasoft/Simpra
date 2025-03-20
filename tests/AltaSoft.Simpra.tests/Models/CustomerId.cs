using AltaSoft.DomainPrimitives;

namespace AltaSoft.Simpra.Tests.Models;

public partial struct CustomerId : IDomainValue<int>
{
    public static PrimitiveValidationResult Validate(int value) => PrimitiveValidationResult.Ok;
}
