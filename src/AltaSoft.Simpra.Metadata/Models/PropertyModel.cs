namespace AltaSoft.Simpra.Metadata.Models;

/// <summary>
/// Represents a model for a property, with details about its characteristics.
/// </summary>
public sealed class PropertyModel
{
    /// <summary>
    /// Gets or sets the name of the property.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the property, providing additional details about its purpose or use.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the type of the property, indicating its data type (e.g., string, integer, etc.).
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the property is required.
    /// </summary>
    public required bool Required { get; set; }
}
