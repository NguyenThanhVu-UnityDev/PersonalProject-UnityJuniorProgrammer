using System;
using UnityEngine;

public class GameplayView : MonoBehaviour
{
    [SerializeField] CustomButton _pauseButton;

    public Action OnPause;

    private void OnEnable()
    {
        if (_pauseButton == null)
        {
            Debug.LogWarning("[GameplayView] Please assigned a pause button!");
        }
        else
        {
            _pauseButton.OnClick += PauseButtonClicked;
        }
    }

    private void PauseButtonClicked()
    {
        OnPause?.Invoke();
    }

}
