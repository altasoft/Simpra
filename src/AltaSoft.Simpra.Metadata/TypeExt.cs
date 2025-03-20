using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AltaSoft.DomainPrimitives;
using AltaSoft.Simpra.Metadata.Models;

namespace AltaSoft.Simpra.Metadata;

internal static class TypeExt
{
    private static readonly ConcurrentDictionary<Type, List<FunctionModel>> s_functionCache = new();
    private static readonly ConcurrentDictionary<Type, List<PropertyModel>> s_propertyCache = new();

    internal static List<FunctionModel> ProcessFunctions(this Type type)
    {
        if (s_functionCache.TryGetValue(type, out var cachedFunctions))
            return cachedFunctions;

        var functions = type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Where(x =>
                {
                    if (x.IsSpecialName)
                    {
                        return false;
                    }
                    if (x.DeclaringType?.Namespace is null)
                    {
                        return true;
                    }
                    var name = x.DeclaringType.Namespace;
                    return !name.StartsWith("System") && !name.StartsWith("Microsoft");
                }
            )
            .Select(method => new FunctionModel
            {
                Name = method.Name,
                Description = method.GetSummaryWithoutException(),
                ReturnType = method.ReturnType.Name,
                Parameters = method.GetParameters().Select(p => p.Name ?? string.Empty).ToList()
            }).ToList();

        s_functionCache[type] = functions;

        return functions;
    }

    internal static List<PropertyModel> ProcessProperties(this Type type)
    {
        if (s_propertyCache.TryGetValue(type, out var cachedProperties))
        {
            return cachedProperties;
        }

        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var processedProperties = properties
            .Where(x => x.GetMethod is not null && x.CanRead)
            .SelectMany(prop => Process(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) is null, prop.PropertyType)).Distinct().ToList();

        s_propertyCache[type] = processedProperties;
        return processedProperties;
    }

    private static List<PropertyModel> Process(string name, bool isRequired, Type type)
    {
        if (type.TryGetUnderlyingDomainPrimitiveType(out var primitive))
        {
            return
            [
                new PropertyModel
                {
                    Name = name,
                    Description = type.GetSummaryWithoutException(),
                    Type = primitive.AliasOrName(),
                    Required = isRequired
                }
            ];
        }

        if (s_primitiveTypes.Contains(type))
        {
            return
            [
                new PropertyModel
                {
                    Name = name,
                    Description = type.GetSummaryWithoutException(),
                    Type = (Nullable.GetUnderlyingType(type) ?? type).AliasOrName(),
                    Required = isRequired
                }
            ];
        }
        if (type.IsEnum || Nullable.GetUnderlyingType(type)?.IsEnum == true)
        {
            var enumType = Nullable.GetUnderlyingType(type) ?? type;
            return
            [
                new PropertyModel
                {
                    Name = name,
                    Description = string.Join(" ", enumType.GetEnumNames()),
                    Type = "enum",
                    Required = isRequired
                }
            ];
        }
        if (type.IsArray)
        {
            var collectionProp = ProcessProperties(type.GetElementType() ?? throw new InvalidOperationException("Should not be null"));
            var aliasOrName = type.AliasOrName();
            collectionProp.ForEach(x => x.Name = $"{aliasOrName}.{x.Name}");
            return
            [
                new PropertyModel { Name = name, Description = type.GetSummaryWithoutException(), Type = aliasOrName, Required = isRequired },
                ..collectionProp
            ];
        }
        if (type is { IsGenericType: true, GenericTypeArguments: [_] } && typeof(IEnumerable).IsAssignableFrom(type))
        {
            var collectionProp = ProcessProperties(type.GetGenericArguments()[0]);
            var aliasOrName = type.AliasOrName();
            collectionProp.ForEach(x => x.Name = $"{aliasOrName}.{x.Name}");
            return
            [
                new PropertyModel { Name = name, Description = type.GetSummaryWithoutException(), Type = aliasOrName, Required = isRequired },
                ..collectionProp
            ];
        }
        var obj = ProcessProperties(Nullable.GetUnderlyingType(type) ?? type);
        obj.ForEach(x => x.Name = $"{name}.{x.Name}");

        return obj;
    }

    private static readonly IReadOnlyList<Type> s_primitiveTypes = new List<Type>()
    {
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(float),
        typeof(double),
        typeof(decimal),
        typeof(DateTime),
        typeof(DateOnly),
        typeof(TimeOnly),
        typeof(TimeSpan),
        typeof(Guid),
        typeof(bool),
        typeof(char),
        typeof(string),
        typeof(byte?),
        typeof(sbyte?),
        typeof(short?),
        typeof(ushort?),
        typeof(int?),
        typeof(uint?),
        typeof(long?),
        typeof(ulong?),
        typeof(float?),
        typeof(double?),
        typeof(decimal?),
        typeof(DateTime?),
        typeof(DateOnly?),
        typeof(TimeOnly?),
        typeof(bool?),
        typeof(char?)
    };

    private static readonly Dictionary<Type, string> s_typeAliases = new()
    {
        { typeof(byte), "byte" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(decimal), "decimal" },
        { typeof(DateTime), "DateTime" },
        { typeof(DateTimeOffset), "DateTimeOffset" },
        { typeof(DateOnly), "DateOnly" },
        { typeof(TimeOnly), "TimeOnly" },
        { typeof(object), "object" },
        { typeof(bool), "bool" },
        { typeof(char), "char" },
        { typeof(string), "string" },
        { typeof(void), "void" },
        { typeof(byte?), "byte?" },
        { typeof(sbyte?), "sbyte?" },
        { typeof(short?), "short?" },
        { typeof(ushort?), "ushort?" },
        { typeof(int?), "int?" },
        { typeof(uint?), "uint?" },
        { typeof(long?), "long?" },
        { typeof(ulong?), "ulong?" },
        { typeof(float?), "float?" },
        { typeof(double?), "double?" },
        { typeof(decimal?), "decimal?" },
        { typeof(DateTime?), "DateTime?" },
        { typeof(DateTimeOffset?), "DateTimeOffset?" },
        { typeof(DateOnly?), "DateOnly?" },
        { typeof(TimeOnly?), "TimeOnly?" },
        { typeof(bool?), "bool?" },
        { typeof(char?), "char?" }
    };

    private static string AliasOrName(this Type type)
    {
        if (s_typeAliases.TryGetValue(type, out var result))
        {
            return result;
        }

        var friendlyName = type.Name;

        if (type.IsGenericType)
        {
            var iBacktick = friendlyName.IndexOf('`');
            if (iBacktick > 0)
            {
                friendlyName = friendlyName.Remove(iBacktick);
            }

            if (Nullable.GetUnderlyingType(type) is { } nt)
            {
                friendlyName = nt.AliasOrName() + "?";
            }
            else
            {
                var typeParameters = type.GetGenericArguments();
                friendlyName += "<";
                for (var i = 0; i < typeParameters.Length; ++i)
                {
                    var typeParamName = typeParameters[i].AliasOrName();
                    friendlyName += i == 0 ? typeParamName : ", " + typeParamName;
                }
                friendlyName += ">";
            }
        }

        return friendlyName;
    }

}
