namespace AltaSoft.Simpra.Async;

internal readonly struct TryInfo
{
    public TryInfo(CatchInfo[]? handlers, MachineState? finallyState, MachineState rethrowState, MachineState exitState)
    {
        //Debug.Assert(handlers == null || handlers.All(c => c.BodyState.StateId > 0));
        Handlers = handlers ?? [];
        //Debug.Assert(finallyState == null || finallyState.StateId > 0);
        FinallyState = finallyState;
        //Debug.Assert(rethrowState != null);
        RethrowState = rethrowState;
        //Debug.Assert(exitState != null);
        ExitState = exitState;
    }

    public CatchInfo[] Handlers { get; }

    public MachineState? FinallyState { get; }

    public MachineState RethrowState { get; }

    public MachineState ExitState { get; }
}
