using AltaSoft.DomainPrimitives;

namespace AltaSoft.Simpra.Tests.Models
{
    public sealed partial class Max35Text : IDomainValue<string>
    {
        public static PrimitiveValidationResult Validate(string value) => PrimitiveValidationResult.Ok;
    }
}
