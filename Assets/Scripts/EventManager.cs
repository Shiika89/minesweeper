using System;

public class EventManager
{
    public static event Action OnStageClear;

    public static void StageClear()
    {
        OnStageClear?.Invoke();
    }
}
