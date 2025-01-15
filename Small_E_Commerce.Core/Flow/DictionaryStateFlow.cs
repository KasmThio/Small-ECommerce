namespace Small_E_Commerce;

public abstract class DictionaryStateFlow<T> : IStateFlow<T> where T : Enum
{
    protected Dictionary<T, T[]> ForwardMap = new();
    public T[] AllowableNext(T state)
    {
        var isKnown = ForwardMap.TryGetValue(state, out var allowableStates);
        if (!isKnown)
            throw new UnknownStateException($"{state} is not defined in the stateflow.");
        return allowableStates!;
    }

    public bool IsAllowed(T currentState, T nextState)
    {
        var isKnown = ForwardMap.TryGetValue(currentState, out var allowableStates);
        if (!isKnown)
            throw new UnknownStateException($"{currentState} is not defined in the stateflow.");

        for (int i = 0; i < allowableStates!.Length; i++)
        {
            if (allowableStates[i].Equals(nextState))
                return true;
        }
        return false;
    }

    protected abstract Dictionary<T, T[]> CreateMap();

    public void AllowedOrThrow(T currentState, T nextState)
    {
        if (!IsAllowed(currentState, nextState))
            throw new IllegalStateTransitionException($"Cannot transit from {currentState} to {nextState}");
    }

    protected DictionaryStateFlow()
    {
        CreateMap();
    }
}