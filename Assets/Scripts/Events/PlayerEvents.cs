using System;
using UnityEngine;

public static class PlayerEvents
{
    public static Action<int> OnCollectedGrapeChanged { get; set; }
    public static Action OnPlayerDead { get; set; }

    public static void RaiseCollectedGrapeChanged(int amount)
    {
        OnCollectedGrapeChanged.Invoke(amount);
    }

    public static void RaisePlayerDead()
    {
        OnPlayerDead.Invoke();
    }
}
