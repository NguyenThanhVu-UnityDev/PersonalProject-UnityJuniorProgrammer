using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] AudioClip _backgroundMusic;

    [SerializeField] MainMenuButtonsView _mainMenuButtonsView;
    [SerializeField] SettingsPanelView _settingsPanelView;

    private void OnEnable()
    {
        if (_mainMenuButtonsView == null)
        {
            Debug.LogWarning("[MainMenuUI] Please assign a main menu buttons view!");
        }
        else
        {
            _mainMenuButtonsView.OnSettings += Settings;
        }

        if (_settingsPanelView == null)
        {
            Debug.LogWarning("[MainMenuUI] Please assign a settings panel view!");
        }
        else
        {
            _settingsPanelView.OnClose += SettingsClose;
        }
    }

    private void OnDisable()
    {
        if (_mainMenuButtonsView != null)
        {
            _mainMenuButtonsView.OnSettings -= Settings;
        }

        if (_settingsPanelView != null)
        {
            _settingsPanelView.OnClose -= SettingsClose;
        }
    }

    private void Start()
    {
        UIEvents.RaisePlayBackgroundMusic(_backgroundMusic);
    }

    private void Settings()
    {
        if (_mainMenuButtonsView != null) { _mainMenuButtonsView.gameObject.SetActive(false); }
        if (_settingsPanelView != null) { _settingsPanelView.gameObject.SetActive(true); }
    }

    private void SettingsClose()
    {
        if (_mainMenuButtonsView != null) { _mainMenuButtonsView.gameObject.SetActive(true); }
        if (_settingsPanelView != null) { _settingsPanelView.gameObject.SetActive(false); }
    }
}
