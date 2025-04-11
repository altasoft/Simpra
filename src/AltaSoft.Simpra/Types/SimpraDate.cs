using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using AltaSoft.Simpra.Visitor;

namespace AltaSoft.Simpra.Types;

internal struct SimpraDate : ISimpraType<DateTime>
{
    public static ISimpraType<DateTime> NoValue { get; } = new SimpraDate((DateTime?)null);

    public DateTime Value { get; private set; }
    public bool HasValue { get; private set; }

    [MemberNotNull(nameof(Value))]
    private void SetValue(DateTime? value)
    {
        Value = value ?? DateTime.MinValue;
        HasValue = value is not null;
    }

    public SimpraDate(DateTime? value) => SetValue(value);
    public SimpraDate(DateTime value) => SetValue(value);
    public SimpraDate(DateTimeOffset? value) => SetValue(value?.DateTime);
    public SimpraDate(DateTimeOffset value) => SetValue(value.DateTime);
    public SimpraDate(TimeSpan? value) => SetValue(value.HasValue ? DateTime.MinValue.Add(value.Value) : null);
    public SimpraDate(TimeSpan value) => SetValue(DateTime.MinValue.Add(value));
    public SimpraDate(DateOnly? value) => SetValue(value?.ToDateTime(TimeOnly.MinValue, DateTimeKind.Local));
    public SimpraDate(DateOnly value) => SetValue(value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Local));
    public SimpraDate(TimeOnly? value) => SetValue(value.HasValue ? new DateTime(value.Value.Ticks, DateTimeKind.Local) : null);
    public SimpraDate(TimeOnly value) => SetValue(new DateTime(value.Ticks, DateTimeKind.Local));
    public SimpraDate(string? value) => SetValue(value is null ? null : DateTime.Parse(value, CultureInfo.InvariantCulture));

    public static implicit operator DateTime(SimpraDate value) => value.Value;
    public static implicit operator DateTimeOffset(SimpraDate value) => value.Value;
    public static implicit operator TimeSpan(SimpraDate value) => value.Value.TimeOfDay;
    public static implicit operator DateOnly(SimpraDate value) => DateOnly.FromDateTime(value.Value);
    public static implicit operator TimeOnly(SimpraDate value) => TimeOnly.FromDateTime(value.Value);

    public static implicit operator DateTime?(SimpraDate value) => value.HasValue ? value.Value : null;
    public static implicit operator DateTimeOffset?(SimpraDate value) => value.HasValue ? value.Value : null;
    public static implicit operator TimeSpan?(SimpraDate value) => value.HasValue ? value.Value.TimeOfDay : null;
    public static implicit operator DateOnly?(SimpraDate value) => value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    public static implicit operator TimeOnly?(SimpraDate value) => value.HasValue ? TimeOnly.FromDateTime(value.Value) : null;

    public static implicit operator SimpraDate(DateTime value) => new(value);
    public static implicit operator SimpraDate(DateTimeOffset value) => new(value);
    public static implicit operator SimpraDate(TimeSpan value) => new(value);
    public static implicit operator SimpraDate(DateOnly value) => new(value);
    public static implicit operator SimpraDate(TimeOnly value) => new(value);

    public static implicit operator SimpraDate(DateTime? value) => new(value);
    public static implicit operator SimpraDate(DateTimeOffset? value) => new(value);
    public static implicit operator SimpraDate(TimeSpan? value) => new(value);
    public static implicit operator SimpraDate(DateOnly? value) => new(value);
    public static implicit operator SimpraDate(TimeOnly? value) => new(value);

    public static SimpraDate operator +(SimpraDate a, SimpraDate b) => new(a.Value.AddTicks(b.Value.Ticks));
    public static SimpraDate operator -(SimpraDate a, SimpraDate b) => new(a.Value.AddTicks(-b.Value.Ticks));
    public static SimpraList<SimpraDate, DateTime> operator +(SimpraDate a, SimpraList<SimpraDate, DateTime> b) => new(b.Value.Concat([a]));

    public static bool operator ==(SimpraDate a, SimpraDate b) => a.Value == b.Value;
    public static bool operator !=(SimpraDate a, SimpraDate b) => !(a == b);

    public static bool operator >(SimpraDate a, SimpraDate b) => a.Value > b.Value;
    public static bool operator >=(SimpraDate a, SimpraDate b) => a.Value >= b.Value;
    public static bool operator <(SimpraDate a, SimpraDate b) => a.Value < b.Value;
    public static bool operator <=(SimpraDate a, SimpraDate b) => a.Value <= b.Value;

    //
    public static SimpraBool In(SimpraDate value, SimpraList<SimpraDate, DateTime> lookup) => lookup.Contains(value);

    public readonly SimpraDate Min(SimpraDate b) => Value.Ticks < b.Value.Ticks ? Value : b.Value;
    public readonly SimpraDate Max(SimpraDate b) => Value.Ticks > b.Value.Ticks ? Value : b.Value;

    public SimpraDate AddAndAssign(SimpraDate value) { SetValue(Value.AddTicks(value.Value.Ticks)); return this; }
    public SimpraDate SubtractAndAssign(SimpraDate value) { SetValue(Value.AddTicks(-value.Value.Ticks)); return this; }

    public readonly bool Equals(SimpraDate other) => Value == other.Value;

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return !HasValue;
        if (obj is SimpraDate simpraVar)
            return Equals(simpraVar);

        if (!obj.GetType().IsDate())
            return false;

        return Value == Convert.ToDateTime(obj, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc />
    public override readonly int GetHashCode() => HasValue ? Value.GetHashCode() : 0;

    /// <inheritdoc />
    public override readonly string? ToString() => HasValue ? Value.ToString(CultureInfo.InvariantCulture) : null;

    public readonly int CompareTo(SimpraDate other) => Value.CompareTo(other.Value);
}
