using AltaSoft.DomainPrimitives;
 
namespace AltaSoft.Simpra.Tests.Models;
 
public sealed partial class CountryCode : IDomainValue<string>
{
    /// <inheritdoc/>
    public static PrimitiveValidationResult Validate(string value)
    {
        if (value.Length != 2 || !char.IsAsciiLetterUpper(value[0]) || !char.IsAsciiLetterUpper(value[1]))
            return PrimitiveValidationResult.Error($"Invalid value for ISO 3166 country code. Input: '{value}'"); ;
        return PrimitiveValidationResult.Ok;
    }
}
