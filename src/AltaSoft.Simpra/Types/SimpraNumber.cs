using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using AltaSoft.Simpra.Visitor;

namespace AltaSoft.Simpra.Types;

internal struct SimpraNumber : ISimpraType<decimal>
{
    public static SimpraNumber Zero { get; } = new(0m);
    public static SimpraNumber NoValue { get; } = new((decimal?)null);

    public decimal Value { get; private set; }
    public bool HasValue { get; private set; }

    [MemberNotNull(nameof(Value))]
    private void SetValue(decimal? value)
    {
        Value = value ?? 0m;
        HasValue = value is not null;
    }

    public SimpraNumber(decimal? value) => SetValue(value);
    public SimpraNumber(decimal value) => SetValue(value);
    public SimpraNumber(bool? value) => SetValue(value.HasValue ? (value.Value ? 1 : 0) : null);
    public SimpraNumber(bool value) => SetValue(value ? 1 : 0);
    public SimpraNumber(string? value) => SetValue(value is null ? null : decimal.Parse(value, CultureInfo.InvariantCulture));

    public static implicit operator sbyte(SimpraNumber value) => (sbyte)value.Value;
    public static implicit operator byte(SimpraNumber value) => (byte)value.Value;
    public static implicit operator short(SimpraNumber value) => (short)value.Value;
    public static implicit operator ushort(SimpraNumber value) => (ushort)value.Value;
    public static implicit operator int(SimpraNumber value) => (int)value.Value;
    public static implicit operator uint(SimpraNumber value) => (uint)value.Value;
    public static implicit operator long(SimpraNumber value) => (long)value.Value;
    public static implicit operator ulong(SimpraNumber value) => (ulong)value.Value;
    public static implicit operator float(SimpraNumber value) => (float)value.Value;
    public static implicit operator double(SimpraNumber value) => (double)value.Value;
    public static implicit operator decimal(SimpraNumber value) => value.Value;
    public static implicit operator bool(SimpraNumber value) => value.Value != 0m;

    public static implicit operator sbyte?(SimpraNumber value) => (sbyte?)value.Value;
    public static implicit operator byte?(SimpraNumber value) => (byte?)value.Value;
    public static implicit operator short?(SimpraNumber value) => (short?)value.Value;
    public static implicit operator ushort?(SimpraNumber value) => (ushort?)value.Value;
    public static implicit operator int?(SimpraNumber value) => (int?)value.Value;
    public static implicit operator uint?(SimpraNumber value) => (uint?)value.Value;
    public static implicit operator long?(SimpraNumber value) => (long?)value.Value;
    public static implicit operator ulong?(SimpraNumber value) => (ulong?)value.Value;
    public static implicit operator float?(SimpraNumber value) => (float?)value.Value;
    public static implicit operator double?(SimpraNumber value) => (double?)value.Value;
    public static implicit operator decimal?(SimpraNumber value) => value.Value;
    public static implicit operator bool?(SimpraNumber value) => value.HasValue ? value.Value != 0m : null;

    public static implicit operator SimpraNumber(sbyte value) => new(value);
    public static implicit operator SimpraNumber(byte value) => new(value);
    public static implicit operator SimpraNumber(short value) => new(value);
    public static implicit operator SimpraNumber(ushort value) => new(value);
    public static implicit operator SimpraNumber(int value) => new(value);
    public static implicit operator SimpraNumber(uint value) => new(value);
    public static implicit operator SimpraNumber(long value) => new(value);
    public static implicit operator SimpraNumber(ulong value) => new(value);
    public static implicit operator SimpraNumber(float value) => new((decimal)value);
    public static implicit operator SimpraNumber(double value) => new((decimal)value);
    public static implicit operator SimpraNumber(decimal value) => new(value);
    public static implicit operator SimpraNumber(bool value) => new(value ? 1m : 0m);

    public static implicit operator SimpraNumber(sbyte? value) => new(value);
    public static implicit operator SimpraNumber(byte? value) => new(value);
    public static implicit operator SimpraNumber(short? value) => new(value);
    public static implicit operator SimpraNumber(ushort? value) => new(value);
    public static implicit operator SimpraNumber(int? value) => new(value);
    public static implicit operator SimpraNumber(uint? value) => new(value);
    public static implicit operator SimpraNumber(long? value) => new(value);
    public static implicit operator SimpraNumber(ulong? value) => new(value);
    public static implicit operator SimpraNumber(float? value) => new((decimal?)value);
    public static implicit operator SimpraNumber(double? value) => new((decimal?)value);
    public static implicit operator SimpraNumber(decimal? value) => new(value);
    public static implicit operator SimpraNumber(bool? value) => new(value.HasValue ? (value.Value ? 1m : 0m) : null);

    public static SimpraNumber operator -(SimpraNumber a) => new(-a.Value);
    public static SimpraNumber operator +(SimpraNumber a) => a;

    public static SimpraNumber operator +(SimpraNumber a, SimpraNumber b) => new(a.Value + b.Value);
    public static SimpraNumber operator -(SimpraNumber a, SimpraNumber b) => new(a.Value - b.Value);
    public static SimpraNumber operator *(SimpraNumber a, SimpraNumber b) => new(a.Value * b.Value);
    public static SimpraNumber operator /(SimpraNumber a, SimpraNumber b) => new(a.Value / b.Value);

    public static SimpraList<SimpraNumber, decimal> operator +(SimpraNumber a, SimpraList<SimpraNumber, decimal> b) => new(b.Value.Concat([a]));

    public static bool operator ==(SimpraNumber a, SimpraNumber b) => a.Value == b.Value;
    public static bool operator !=(SimpraNumber a, SimpraNumber b) => !(a == b);

    public static bool operator >(SimpraNumber a, SimpraNumber b) => a.Value > b.Value;
    public static bool operator >=(SimpraNumber a, SimpraNumber b) => a.Value >= b.Value;
    public static bool operator <(SimpraNumber a, SimpraNumber b) => a.Value < b.Value;
    public static bool operator <=(SimpraNumber a, SimpraNumber b) => a.Value <= b.Value;

    //
    public static SimpraBool In(SimpraNumber value, SimpraList<SimpraNumber, decimal> lookup, bool _) => lookup.Contains(value);

    public readonly SimpraNumber Min(SimpraNumber b) => new(Math.Min(Value, b.Value));
    public readonly SimpraNumber Max(SimpraNumber b) => new(Math.Max(Value, b.Value));

    public SimpraNumber AddAndAssign(SimpraNumber value) { SetValue(Value + value.Value); return this; }
    public SimpraNumber SubtractAndAssign(SimpraNumber value) { SetValue(Value - value.Value); return this; }

    public readonly bool Equals(SimpraNumber other) => Value == other.Value;

    /// <inheritdoc />
    public override readonly bool Equals(object? obj)
    {
        if (obj is null)
            return !HasValue;
        if (obj is SimpraDate simpraVar)
            return Equals(simpraVar);

        return obj.GetType().IsNumber() && Value == Convert.ToDecimal(obj, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc />
    public override readonly int GetHashCode() => HasValue ? Value.GetHashCode() : 0;

    /// <inheritdoc />
    public override readonly string? ToString() => HasValue ? Value.ToString(CultureInfo.InvariantCulture) : null;

    public readonly int CompareTo(SimpraNumber other) => Value.CompareTo(other.Value);
}
