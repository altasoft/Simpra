using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AltaSoft.Simpra.Async;

internal sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
{
    internal static readonly ReferenceEqualityComparer<T> s_default = new();

    public bool Equals(T? x, T? y) => ReferenceEquals(x, y);

    public int GetHashCode(T obj) => RuntimeHelpers.GetHashCode(obj);
}
