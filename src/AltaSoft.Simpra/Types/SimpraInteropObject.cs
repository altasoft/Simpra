namespace AltaSoft.Simpra.Types;

internal readonly struct SimpraInteropObject<T> : ISimpraType
{
    public T? Value { get; init; }
    public bool HasValue => Value is not null;

    public SimpraInteropObject(T? value) => Value = value;

    public bool Equals(SimpraInteropObject<T> other) => Equals(Value, other.Value);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is SimpraInteropObject<T> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    /// <inheritdoc />
    public override string? ToString() => Value?.ToString();
}
