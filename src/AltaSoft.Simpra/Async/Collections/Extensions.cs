using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace AltaSoft.Simpra.Async.Collections;

internal static class Extensions
{
    [Pure]
    internal static ReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> that) => that as ReadOnlyCollection<T> ?? (that is T[] array ? Array.AsReadOnly(array) : that.ToList().AsReadOnly());

    [Pure]
    internal static bool TryFirst<TItem>(this IEnumerable<TItem> that, [NotNullWhen(true)] out TItem? item)
    {
        using var enumerator = that.GetEnumerator();
        if (enumerator.MoveNext())
        {
            item = enumerator.Current!;
            return true;
        }
        item = default;
        return false;
    }

    [Pure]
    internal static bool TryFirstNotNull<TItem>(this IEnumerable<TItem?> that, [NotNullWhen(true)] out TItem? item) where TItem : class => that.Where(x => x is not null).TryFirst(out item);

    [Pure]
    internal static bool TryFirstNotNull<TItem, TResult>(this IEnumerable<TItem?> that, Func<TItem?, TResult> selector, [NotNullWhen(true)] out TResult? item) where TResult : class => that.Select(selector).TryFirstNotNull(out item);
}
