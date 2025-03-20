using System;
using System.Threading.Tasks;

namespace AltaSoft.Simpra.Async.Tasks;

internal static class CompletionSourceEmitterFactory
{
    internal static ICompletionSourceEmitter Get(Type returnType)
    {
        var instance = returnType == typeof(Task)
            ? new TaskCompletionSourceEmitter()
            : returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)
                ? (ICompletionSourceEmitter?)Activator.CreateInstance(
                    typeof(TaskCompletionSourceEmitter<>).MakeGenericType(returnType.GetGenericArguments()))
                : returnType == typeof(ValueTask)
                    ? new ValueTaskCompletionSourceEmitter()
                    : returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        ? (ICompletionSourceEmitter?)Activator.CreateInstance(
                            typeof(ValueTaskCompletionSourceEmitter<>).MakeGenericType(
                                returnType.GetGenericArguments()))
                        : throw new InvalidOperationException($"There is no CompletionSource for {returnType.Name}");

        return instance ?? throw new InvalidOperationException($"Cannot create CompletionSource for {returnType.Name}");
    }
}
