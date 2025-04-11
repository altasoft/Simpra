using System.Diagnostics.CodeAnalysis;

namespace AltaSoft.Simpra.Types;

internal interface ISimpraType
{
    public bool HasValue { get; }
}

internal interface ISimpraType<out T> : ISimpraType
{
    [NotNull]
    T Value { get; }

    static abstract ISimpraType<T> NoValue { get; }

}
