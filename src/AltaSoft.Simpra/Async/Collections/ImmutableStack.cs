using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AltaSoft.Simpra.Async.Collections;

internal sealed class ImmutableStack<T> : IEnumerable<T>
{
    internal static readonly ImmutableStack<T> s_empty = new(null, default!);

    private readonly ImmutableStack<T>? _parent;
    private readonly T _value;

    private ImmutableStack(ImmutableStack<T>? parent, T value)
    {
        _parent = parent;
        _value = value;
    }

    [MemberNotNullWhen(false, nameof(_parent))]
    public bool IsEmpty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _parent is null;
    }

    [MemberNotNull(nameof(_parent))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AssertNotEmpty()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Stack is empty");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ImmutableStack<T> Pop()
    {
        AssertNotEmpty();
        return _parent;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Peek()
    {
        AssertNotEmpty();
        return _value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // ReSharper disable once ReturnTypeCanBeNotNullable
    public T? PeekOrDefault() => _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ImmutableStack<T> Push(T value) => new(this, value);

    public IEnumerator<T> GetEnumerator()
    {
        for (var item = this; item._parent is not null; item = item._parent)
        {
            yield return item._value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
