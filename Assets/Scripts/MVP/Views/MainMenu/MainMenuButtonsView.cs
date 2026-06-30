using System;
using UnityEngine;

public class MainMenuButtonsView : MonoBehaviour
{
    [SerializeField] CustomButton _playButton;
    [SerializeField] CustomButton _settingsButton;

    public Action OnSettings;

    private void OnEnable()
    {
        if (_playButton == null)
        {
            Debug.LogWarning("[MainMenuButtonsVIew] Please assign a play button!");
        }
        else
        {
            _playButton.OnClick += PlayButtonClicked;
        }

        if (_settingsButton == null)
        {
            Debug.LogWarning("[MainMenuButtonsView] Please assign a settings button!");
        }
        else
        {
            _settingsButton.OnClick += SettingsButtonClicked;
        }
    }

    private void OnDisable()
    {
        if (_playButton != null)
        {
            _playButton.OnClick -= PlayButtonClicked;
        }

        if (_settingsButton != null)
        {
            _settingsButton.OnClick -= SettingsButtonClicked;
        }
    }

    private void PlayButtonClicked()
    {
        GameEvents.RaiseOnOpenScene(SceneNames.MainGameplay);
    }

    private void SettingsButtonClicked()
    {
        OnSettings?.Invoke();
    }
}
