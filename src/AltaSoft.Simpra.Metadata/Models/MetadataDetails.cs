using System;

namespace AltaSoft.Simpra.Metadata.Models;

/// <summary>
/// Represents the metadata details for a model.
/// </summary>
public sealed class MetadataDetails
{
    /// <summary>
    /// Gets the name of the metadata. if null ModelType.Name will be used
    /// </summary>
    public required string? Name { get; init; }

    /// <summary>
    /// Gets the type of the model.
    /// </summary>
    public required Type ModelType { get; init; }

    /// <summary>
    /// Gets the type of the functions associated with the model, if any.
    /// </summary>
    public Type? ExternalFunctionsType { get; init; }
}
