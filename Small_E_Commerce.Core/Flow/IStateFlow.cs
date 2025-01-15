namespace Small_E_Commerce;

internal interface IStateFlow<T> where T : Enum
{
    T[] AllowableNext(T state);
    bool IsAllowed(T currentState, T nextState);
    void AllowedOrThrow(T currentState, T nextState);
}