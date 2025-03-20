using System;

namespace AltaSoft.Simpra.Metadata.Models;

internal sealed class TypeMetadata
{
    internal required Type ModelType { get; set; }
    internal required Type? ExternalFunctionsType { get; set; }
    internal required ModelInfo ModelInfo { get; set; }
}
