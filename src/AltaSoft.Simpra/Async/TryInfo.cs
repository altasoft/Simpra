namespace AltaSoft.Simpra.Async;

internal readonly struct TryInfo
{
    public TryInfo(CatchInfo[]? handlers, MachineState? finallyState, MachineState rethrowState, MachineState exitState)
    {
        Handlers = handlers ?? [];
        FinallyState = finallyState;
        RethrowState = rethrowState;
        ExitState = exitState;
    }

    public CatchInfo[] Handlers { get; }

    public MachineState? FinallyState { get; }

    public MachineState RethrowState { get; }

    public MachineState ExitState { get; }
}
