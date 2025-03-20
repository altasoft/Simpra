using AltaSoft.DomainPrimitives;

namespace AltaSoft.Simpra.Tests.Models;

public readonly partial struct NonNegativeAmount : IDomainValue<decimal>
{
    public static PrimitiveValidationResult Validate(decimal value)
    {
        return value < 0m ? "Value must be non negative" : PrimitiveValidationResult.Ok;
    }
}
