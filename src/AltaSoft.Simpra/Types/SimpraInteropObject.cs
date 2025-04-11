namespace AltaSoft.Simpra.Types;

internal readonly struct SimpraInteropObject<T> : ISimpraType<T>
{
    public T Value { get; init; }
    public static ISimpraType<T> NoValue => new SimpraInteropObject<T>(default);
    public bool HasValue { get; }

    public SimpraInteropObject(T? value)
    {
        Value = value ?? default!;
        HasValue = value is not null;
    }

    public bool Equals(SimpraInteropObject<T> other) => Equals(Value, other.Value);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is SimpraInteropObject<T> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    /// <inheritdoc />
    public override string? ToString() => Value?.ToString();
}
