using UnityEngine;

public class IngameUI : MonoBehaviour
{
    [SerializeField] GameplayView _gameplayView;
    [SerializeField] PauseMenuView _pauseMenuView;

    private void OnEnable()
    {
        if (_gameplayView == null)
        {
            Debug.LogWarning("[IngameUI] Please assign a gameplay view!");
        }
        else
        {
            _gameplayView.OnPause += ShowPausePanel;
        }

        if (_pauseMenuView == null)
        {
            Debug.LogWarning("[IngameUI] Please assign a pause menu view!");
        }
        else
        {
            _pauseMenuView.OnResume += Resume;
        }
    }

    private void OnDisable()
    {
        if (_gameplayView != null)
        {
            _gameplayView.OnPause -= ShowPausePanel;
        }

        if (_pauseMenuView != null)
        {
            _pauseMenuView.OnResume -= Resume;
        }
    }

    private void ShowPausePanel()
    {
        if (_gameplayView != null)
        {
            _gameplayView.gameObject.SetActive(false);
        }

        if (_pauseMenuView != null)
        {
            _pauseMenuView.gameObject.SetActive(true);
        }
    }

    private void Resume()
    {
        if (_gameplayView != null)
        {
            _gameplayView.gameObject.SetActive(true);
        }

        if (_pauseMenuView != null)
        {
            _pauseMenuView.gameObject.SetActive(false);
        }
    }
}
