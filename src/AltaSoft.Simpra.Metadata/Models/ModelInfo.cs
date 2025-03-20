using System.Collections.Generic;

namespace AltaSoft.Simpra.Metadata.Models;

/// <summary>
/// This class represents a type with detailed information about its functions and properties.
/// </summary>
public sealed class ModelInfo
{
    /// <summary>
    /// Name of the object
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// List of properties associated with the type.
    /// </summary>
    public List<PropertyModel> Properties { get; init; } = [];

    /// <summary>
    /// List of functions associated with the type.
    /// </summary>
    public List<FunctionModel> Functions { get; init; } = [];
}
