using System;
using System.Collections.Generic;
using System.Linq;

namespace AltaSoft.Simpra.Types;

internal readonly struct SimpraList<TSimpraType, TNetType> : ISimpraType<List<TSimpraType>>
    where TSimpraType : ISimpraType<TNetType>
{
    public List<TSimpraType> Value { get; }
    public bool HasValue { get; }

    public SimpraList(List<TSimpraType>? value)
    {
        Value = value ?? [];
        HasValue = value is not null;
    }

    public SimpraList(IEnumerable<TSimpraType>? value) : this(value?.ToList())
    {
    }

    // Sets or Gets the element at the given index.
    public TSimpraType this[SimpraNumber index]
    {
        get => Value[index - 1];
        set => Value[index - 1] = value;
    }

    public static implicit operator string[]?(SimpraList<TSimpraType, TNetType> value) => value.HasValue ? value.Value.Select(x => x?.ToString() ?? string.Empty).ToArray() : null;
    public static implicit operator List<string>?(SimpraList<TSimpraType, TNetType> value) => value.HasValue ? value.Value.ConvertAll(x => x?.ToString() ?? string.Empty) : null;

    // Union
    public static SimpraList<TSimpraType, TNetType> operator +(SimpraList<TSimpraType, TNetType> a, SimpraList<TSimpraType, TNetType> b) => new(a.Value.Concat(b.Value));
    // Difference (Except), [1, 2, 3] - [2, 3] = [1]
    public static SimpraList<TSimpraType, TNetType> operator -(SimpraList<TSimpraType, TNetType> a, SimpraList<TSimpraType, TNetType> b) => new(a.Value.Except(b.Value));
    // Intersection, [1, 2, 3] * [2, 3] = [2, 3]
    public static SimpraList<TSimpraType, TNetType> operator *(SimpraList<TSimpraType, TNetType> a, SimpraList<TSimpraType, TNetType> b) => new(a.Value.Intersect(b.Value));
    public static SimpraList<TSimpraType, TNetType> operator /(SimpraList<TSimpraType, TNetType> a, SimpraList<TSimpraType, TNetType> b) => throw new InvalidOperationException("Cannot divide enumerations");

    public static SimpraList<TSimpraType, TNetType> operator +(SimpraList<TSimpraType, TNetType> a, TSimpraType b) => new(a.Value.Concat([b]));

    public static bool operator ==(SimpraList<TSimpraType, TNetType> a, SimpraList<TSimpraType, TNetType> b) => a.Value.SequenceEqual(b.Value);
    public static bool operator !=(SimpraList<TSimpraType, TNetType> a, SimpraList<TSimpraType, TNetType> b) => !(a == b);

    public SimpraList<TSimpraType, TNetType> AddAndAssign(TSimpraType value) { Value.Add(value); return this; }
    public SimpraList<TSimpraType, TNetType> AddAndAssign(SimpraList<TSimpraType, TNetType> value) { Value.AddRange(value.Value); return this; }

    public SimpraList<TSimpraType, TNetType> SubtractAndAssign(TSimpraType value) { Value.Remove(value); return this; }
    public SimpraList<TSimpraType, TNetType> SubtractAndAssign(SimpraList<TSimpraType, TNetType> value) { Value.RemoveAll(value.Contains); return this; }

    /// <summary>
    /// Determines if any element in the first list is contained in the second list.
    /// </summary>
    /// <returns>A <see cref="SimpraBool"/> indicating whether any element in the first list is contained in the second list.</returns>
    public static SimpraBool Any(SimpraList<TSimpraType, TNetType> a, SimpraList<TSimpraType, TNetType> b) => a.Value.Exists(b.Value.Contains);

    /// <summary>
    /// Determines if all elements in the first list are contained in the second list.
    /// </summary>
    /// <returns>A <see cref="SimpraBool"/> indicating whether all elements in the first list are contained in the second list.</returns>
    public static SimpraBool All(SimpraList<TSimpraType, TNetType> a, SimpraList<TSimpraType, TNetType> b) => a.Value.All(b.Value.Contains);

    public bool Equals(SimpraList<TSimpraType, TNetType> other) => Equals(Value, other.Value);

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not SimpraList<TSimpraType, TNetType> list)
            return false;

        if (!HasValue && !HasValue)
            return true;

        return HasValue && HasValue && Value.SequenceEqual(list.Value);
    }

    /// <inheritdoc />
    public override int GetHashCode() => HasValue ? Value.GetHashCode() : 0;

    /// <inheritdoc />
    public override string ToString() => !HasValue
        ? "[]"
        : $"[{string.Join(", ", Value.Select(x => x.ToString() ?? "null"))}]";

    internal bool Contains(TSimpraType value) => Value.Any(x => x.Value.Equals(value.Value));
}
