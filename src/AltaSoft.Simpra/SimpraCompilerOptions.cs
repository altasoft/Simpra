namespace AltaSoft.Simpra;

/// <summary>
/// Specifies the mutability options for the compiler.
/// </summary>
public enum MutabilityOption
{
    /// <summary>
    /// Indicates that the object is immutable and cannot be made mutable. Default.
    /// </summary>
    Immutable,

    /// <summary>
    /// Indicates that the object is immutable by default, but can be made mutable.
    /// </summary>
    DefaultImmutable,

    /// <summary>
    /// Indicates that the object is mutable by default. Can be made immutable.
    /// </summary>
    DefaultMutable
}

/// <summary>
/// Specifies the string comparison options for the compiler.
/// </summary>
public enum StringComparisonOption
{
    /// <summary>
    /// Indicates that the string comparison is case-sensitive.
    /// </summary>
    CaseSensitive,

    /// <summary>
    /// Indicates that the string comparison ignores case.
    /// </summary>
    IgnoreCase
}

/// <summary>
/// Represents the compiler options.
/// Default values are:
/// CaseSensitive, Immutable
/// </summary>
public class SimpraCompilerOptions
{
    /// <summary>
    /// Gets or sets the string comparison option.
    /// </summary>
    public StringComparisonOption StringComparisonOption { get; set; } = StringComparisonOption.CaseSensitive;

    /// <summary>
    /// Gets or sets the mutability option.
    /// </summary>
    public MutabilityOption MutabilityOption { get; set; } = MutabilityOption.Immutable;
}

/// <summary>
/// Represents the active compiler options.
/// </summary>
internal sealed class ActiveCompilerOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the string comparison is case-sensitive.
    /// </summary>
    public bool IsCaseSensitive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the object is mutable.
    /// </summary>
    public bool IsMutable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the object can be made mutable.
    /// </summary>
    public bool CanEnableMutable { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActiveCompilerOptions"/> class.
    /// </summary>
    /// <param name="compilerOptions">The compiler options.</param>
    public ActiveCompilerOptions(SimpraCompilerOptions? compilerOptions)
    {
        IsCaseSensitive = true;
        IsMutable = false;
        CanEnableMutable = false;

        if (compilerOptions is null)
            return;

        IsCaseSensitive = compilerOptions.StringComparisonOption != StringComparisonOption.IgnoreCase;
        IsMutable = compilerOptions.MutabilityOption == MutabilityOption.DefaultMutable;
        CanEnableMutable = compilerOptions.MutabilityOption != MutabilityOption.Immutable;
    }
}
