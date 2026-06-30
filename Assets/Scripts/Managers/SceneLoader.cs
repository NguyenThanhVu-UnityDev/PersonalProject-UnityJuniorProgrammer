using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.OnOpenScene += OpenScene;
    }

    private void OnDisable()
    {
        GameEvents.OnOpenScene -= OpenScene;
    }

    private void OpenScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}

public static class SceneNames
{
    public const string MainMenu = "MainMenu";
    public const string MainGameplay = "Gameplay";
}
