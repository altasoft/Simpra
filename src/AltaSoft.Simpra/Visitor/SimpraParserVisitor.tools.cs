using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using AltaSoft.DomainPrimitives;
using AltaSoft.Simpra.Async.Expressions;
using AltaSoft.Simpra.Types;
using Antlr4.Runtime;

namespace AltaSoft.Simpra.Visitor;

internal partial class SimpraParserVisitor<TResult, TModel>
{
    private static bool IsSimpraBuiltinMethod(string methodName, List<Expression> arguments)
    {
        if (!methodName.IsSimpraBuiltInFunction())
            return false;

        var argTypes = arguments.ConvertAll(x => x.Type);
        var method = typeof(InternalLanguageFunctions).GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, [.. argTypes]);

        return method is not null;
    }

    // DomainTypes are expanded already
    // Nullable<T> are not expanded
    private static Type GetCorrespondingSimpraType(Type type, bool convertLists, ParserRuleContext context)
    {
        var expandedType = type;
        if (expandedType.IsNullableT(out var domainType))
            expandedType = domainType;

        if (expandedType.IsNumber())
        {
            expandedType = typeof(SimpraNumber);
        }
        else
        if (expandedType.IsBool())
        {
            expandedType = typeof(SimpraBool);
        }
        else
        if (expandedType.IsString() || expandedType.IsEnum)
        {
            expandedType = typeof(SimpraString);
        }
        else
        if (expandedType.IsDate())
        {
            expandedType = typeof(SimpraDate);
        }
        else
        if (convertLists && expandedType.IsIEnumerableT() && !expandedType.IsIReadOnlyDictionaryT())
        {
            var arrayElementType = expandedType.GetEnumerationElementType(context);

            var tSimpraType = arrayElementType.IsSimpraType()
                ? arrayElementType
                : GetCorrespondingSimpraType(arrayElementType, true, context);

            expandedType = typeof(SimpraList<,>).MakeGenericType(tSimpraType, tSimpraType.GetSimpraTypeNetType());
        }
        else
        {
            expandedType = typeof(SimpraInteropObject<>).MakeGenericType(type);
        }
        return expandedType;
    }

    // Converts external type to Simpra type
    private static Expression ConvertToSimpraType(Expression expr, bool convertLists, ParserRuleContext context)
    {
        var type = expr.Type;
        if (type.IsSimpraType())
            return expr;

        var domainType = type.GetUnderlyingDomainPrimitiveType();
        if (domainType is not null)
        {
            expr = Expression.Convert(expr, domainType);
            type = domainType;
        }

        var simpraType = GetCorrespondingSimpraType(type, convertLists, context);

        if (type.IsEnum())
        {
            var toStringCall = Expression.Call(expr, typeof(object).GetMethod(nameof(ToString), Type.EmptyTypes)!);

            if (!type.IsNullableT())
            {
                expr = toStringCall;
            }
            else
            {
                // Handle null case: value is null ? null : value.ToString()
                var nullCheck = Expression.Equal(expr, Expression.Constant(null, type));
                // Use a conditional expression to return null or value.ToString()
                expr = Expression.Condition(nullCheck, Expression.Constant(null, typeof(string)), toStringCall);
            }
            return Expression.Convert(expr, simpraType);
        }

        if (convertLists && simpraType.IsSimpraList())
        {
            return NewSimpraList(expr, context);
        }

        if (simpraType.IsSimpraInteropObject())
        {
            var constructorInfo = simpraType.GetConstructor([simpraType.GetGenericArguments()[0]]);
            if (constructorInfo is null)
                throw new InvalidOperationException($"The required constructor for {simpraType.Name} was not found");
            return Expression.New(constructorInfo, expr);
        }

        return Expression.Convert(expr, simpraType);
    }

    private static MethodCallExpression CallBuiltinStaticMethod(Type type, string methodName, Expression[] arguments, ParserRuleContext context)
    {
        var argTypes = Array.ConvertAll(arguments, x => x.Type);
        var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, argTypes);

        return method is null
            ? throw new SimpraException(context, $"Method '{methodName}({string.Join(',', argTypes.Select(x => x.Name))})' not found")
            : Expression.Call(method, arguments);
    }

    private static MethodCallExpression CallBuiltinInstanceMethod(Expression instance, string methodName, Expression[] arguments, ParserRuleContext context)
    {
        var argTypes = Array.ConvertAll(arguments, x => x.Type);
        var method = instance.Type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public, argTypes);

        return method is null
            ? throw new SimpraException(context, $"Method '{instance.Type.Name}.{methodName}({string.Join(',', argTypes.Select(x => x.Name))})' not found")
            : Expression.Call(instance, method, arguments);
    }

    private static MethodCallExpression CallBuiltinPropertyAccess(Expression instance, string propertyName, ParserRuleContext context)
    {
        var method = instance.Type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)?.GetMethod;

        return method is null
            ? throw new SimpraException(context, $"Property '{instance.Type.Name}.{propertyName} {{get}} not found")
            : Expression.Call(instance, method);
    }

    private static MethodCallExpression CallBuiltinStaticGenericMethod(Type type, string methodName, Type[] genericTypes, Expression[] arguments, ParserRuleContext context)
    {
        var argTypes = Array.ConvertAll(arguments, x => x.Type);

        var method = GetBuiltinStaticGenericMethod(type, methodName, genericTypes, argTypes);
        return method is null
            ? throw new SimpraException(context, $"Method '{type.Name}.{methodName}' with arguments ({string.Join(',', argTypes.Select(x => x.Name))}) not found")
            : Expression.Call(method, arguments);
    }

    private static MethodInfo? GetBuiltinStaticGenericMethod(Type type, string methodName, Type[] genericTypes, Type[] parameterTypes)
    {
        var genericArity = genericTypes.Length;

        return type.GetMethods()
            .FirstOrDefault(x =>
                    x.Name == methodName && x is { IsGenericMethod: true, IsStatic: true } &&
                    x.GetGenericArguments().Length == genericArity &&
                    x.GetParameters().Length == parameterTypes.Length
            )
            ?.MakeGenericMethod(genericTypes);
    }

    /// <summary>
    /// Is used to call functions from class provided as TFunctions
    /// </summary>
    private Expression CallFunction(string methodName, Expression[] arguments, ParserRuleContext context)
    {
        //Type targetType, Expression instance, 
        //_externalFunctionsParameter.Type, _externalFunctionsParameter,

        var paramTypes = arguments.Select(a =>
        {
            var type = a.Type;
            if (type == typeof(SimpraBool))
                return typeof(bool);
            if (type == typeof(SimpraNumber))
                return typeof(decimal);
            if (type == typeof(SimpraString))
                return typeof(string);
            if (type.IsSimpraInteropObject())
                return type.GetGenericArguments()[0];

            return a.Type;
        }).ToArray();
        Expression instance = _rootParameter;

        var method = FindBestMatch(typeof(TModel), methodName, paramTypes, out var matchWithToken);
        if (method is null && _externalFunctionsParameter is not null)
        {
            method = FindBestMatch(_externalFunctionsParameter.Type, methodName, paramTypes, out matchWithToken);
            instance = _externalFunctionsParameter;
        }

        if (method is null)
            throw new SimpraException(context, $"Function '{methodName}({string.Join(',', paramTypes.Select(x => x.Name))})' not found");

        var convertedArguments = new Expression[paramTypes.Length + (matchWithToken ? 1 : 0)];
        for (var i = 0; i < paramTypes.Length; i++)
        {
            var expr = arguments[i];

            convertedArguments[i] = ConvertToType(expr, paramTypes[i], context);
        }

        if (matchWithToken)
        {
            convertedArguments[^1] = _cancellationTokenParam;
        }

        var methodCall = method.IsStatic ? Expression.Call(method, convertedArguments) : Expression.Call(instance, method, convertedArguments);

        Expression result = method.IsAsyncMethod() ? methodCall.AwaitConfigured(false) : methodCall;
        return ConvertToSimpraType(result, true, context);
    }

    private static MethodInfo? FindBestMatch(Type targetType, string methodName, Type[] argTypes, out bool matchWithToken)
    {
        matchWithToken = false;
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        var asyncMethodName = methodName + "Async";

        // Get all methods matching the method name
        var methods = targetType.GetMethods(bindingFlags)
            .Where(m => m.Name == methodName || m.Name == asyncMethodName)
            .ToList();

        // Track the best match and its score
        MethodInfo? bestMatch = null;
        var bestScore = int.MaxValue;

        foreach (var method in methods)
        {
            var isAsyncMethod = method.IsAsyncMethod();
            var parameters = method.GetParameters();

            matchWithToken = isAsyncMethod && parameters.Length == argTypes.Length + 1 && parameters[^1].ParameterType == typeof(CancellationToken);

            if (parameters.Length == argTypes.Length || matchWithToken)
            {
                // Calculate the match score for the current method
                var score = CalculateMatchScore(parameters, argTypes, isAsyncMethod);

                // Update the best match if the current method has a better score
                if (score < bestScore)
                {
                    bestMatch = method;
                    bestScore = score;
                }
            }
        }

        return bestMatch;

        static int CalculateMatchScore(ParameterInfo[] parameters, Type[] argTypes, bool isAsyncMethod)
        {
            var paramsToCheck = parameters.Length;
            if (isAsyncMethod && parameters[^1].ParameterType == typeof(CancellationToken))
                paramsToCheck--;

            var score = 0;

            for (var i = 0; i < paramsToCheck; i++)
            {
                var paramType = parameters[i].ParameterType;
                var argType = argTypes[i];

                if (paramType == argType)
                {
                    // Exact match, no penalty
                    score += 0;
                }
                else
                if (IsImplicitlyConvertible(argType, paramType))
                {
                    // Implicit conversion, small penalty
                    score++;
                }
                else
                if (IsNumericType(argType) && IsNumericType(paramType))
                {
                    // Numeric types that require explicit conversion, larger penalty
                    score += 2;
                }
                else
                {
                    // Significant mismatch, very high penalty
                    score += 10;
                }
            }

            return score;
        }

        static bool IsNumericType(Type type)
        {
            return type == typeof(byte) || type == typeof(sbyte) ||
                   type == typeof(short) || type == typeof(ushort) ||
                   type == typeof(int) || type == typeof(uint) ||
                   type == typeof(long) || type == typeof(ulong) ||
                   type == typeof(float) || type == typeof(double) ||
                   type == typeof(decimal);
        }

        static bool IsImplicitlyConvertible(Type fromType, Type toType)
        {
            if (fromType == toType) return true;

            // Handle numeric promotions (e.g., int to long, short to int)
            if (fromType == typeof(byte) && (toType == typeof(short) || toType == typeof(int) || toType == typeof(long))) return true;
            if (fromType == typeof(sbyte) && (toType == typeof(short) || toType == typeof(int) || toType == typeof(long))) return true;
            if (fromType == typeof(short) && (toType == typeof(int) || toType == typeof(long))) return true;
            if (fromType == typeof(ushort) && (toType == typeof(int) || toType == typeof(long))) return true;
            if (fromType == typeof(int) && (toType == typeof(long))) return true;

            return false;
        }
    }

    private MemberExpression? GetModelProperty(string name) => GetPropertyOrConstant(typeof(TModel), _rootParameter, name);

    private static MemberExpression? GetExternalProperty(Type targetType, Expression instance, string name) => GetPropertyOrConstant(targetType, instance, name);

    private static MemberExpression? GetPropertyOrConstant(Type targetType, Expression instance, string name)
    {
        var property = targetType.GetProperty(name);

        if (property is null)
        {
            if (!targetType.IsInterface)
            {
                // Try to find a public constant field (literal, not readonly)
                var constField = targetType
                    .GetField(name, BindingFlags.Public | BindingFlags.Static);

                if (constField is { IsLiteral: true, IsInitOnly: false })
                {
                    return Expression.Field(null, constField);
                }

                return null;
            }

            // Handle interfaces: search across interface and its base interfaces
            property = targetType
                .GetInterfaces()
                .Append(targetType) // include the interface itself
                .SelectMany(i => i.GetProperties())
                .FirstOrDefault(p => p.Name == name);

            if (property is null)
                return null;
        }

        var getter = property.GetGetMethod();
        if (getter is null)
            return null;

        if (getter.IsStatic) // Static property
            return Expression.Property(null, property);

        return Expression.Property(instance, property);
    }

    private (ParameterExpression variable, Type variableType)? GetVariable(string name)
    {
        if (_variables.TryGetValue(name, out var variableInfo))
        {
            return variableInfo;
        }
        return null;
    }

    private (ParameterExpression variable, Type variableType) CreateVariable(string name, Expression exprValue)
    {
        var type = exprValue.Type;

        var variable = Expression.Variable(type, name);
        _variables.Add(name, (variable, type));

        return (variable, type);
    }

    private static Expression ConvertToBool(Expression expr)
    {
        if (expr.Type == typeof(bool))
            return expr;

        if (expr.Type.TryGetUnderlyingDomainPrimitiveType(out var domainType))
        {
            expr = Expression.Convert(expr, domainType);
        }

        return Expression.Convert(expr, typeof(bool));
    }

    private static Expression ConvertToType(Expression expr, Type targetType, ParserRuleContext context)
    {
        if (expr.Type == targetType)
            return expr;

        if (expr.Type.TryGetUnderlyingDomainPrimitiveType(out var domainType))
        {
            expr = Expression.Convert(expr, domainType);
        }

        if (targetType.TryGetUnderlyingDomainPrimitiveType(out domainType))
        {
            expr = Expression.Convert(expr, domainType);
        }

        if (expr.Type == typeof(SimpraString) && targetType.IsEnum())
        {
            if (targetType.IsNullableT())
                expr = CallBuiltinStaticGenericMethod(typeof(SimpraString), nameof(SimpraString.ToNullableEnum), [targetType], [expr], context);
            else
                expr = CallBuiltinStaticGenericMethod(typeof(SimpraString), nameof(SimpraString.ToEnum), [targetType], [expr], context);
        }

        if (!expr.Type.IsGenericType)
            return Expression.Convert(expr, targetType);

        var genericTypeDefinition = expr.Type.GetGenericTypeDefinition();
        var isSimpraObject = genericTypeDefinition == typeof(SimpraInteropObject<>);
        var isSimpraList = genericTypeDefinition == typeof(SimpraList<,>);

        if (!isSimpraList && !isSimpraObject)
            return Expression.Convert(expr, targetType);

        expr = CallBuiltinPropertyAccess(expr, nameof(ISimpraType<object>.Value), context);

        if (isSimpraObject)
        {
            return ConvertToType(expr, targetType, context);
        }

        //simpraList

        var listTType = expr.Type.GetGenericArguments()[0];
        var underlyingTargetType = targetType.GetEnumerationElementType(context);

        expr = CallConvertAllMethodOnList(expr, listTType, underlyingTargetType, context);

        if (targetType.IsArray)
        {
            expr = CallBuiltinInstanceMethod(expr, nameof(List<object>.ToArray), [], context);
        }

        return ConvertToType(expr, targetType, context);

    }
    private static Expression CallConvertAllMethodOnList(Expression simpraListExpr, Type listTType, Type convertTo, ParserRuleContext context)
    {
        var param = Expression.Parameter(listTType, "x");

        var delegateType = typeof(Converter<,>).MakeGenericType(listTType, convertTo);

        var lambda = Expression.Lambda(delegateType, ConvertToType(param, convertTo, context), param);

        // Get ConvertAll method
        var convertAllMethod = simpraListExpr.Type
            .GetMethod(nameof(List<object>.ConvertAll), BindingFlags.Public | BindingFlags.Instance)!
            .MakeGenericMethod(convertTo);

        simpraListExpr = Expression.Call(simpraListExpr, convertAllMethod, lambda);
        return simpraListExpr;
    }

    private static NewExpression NewSimpraList(IEnumerable<Expression> arrayElements, ParserRuleContext context)
    {
        var array = arrayElements.ToArray();
        if (array.Length == 0)
            throw new InvalidOperationException("Array elements cannot be empty");

        var simpraElementType = array[0].Type;
        var elementType = simpraElementType.GetSimpraTypeNetType();
        var simpraListType = typeof(SimpraList<,>).MakeGenericType(simpraElementType, elementType);
        var ctorParamType = typeof(IEnumerable<>).MakeGenericType(simpraElementType);

        var constructorInfo = simpraListType.GetConstructor([ctorParamType]);
        if (constructorInfo is null)
            throw new InvalidOperationException($"The required constructor for {simpraListType.Name} was not found");

        var convertedArrayElements = array.Select(x => ConvertToSimpraType(x, true, context));

        var arrayExpression = Expression.NewArrayInit(simpraElementType, convertedArrayElements);

        return Expression.New(constructorInfo, arrayExpression);
    }

    private static Expression NewSimpraList(Expression arrayElements, ParserRuleContext context)
    {
        if (!arrayElements.Type.IsIEnumerableT())
            throw new InvalidOperationException("Enumeration elements must implement IEnumerable<T>");

        var arrayElementType = arrayElements.Type.GetEnumerationElementType(context);
        var domainType = arrayElementType.GetUnderlyingDomainPrimitiveType();

        var tSimpraType = GetCorrespondingSimpraType(domainType ?? arrayElementType, true, context);

        var simpraListType = typeof(SimpraList<,>).MakeGenericType(tSimpraType, tSimpraType.GetSimpraTypeNetType());
        var ctorParamType = typeof(IEnumerable<>).MakeGenericType(tSimpraType);

        var constructorInfo = simpraListType.GetConstructor([ctorParamType]);
        if (constructorInfo is null)
            throw new InvalidOperationException($"The required constructor for {simpraListType.Name} was not found");

        // Create a variable for arrayElements to avoid duplication
        var arrayVar = Expression.Variable(arrayElements.Type, "arrayElementsVar");
        var assignArray = Expression.Assign(arrayVar, arrayElements);

        // Create a parameter to iterate over the enumerable
        var parameter = Expression.Parameter(arrayElementType, "x");

        // Create the conversion lambda
        var conversionLambda = Expression.Lambda(
            Expression.Convert(ConvertToSimpraType(parameter, true, context), tSimpraType),
            parameter
        );

        // Generate Enumerable.Select(arrayVar, x => ConvertToSimpraType(x))
        var projectedElements = Expression.Call(
            typeof(Enumerable), nameof(Enumerable.Select),
            [arrayElementType, tSimpraType],
            arrayVar,
            conversionLambda
        );

        // Generate fallback: Enumerable.Empty<tSimpraType>()
        var emptyEnumerable = Expression.Call(
            typeof(Enumerable), nameof(Enumerable.Empty), [tSimpraType]
        );

        // If arrayVar != null ? Select(...) : Empty<tSimpraType>()
        var nullCheck = Expression.NotEqual(arrayVar, Expression.Constant(null, arrayElements.Type));
        var safeProjectedElements = Expression.Condition(nullCheck, projectedElements, emptyEnumerable);

        // Final constructor call
        var newSimpraList = Expression.New(constructorInfo, safeProjectedElements);

        // Return wrapped block: declare, assign, use
        return Expression.Block([arrayVar], assignArray, newSimpraList);
    }

    private static BinaryExpression SetCaseSensitivity(bool isCaseSensitive)
    {
        var propertyInfo = typeof(StringComparisonContext).GetProperty(nameof(StringComparisonContext.IsCaseSensitive), BindingFlags.Static | BindingFlags.Public)!;

        return Expression.Assign(Expression.Property(null, propertyInfo), Expression.Constant(isCaseSensitive, typeof(bool)));
    }

    private static IdentifierAccessType GetIdentifierAccessType(RuleContext context)
    {
        var parent = context.Parent;

        // Case 1: Writing (assignment)
        if (parent is SimpraParser.AssignmentContext assignmentContext && assignmentContext.Left == context)
        {
            return IdentifierAccessType.Write;
        }

        // Case 2: Reading (expression)
        if (parent is SimpraParser.ExpContext or SimpraParser.StatementContext)
        {
            // Property is being read
            return IdentifierAccessType.Read;
        }

        // Case 3: Accessing Members
        if (parent is SimpraParser.MemberAccessContext)
        {
            // Property is part of a member access
            return IdentifierAccessType.Read;
        }

        if (parent is SimpraParser.IndexAccessContext)
        {
            // Property is being read
            return IdentifierAccessType.Read;
        }

        throw new InvalidOperationException($"Unsupported usage of property '{context.GetText()}'");
    }

    internal enum IdentifierAccessType
    {
        Read,
        Write
    }
}
