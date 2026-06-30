using System;
using UnityEngine;

public class PauseMenuView : MonoBehaviour
{
    [SerializeField] PauseMenuButtonsView _pauseMenuButtonsView;
    [SerializeField] SettingsPanelView _settingsPanelView;

    public Action OnResume;

    private void OnEnable()
    {
        if (_pauseMenuButtonsView == null)
        {
            Debug.LogWarning("[PauseMenuView] Please assign a pause menu buttons view");
        }
        else
        {
            _pauseMenuButtonsView.OnConitnue += ContinueButtonClicked;
            _pauseMenuButtonsView.OnSettings += SettingsButtonClicked;
        }

        if (_settingsPanelView == null)
        {
            Debug.LogWarning("[PauseMenuView] Please assign a settings panel view!");
        }
        else
        {
            _settingsPanelView.OnClose += SettingsPanelClose;
        }

            GameEvents.RaiseOnPause();
    }

    private void OnDisable()
    {
        if (_pauseMenuButtonsView != null)
        {
            _pauseMenuButtonsView.OnConitnue -= ContinueButtonClicked;
            _pauseMenuButtonsView.OnSettings -= SettingsButtonClicked;
        }

        if (_settingsPanelView != null)
        {
            _settingsPanelView.OnClose -= SettingsPanelClose;
        }

        GameEvents.RaiseOnResume();
    }

    private void ContinueButtonClicked()
    {
        OnResume?.Invoke();
    }

    private void SettingsButtonClicked()
    {
        if (_pauseMenuButtonsView != null)
        {
            _pauseMenuButtonsView.gameObject.SetActive(false);
        }

        if (_settingsPanelView != null)
        {
            _settingsPanelView.gameObject.SetActive(true);
        }
    }

    private void SettingsPanelClose()
    {
        if (_pauseMenuButtonsView != null)
        {
            _pauseMenuButtonsView.gameObject.SetActive(true);
        }

        if (_settingsPanelView != null)
        {
            _settingsPanelView.gameObject.SetActive(false);
        }
    }
}
