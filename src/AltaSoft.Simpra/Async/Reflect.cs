using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AltaSoft.Simpra.Async;

internal static class Reflect<TType>
{
    internal static PropertyInfo GetProperty<TResult>(Expression<Func<TType, TResult>> propertyAccess)
    {
        if (propertyAccess.Body is not MemberExpression { Member: PropertyInfo } expression)
        {
            if (propertyAccess.Body is MethodCallExpression { Method.DeclaringType: not null } callExpression)
            {
                foreach (var property in callExpression.Method.DeclaringType.GetProperties())
                {
                    if (property.GetGetMethod() == callExpression.Method || property.GetSetMethod() == callExpression.Method)
                    {
                        return property;
                    }
                }
            }
            throw new ArgumentException("Lambda expression is not a property access");
        }
        return (PropertyInfo)expression.Member;
    }

    internal static MethodInfo GetMethod(Expression<Action<TType>> methodCall) => methodCall.Body is not MethodCallExpression expression || expression.Method.IsStatic
            ? throw new ArgumentException("Lambda expression is not an instance method call")
            : expression.Method;
}

internal static class Reflect
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> s_methDelegateInvoke = new();

    private static readonly ConcurrentDictionary<(Type type, bool interfaces), Type[]> s_compatibleTypeCache = new();
    private static readonly ConcurrentDictionary<Type, (MethodInfo? meth_GetAwaiter, MethodInfo? meth_ConfigureAwait)> s_awaitableInfos = new();
    private static readonly ConcurrentDictionary<Type, (PropertyInfo? prop_IsCompleted, MethodInfo? meth_OnCompleted, MethodInfo? meth_GetResult)> s_awaiterInfos = new();

    internal static MethodInfo GetDelegateInvokeMethod(this Type that) => s_methDelegateInvoke.GetOrAdd(that, t =>
       {
           if (!typeof(Delegate).IsAssignableFrom(t))
           {
               throw new ArgumentException("Type is not a delegate");
           }
           var method = t.GetMethod("Invoke");
           Debug.Assert(method is not null);
           return method;
       });

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static MethodInfo? GetAwaitableGetAwaiterMethod(this Type that) => GetAwaitableInfos(that).meth_GetAwaiter;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static MethodInfo? GetAwaitableGetConfigureAwaitMethod(this Type that) => GetAwaitableInfos(that).meth_ConfigureAwait;

    private static (MethodInfo? meth_GetAwaiter, MethodInfo? meth_ConfigureAwait) GetAwaitableInfos(Type that)
        => s_awaitableInfos.GetOrAdd(that, t =>
        {
            var methAwaiter = t.GetMethod("GetAwaiter", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);
            var methConfigureAwait = t.GetMethod("ConfigureAwait", BindingFlags.Instance | BindingFlags.Public, null,
                [typeof(bool)], null);
            return
            (
methAwaiter is not null && methAwaiter.ReturnType.IsAwaiter() ? methAwaiter : null,
methConfigureAwait is not null && methConfigureAwait.ReturnType.IsAwaitable() ? methConfigureAwait : null
            );
        });

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsStrongBox(this Type? that) => GetStrongBoxValueType(that) is not null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Type? GetStrongBoxValueType(this Type? that)
    {
        while (that != null)
        {
            if (that.IsGenericType && that.GetGenericTypeDefinition() == typeof(StrongBox<>))
            {
                return that.GetGenericArguments()[0];
            }
            that = that.BaseType;
        }
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsAwaitable(this Type that) => GetAwaitableGetAwaiterMethod(that) is not null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static MethodInfo? GetAwaiterGetResultMethod(this Type that) => AwaiterInfo(that).meth_GetResult;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static MethodInfo? GetAwaiterOnCompletedMethod(this Type that) => AwaiterInfo(that).meth_OnCompleted;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static PropertyInfo? GetAwaiterIsCompletedProperty(this Type that) => AwaiterInfo(that).prop_IsCompleted;

    private static (PropertyInfo? prop_IsCompleted, MethodInfo? meth_OnCompleted, MethodInfo? meth_GetResult) AwaiterInfo(Type that)
        => s_awaiterInfos.GetOrAdd(that, t =>
      {
          if (!typeof(INotifyCompletion).IsAssignableFrom(t))
          {
              return default;
          }
          var isCompleted = t.GetProperty("IsCompleted", BindingFlags.Instance | BindingFlags.Public);
          if (isCompleted?.PropertyType != typeof(bool))
          {
              return default;
          }
          var onCompleted = t.GetMethod("OnCompleted", BindingFlags.Instance | BindingFlags.Public, null,
          [typeof(Action)], null);
          if (onCompleted?.ReturnType != typeof(void))
          {
              return default;
          }
          var getResult = t.GetMethod("GetResult", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);
          return getResult == null ? default : (isCompleted, onCompleted, getResult);
      });

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsAwaiter(this Type that) => GetAwaiterGetResultMethod(that) != null;

    internal static MethodInfo GetStaticMethod(Expression<Action> staticMethodCall) => staticMethodCall.Body is not MethodCallExpression expression || !expression.Method.IsStatic
            ? throw new ArgumentException("Lambda expression is not a static method call")
            : expression.Method;

    internal static PropertyInfo GetStaticProperty<TResult>(Expression<Func<TResult>> propertyAccess) => propertyAccess.Body is not MemberExpression
    {
        Member: PropertyInfo
    } expression
            ? throw new ArgumentException("Lambda expression is not a property access")
            : (PropertyInfo)expression.Member;

    internal static ConstructorInfo GetConstructor(Expression<Action> constructorCall) => constructorCall.Body is not NewExpression expression
            ? throw new ArgumentException("Lambda expression is not a constructor call")
            : expression.Constructor ?? throw new InvalidOperationException("Cannot get constructor from lambda expression");

    internal static IReadOnlyList<Type> GetCompatibleTypes(Type type, bool interfaces) => s_compatibleTypeCache.GetOrAdd((type, interfaces), t => GetCompatibleTypesImpl(t.type, t.interfaces).Distinct().ToArray());

    private static IEnumerable<Type> GetCompatibleTypesImpl(Type type, bool interfaces)
    {
        yield return type;
        if (!type.IsValueType)
        {
            for (var current = type.BaseType; current != null; current = current.BaseType)
            {
                yield return type;
            }
        }
        if (!interfaces)
        {
            yield break;
        }
        foreach (var implementedInterface in type.GetInterfaces())
        {
            // maybe add generic support for contravariance, currently only supported for IEnumerable<*>
            if (implementedInterface.IsGenericType)
            {
                var typeDefinition = implementedInterface.GetGenericTypeDefinition();
                if (typeDefinition == typeof(IEnumerable<>) || typeDefinition == typeof(IAsyncEnumerable<>))
                {
                    var itemType = implementedInterface.GetGenericArguments()[0];
                    if (!type.IsAssignableFrom(itemType))
                    {
                        foreach (var compatibleItemType in GetCompatibleTypes(itemType, true))
                        {
                            yield return typeDefinition.MakeGenericType(compatibleItemType);
                        }
                    }
                }
                else
                {
                    yield return implementedInterface;
                }
            }
            else
            {
                yield return implementedInterface;
            }
        }
    }
}
