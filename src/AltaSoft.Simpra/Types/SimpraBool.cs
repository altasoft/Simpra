using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using AltaSoft.Simpra.Visitor;

namespace AltaSoft.Simpra.Types;

internal struct SimpraBool : ISimpraType<bool>
{
    public static SimpraBool False { get; } = new(false);
    public static SimpraBool True { get; } = new(true);
    public static SimpraBool NoValue { get; } = new((bool?)null);

    public bool Value { get; private set; }
    public bool HasValue { get; private set; }

    [MemberNotNull(nameof(Value))]
    private void SetValue(bool? value)
    {
        Value = value ?? false;
        HasValue = value is not null;
    }

    public SimpraBool(bool? value) => SetValue(value);
    public SimpraBool(bool value) => SetValue(value);
    public SimpraBool(string? value) => SetValue(value is null ? null : value is "true" or "1" or "TRUE" or "YES");

    public readonly int CompareTo(SimpraBool other) => Value.CompareTo(other.Value);

    public static implicit operator bool(SimpraBool simpraBool) => simpraBool.Value;
    public static implicit operator bool?(SimpraBool simpraBool) => simpraBool.HasValue ? simpraBool.Value : null;

    public static implicit operator SimpraBool(bool value) => new(value);
    public static implicit operator SimpraBool(bool? value) => new(value);

    public static SimpraBool operator !(SimpraBool simpraBool) => new(!simpraBool.Value);
    public static SimpraBool operator +(SimpraBool a, SimpraBool b) => new(a.Value || b.Value); // Logical OR
    public static SimpraBool operator -(SimpraBool a, SimpraBool b) => new(a.Value && !b.Value); // Logical AND NOT (a AND NOT b)
    public static SimpraBool operator *(SimpraBool a, SimpraBool b) => new(a.Value && b.Value); // Logical AND
    public static SimpraBool operator /(SimpraBool a, SimpraBool b) => new(!(a.Value && b.Value)); // Logical NAND (NOT (a AND b))

    public static SimpraBool operator |(SimpraBool a, SimpraBool b) => new(a.Value || b.Value);
    public static SimpraBool operator |(SimpraBool a, bool b) => new(a.Value || b);
    public static SimpraBool operator |(bool a, SimpraBool b) => new(a || b.Value);

    public static SimpraBool operator &(SimpraBool a, SimpraBool b) => new(a.Value && b.Value);
    public static SimpraBool operator &(SimpraBool a, bool b) => new(a.Value && b);
    public static SimpraBool operator &(bool a, SimpraBool b) => new(a && b.Value);

    public static SimpraList<SimpraBool, bool> operator +(SimpraBool a, SimpraList<SimpraBool, bool> b) => new(b.Value.Concat([a]));

    public static bool operator ==(SimpraBool a, SimpraBool b) => a.Value == b.Value;
    public static bool operator !=(SimpraBool a, SimpraBool b) => !(a == b);

    public static bool operator ==(bool a, SimpraBool b) => a == b.Value;
    public static bool operator !=(bool a, SimpraBool b) => !(a == b);

    public static bool operator ==(SimpraBool a, bool b) => a.Value == b;
    public static bool operator !=(SimpraBool a, bool b) => !(a == b);

    //
    public static SimpraBool In(SimpraBool value, SimpraList<SimpraBool, bool> lookup) => lookup.Contains(value);

    public readonly SimpraBool Min(SimpraBool b) => new(Value && b.Value);
    public readonly SimpraBool Max(SimpraBool b) => new(Value || b.Value);

    public SimpraBool AddAndAssign(SimpraBool value) { SetValue(Value & value.Value); return this; }
    public SimpraBool SubtractAndAssign(SimpraBool value) { SetValue(Value | value.Value); return this; }

    public readonly bool Equals(SimpraBool other) => Value == other.Value;

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return !HasValue;
        if (obj is SimpraBool simpraVar)
            return Equals(simpraVar);

        if (!obj.GetType().IsBool())
            return false;

        return Value == Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc />
    public override readonly int GetHashCode() => HasValue ? Value.GetHashCode() : 0;

    /// <inheritdoc />
    public override readonly string? ToString() => HasValue ? Value.ToString(CultureInfo.InvariantCulture) : null;
}
