using System;
using UnityEngine;

public class PauseMenuButtonsView : MonoBehaviour
{
    [SerializeField] CustomButton _continueButton;
    [SerializeField] CustomButton _settingsButton;
    [SerializeField] CustomButton _quitToMenuButton;

    public Action OnConitnue;
    public Action OnSettings;

    private void OnEnable()
    {
        _continueButton.OnClick += ContinueButtonClicked;
        _settingsButton.OnClick += SettingsButtonClicked;
        _quitToMenuButton.OnClick += QuitToMenuButtonClicked;
    }

    private void OnDisable()
    {
        _continueButton.OnClick -= ContinueButtonClicked;
        _settingsButton.OnClick -= SettingsButtonClicked;
        _quitToMenuButton.OnClick -= QuitToMenuButtonClicked;
    }

    private void ContinueButtonClicked()
    {
        OnConitnue?.Invoke();
    }

    private void SettingsButtonClicked()
    {
        OnSettings?.Invoke();
    }

    private void QuitToMenuButtonClicked()
    {
        GameEvents.OnOpenScene(SceneNames.MainMenu);
    }
}
