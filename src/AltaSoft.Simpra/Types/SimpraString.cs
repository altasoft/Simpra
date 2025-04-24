using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using AltaSoft.Simpra.Visitor;

namespace AltaSoft.Simpra.Types;

internal struct SimpraString : ISimpraType<string>
{
    public static ISimpraType<string> NoValue { get; } = new SimpraString((string?)null);
    public static SimpraString Empty { get; } = new(string.Empty);

    public string Value { get; private set; }
    public bool HasValue { get; private set; }

    [MemberNotNull(nameof(Value))]
    private void SetValue(string? value)
    {
        Value = value ?? string.Empty;
        HasValue = value is not null;
    }

    public SimpraString(string? value) => SetValue(value);
    public SimpraString(char? value) => SetValue(value?.ToString());

    public static implicit operator string?(SimpraString value) => value.HasValue ? value.Value : null;

    public static implicit operator SimpraString(string? value) => new(value);

    public static implicit operator char?(SimpraString value) => value.HasValue ? (value.Value.Length > 0 ? value.Value[0] : '\0') : null;

    public static implicit operator SimpraString(char? value) => new(value);

    public static SimpraString operator +(SimpraString a, SimpraString b) => new(a.Value + b.Value);
    public static SimpraString operator *(SimpraString a, SimpraNumber b) => a.HasValue && b.HasValue
        ? new SimpraString(string.Concat(Enumerable.Repeat(a.Value, (int)b.Value)))
        : a;
    public static SimpraList<SimpraString, string> operator /(SimpraString a, SimpraString b) => a.HasValue && b.HasValue
        ? new SimpraList<SimpraString, string>(a.Value.Split(b.Value).Select(x => new SimpraString(x)))
        : new SimpraList<SimpraString, string>(null);

    public static SimpraString operator +(SimpraString a, SimpraBool b) => new(a.Value + b.Value.ToString(CultureInfo.InvariantCulture));
    public static SimpraString operator +(SimpraString a, SimpraNumber b) => new(a.Value + b.Value.ToString(CultureInfo.InvariantCulture));
    public static SimpraList<SimpraString, string> operator +(SimpraString a, SimpraList<SimpraString, string> b) => new(b.Value.Concat([a]));

    public static bool operator ==(SimpraString a, SimpraString b) => a.Value.Equals(b.Value, StringComparisonContext.GetStringComparison());
    public static bool operator !=(SimpraString a, SimpraString b) => !(a == b);

    public static bool operator ==(string a, SimpraString b) => b.Value.Equals(a, StringComparisonContext.GetStringComparison());
    public static bool operator !=(string a, SimpraString b) => !(a == b);

    public static bool operator ==(SimpraString a, string b) => a.Value.Equals(b, StringComparisonContext.GetStringComparison());
    public static bool operator !=(SimpraString a, string b) => !(a == b);

    //
    public static SimpraBool In(SimpraString value, SimpraList<SimpraString, string> lookup) => lookup.Contains(value);

    public readonly SimpraString Min(SimpraString b) => new(string.Compare(Value, b.Value, StringComparisonContext.GetStringComparison()) < 0 ? Value : b.Value);
    public readonly SimpraString Max(SimpraString b) => new(string.Compare(Value, b.Value, StringComparisonContext.GetStringComparison()) > 0 ? Value : b.Value);

    public SimpraString AddAndAssign(SimpraString value) { SetValue(Value + value.Value); return this; }
    //public SimpraString SubtractAndAssign(SimpraString value) { SetValue(Value - value.Value); return this; }

    public static SimpraString Substring(SimpraString value, SimpraNumber index, SimpraNumber count)
    {
        if (!value.HasValue)
            return value;

        var startIndex = (int)index.Value;
        var length = (int)count.Value;

        return startIndex < 0 || length < 0 || startIndex + length > value.Value.Length
            ? throw new InvalidOperationException("The index and count must be within the bounds of the string")
            : new SimpraString(value.Value.Substring(startIndex, length));
    }

    public static SimpraString Substring(SimpraString value, SimpraNumber index) => !value.HasValue ? value : new SimpraString(value.Value.Substring((int)index.Value, 1));

    public static SimpraBool Like(SimpraString value, SimpraString pattern)
    {
        if (!value.HasValue || !pattern.HasValue)
            return false;

        // Escape regex special characters in the pattern
        var p = "^" + Regex.Escape(pattern.Value)
                        .Replace("%", ".*")  // Convert % to .*
                        .Replace("_", ".")   // Convert _ to .
                    + "$";

        var options = StringComparisonContext.GetRegexOptions();
        return Regex.IsMatch(value.Value, p, options);
    }

    public static SimpraBool Matches(SimpraString value, SimpraString pattern)
    {
        if (!value.HasValue || !pattern.HasValue)
            return false;

        var options = StringComparisonContext.GetRegexOptions();
        return Regex.IsMatch(value.Value, pattern.Value, options);
    }

    public static TEnum ToEnum<TEnum>(SimpraString value) where TEnum : struct, Enum
    {
        var isNullable = typeof(TEnum).IsNullableT();

        return value.HasValue
            ? Enum.Parse<TEnum>(value.Value)
            : isNullable
            ? default
            : throw new InvalidOperationException($"Cannot convert null to non-nullable enum of type '{typeof(TEnum).Name}'");
    }

    public static TEnum? ToNullableEnum<TEnum>(SimpraString value)
    {
        var type = typeof(TEnum).IsNullableT(out var underlyingType) ? underlyingType : typeof(TEnum);

        return value.HasValue ? (TEnum?)Enum.Parse(type, value.Value) : default;
    }
    public readonly bool Equals(SimpraString other) => Value == other.Value;

    /// <inheritdoc />
    public override readonly bool Equals(object? obj) => obj switch
    {
        null => !HasValue,
        SimpraString simpraVar => Equals(simpraVar),
        string s => string.Equals(Value, s, StringComparisonContext.GetStringComparison()),
        _ => obj is char c && string.Equals(Value, c.ToString(), StringComparisonContext.GetStringComparison())
    };

    /// <inheritdoc />
    public override readonly int GetHashCode() => HasValue ? Value.GetHashCode() : 0;

    /// <inheritdoc />
    public override readonly string? ToString() => HasValue ? Value : null;
}
