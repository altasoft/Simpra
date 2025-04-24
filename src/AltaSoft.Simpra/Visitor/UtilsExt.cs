using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AltaSoft.Simpra.Types;
using Antlr4.Runtime;

namespace AltaSoft.Simpra.Visitor;

/// <summary>
/// Provides extension methods for various type checks and operations related to Simpra types.
/// </summary>
internal static class UtilsExt
{
    /// <summary>
    /// Determines whether the specified type is a Simpra type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is a Simpra type; otherwise, <c>false</c>.</returns>
    public static bool IsSimpraType(this Type type) => type == typeof(SimpraBool) || type == typeof(SimpraNumber) || type == typeof(SimpraDate) ||
            type == typeof(SimpraString) || type.IsSimpraList() || /* type == typeof(SimpraDictionary) || */
            type.IsSimpraInteropObject();

    /// <summary>
    /// Determines whether the specified type is a Simpra list.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is a Simpra list; otherwise, <c>false</c>.</returns>
    public static bool IsSimpraList(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SimpraList<,>);

    /// <summary>
    /// Determines whether the specified type is a Simpra interop object.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is a Simpra interop object; otherwise, <c>false</c>.</returns>
    public static bool IsSimpraInteropObject(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SimpraInteropObject<>);

    /// <summary>
    /// Gets the .NET type of the Simpra type.
    /// </summary>
    /// <param name="type">The Simpra type.</param>
    /// <returns>The .NET type of the Simpra type.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the type is not a Simpra type.</exception>
    public static Type GetSimpraTypeNetType(this Type type)
    {
        var simpraTypeInterface = type
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISimpraType<>));

        if (simpraTypeInterface is { IsGenericType: true, GenericTypeArguments.Length: 1 })
            return simpraTypeInterface.GenericTypeArguments[0];

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SimpraInteropObject<>))
            return type.GenericTypeArguments[0];

        throw new InvalidOperationException($"Type '{type}' is not a Simpra type");
    }

    /// <summary>
    /// Determines whether the specified type is a nullable type and returns the underlying type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="underlyingType">The underlying type if the type is nullable.</param>
    /// <returns><c>true</c> if the specified type is a nullable type; otherwise, <c>false</c>.</returns>
    public static bool IsNullableT(this Type type, [NotNullWhen(true)] out Type? underlyingType)
    {
        underlyingType = Nullable.GetUnderlyingType(type);
        return underlyingType is not null;
    }

    /// <summary>
    /// Determines whether the specified type is a nullable type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is a nullable type; otherwise, <c>false</c>.</returns>
    public static bool IsNullableT(this Type type) => Nullable.GetUnderlyingType(type) is not null;

    /// <summary>
    /// Determines whether the specified type is a nullable type of the specified generic type.
    /// </summary>
    /// <typeparam name="T">The generic type to check.</typeparam>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is a nullable type of the specified generic type; otherwise, <c>false</c>.</returns>
    public static bool IsNullable<T>(this Type type) => type.IsNullableT(out var underlyingType) && underlyingType == typeof(T);

    /// <summary>
    /// Determines whether the specified type is an IEnumerable of T.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is an IEnumerable of T; otherwise, <c>false</c>.</returns>
    public static bool IsIEnumerableT(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || type
            .GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

    /// <summary>
    /// Determines whether the specified type is an IReadOnlyDictionary of TKey and TValue.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is an IReadOnlyDictionary of TKey and TValue; otherwise, <c>false</c>.</returns>
    public static bool IsIReadOnlyDictionaryT(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>) || type
            .GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>));

    /// <summary>
    /// Gets the element type of enumeration type.
    /// </summary>
    /// <param name="type">The enumeration type.</param>
    /// <param name="context">The parser rule context.</param>
    /// <returns>The element type of the enumeration type.</returns>
    /// <exception cref="SimpraException">Thrown when the type is not a recognized collection type.</exception>
    public static Type GetEnumerationElementType(this Type type, ParserRuleContext context)
    {
        // Handle arrays (T[])
        if (type.IsArray)
        {
            return type.GetElementType()!;
        }

        // Handle IEnumerable<T>
        if (type.IsIEnumerableT())
        {
            return type.GenericTypeArguments[0];
        }

        // Handle cases where the type implements IEnumerable<T> (e.g., custom collections)
        var enumerable = type.GetInterfaces().FirstOrDefault(x => x.IsIEnumerableT());
        if (enumerable is not null)
        {
            return enumerable.GenericTypeArguments[0];
        }

        // Throw an exception if the type is not a recognized collection type
        throw new SimpraException(context, $"Type '{type}' is not an IEnumerable<T> or an array");
    }

    /// <summary>
    /// Determines whether the specified type is a boolean type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is a boolean type; otherwise, <c>false</c>.</returns>
    public static bool IsBool(this Type type) => type == typeof(bool);

    /// <summary>
    /// Determines whether the specified type is a numeric type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is a numeric type; otherwise, <c>false</c>.</returns>
    public static bool IsNumber(this Type type) => type == typeof(byte) ||
               type == typeof(sbyte) ||
               type == typeof(short) ||
               type == typeof(ushort) ||
               type == typeof(int) ||
               type == typeof(uint) ||
               type == typeof(long) ||
               type == typeof(ulong) ||
               type == typeof(float) ||
               type == typeof(double) ||
               type == typeof(decimal);

    /// <summary>
    /// Determines whether the specified type is a string or char type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is a string or char type; otherwise, <c>false</c>.</returns>
    public static bool IsString(this Type type) => type == typeof(string) || type == typeof(char);

    /// <summary>
    /// Determines whether the specified type is a date or time type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is a date or time type; otherwise, <c>false</c>.</returns>
    public static bool IsDate(this Type type) => type == typeof(DateTime) ||
               type == typeof(DateTimeOffset) ||
               type == typeof(TimeSpan) ||
               type == typeof(DateOnly) ||
               type == typeof(TimeOnly);

    /// <summary>
    /// Determines whether the specified type is an enumeration type. or Nullable enumeration type
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is an enumeration type; otherwise, <c>false</c>.</returns>
    public static bool IsEnum(this Type type)
    {
        return type.IsEnum || (type.IsNullableT(out var t) && t.IsEnum);
    }

    ///// <summary>
    ///// Determines whether the specified type is a domain primitive.
    ///// </summary>
    ///// <param name="self">The type to check.</param>
    ///// <returns><c>true</c> if the specified type is a domain primitive; otherwise, <c>false</c>.</returns>
    public static bool IsAsyncMethod(this MethodInfo method) => typeof(Task).IsAssignableFrom(method.ReturnType) ||
               (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(ValueTask<>));
}
