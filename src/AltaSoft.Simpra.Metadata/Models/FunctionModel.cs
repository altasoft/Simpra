using System.Collections.Generic;

namespace AltaSoft.Simpra.Metadata.Models;

/// <summary>
/// Represents a function model with a name, return type, description, and parameters.
/// </summary>
public sealed class FunctionModel
{
    /// <summary>
    /// Gets or sets the name of the function.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the return type of the function.
    /// </summary>
    public required string ReturnType { get; set; }

    /// <summary>
    /// Gets or sets the description of the function.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the list of parameters for the function.
    /// </summary>
    public required List<string> Parameters { get; set; }
}
