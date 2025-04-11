using System;
using System.Collections.Generic;
using System.Linq;
using AltaSoft.Simpra.Types;

namespace AltaSoft.Simpra;

internal static class InternalLanguageFunctions
{
    // ReSharper disable InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles

    public static SimpraNumber length(SimpraString input) => new(input.Value.Length);
    public static SimpraNumber length(SimpraString[] input) => new(input.Length);
    public static SimpraNumber length(SimpraNumber[] input) => new(input.Length);
    public static SimpraNumber length(SimpraBool[] input) => new(input.Length);
    public static SimpraNumber length(SimpraList<SimpraBool, bool> input) => new(input.Value.Count);
    public static SimpraNumber length(SimpraList<SimpraDate, DateTime> input) => new(input.Value.Count);
    public static SimpraNumber length(SimpraList<SimpraNumber, decimal> input) => new(input.Value.Count);
    public static SimpraNumber length(SimpraList<SimpraString, string> input) => new(input.Value.Count);

    public static SimpraNumber round(SimpraNumber input) => input.HasValue ? new SimpraNumber(Math.Round(input.Value, 0)) : input;
    public static SimpraNumber round(SimpraNumber input, SimpraNumber decimals) => input.HasValue ? new SimpraNumber(Math.Round(input.Value, (int)decimals.Value)) : input;

    public static SimpraNumber abs(SimpraNumber input) => input.HasValue ? new SimpraNumber(Math.Abs(input.Value)) : input;
    public static SimpraNumber sum(SimpraList<SimpraNumber, decimal> input) => input.HasValue ? input.Value.Sum(x => x.Value) : (SimpraNumber)SimpraNumber.NoValue;

    public static SimpraString @string(SimpraNumber input) => new(input.ToString());
    public static SimpraString @string(SimpraBool input) => new(input.ToString());
    public static SimpraString @string(SimpraDate input) => new(input.ToString());

    public static SimpraNumber number(SimpraString input) => new(input.Value);
    public static SimpraNumber number(SimpraBool input) => new(input.Value);

    public static SimpraDate date(SimpraString input) => new(input.Value);

#pragma warning restore IDE1006 // Naming Styles
    // ReSharper enable InconsistentNaming

    /// <summary>
    /// Built-in functions that are available in the Simpra language.
    /// With 1 string parameter.
    /// </summary>
    private static readonly HashSet<string> s_builtInFunctions = [
        nameof(length),
        nameof(round),
        nameof(abs),
        nameof(sum),
        nameof(@string),
        nameof(@number),
        nameof(@date)
    ];

    internal static bool IsSimpraBuiltInFunction(this string name) => s_builtInFunctions.Contains(name);
}
