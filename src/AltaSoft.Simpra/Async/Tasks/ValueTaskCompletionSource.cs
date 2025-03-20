using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace AltaSoft.Simpra.Async.Tasks;

internal sealed class ValueTaskCompletionSource : IValueTaskSource
{
    // Don't make this readonly, since it is a mutable struct
    private ManualResetValueTaskSourceCore<object> _mr;

    public ValueTaskCompletionSource() => _mr.RunContinuationsAsynchronously = true;

    public void SetResult() => _mr.SetResult(default!);

    public void SetException(Exception error) => _mr.SetException(error);

    public ValueTask GetValueTask() => new(this, _mr.Version);

    void IValueTaskSource.GetResult(short token) => _mr.GetResult(token);

    ValueTaskSourceStatus IValueTaskSource.GetStatus(short token) => _mr.GetStatus(token);

    void IValueTaskSource.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags) => _mr.OnCompleted(continuation, state, token, flags);
}

internal sealed class ValueTaskCompletionSource<TResult> : IValueTaskSource<TResult>
{
    // Don't make this readonly, since it is a mutable struct
    private ManualResetValueTaskSourceCore<TResult> _mr;

    public ValueTaskCompletionSource() => _mr.RunContinuationsAsynchronously = true;

    public void SetResult(TResult result) => _mr.SetResult(result);

    public void SetException(Exception error) => _mr.SetException(error);

    public ValueTask<TResult> GetValueTask() => new(this, _mr.Version);

    TResult IValueTaskSource<TResult>.GetResult(short token) => _mr.GetResult(token);

    ValueTaskSourceStatus IValueTaskSource<TResult>.GetStatus(short token) => _mr.GetStatus(token);

    void IValueTaskSource<TResult>.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags) => _mr.OnCompleted(continuation, state, token, flags);
}
