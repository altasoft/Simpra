using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace AltaSoft.Simpra;

internal static class StringComparisonContext
{
    private static readonly AsyncLocal<bool> s_isCaseSensitive = new() { Value = true };

    public static StringComparison GetStringComparison() => s_isCaseSensitive.Value ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

    public static RegexOptions GetRegexOptions() => s_isCaseSensitive.Value ? RegexOptions.None : RegexOptions.IgnoreCase;

    public static bool IsCaseSensitive
    {
        get => s_isCaseSensitive.Value;
        set => s_isCaseSensitive.Value = value;
    }
}
