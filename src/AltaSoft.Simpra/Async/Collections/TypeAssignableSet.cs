using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AltaSoft.Simpra.Async.Collections;

internal sealed class TypeAssignableSet : ICollection<Type>
{
    private readonly List<Type> _types = [];

    void ICollection<Type>.Add(Type item) => Add(item);

    public void Clear() => _types.Clear();

    public bool Contains(Type type) => _types.Any(t => t.IsAssignableFrom(type));

    void ICollection<Type>.CopyTo(Type[] array, int arrayIndex) => _types.CopyTo(array, arrayIndex);

    bool ICollection<Type>.Remove(Type item) => throw new NotSupportedException("Removing is not supported");

    public int Count => _types.Count;

    bool ICollection<Type>.IsReadOnly => false;

    public bool Add(Type? type)
    {
        if (type is null)
        {
            return false;
        }
        if (Contains(type))
        {
            return false;
        }
        _types.RemoveAll(type.IsAssignableFrom);
        _types.Add(type);
        return true;
    }

    public IEnumerator<Type> GetEnumerator() => _types.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _types.GetEnumerator();
}
