using System;
using UnityEngine;

public static class GameEvents
{
    public static Action OnPause;
    public static Action OnResume;

    public static Action<string> OnOpenScene;

    public static void RaiseOnPause() { OnPause?.Invoke(); }
    public static void RaiseOnResume() { OnResume?.Invoke(); }
    public static void RaiseOnOpenScene(string sceneName) { OnOpenScene?.Invoke(sceneName); }
}
