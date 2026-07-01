using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelView : MonoBehaviour
{
    [SerializeField] string _closeTrigger = "Close";

    [Header("UI Elements")]
    [SerializeField] CustomToggle _backgroundMusicToggle;
    [SerializeField] CustomToggle _sfxToggle;
    [SerializeField] CustomButton _closeButton;

    private Animator _animator;

    public Action<bool> OnBackgroundMusicToggleChanged;
    public Action<bool> OnSFXToggleChanged;
    public Action OnClose;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (_closeButton == null)
        {
            Debug.LogWarning("[SettingsPanelView] No close button is assigned!");
        }
        else
        {
            _closeButton.OnClick += Close;
        }

        if (_backgroundMusicToggle == null)
        {
            Debug.LogWarning("[SettingsPanelView] No background music toggle is assigned!");
        }
        else
        {
            _backgroundMusicToggle.OnValueChanged += BackgroundMusicToggleChanged;
        }

        if (_sfxToggle == null)
        {
            Debug.LogWarning("[SettingsPanelView] No SFX toggle is assigned!");
        }
        else
        {
            _sfxToggle.OnValueChanged += SFXToggleChanged;
        }
    }

    private void OnDisable()
    {
        if (_closeButton != null)
        {
            _closeButton.OnClick -= Close;
        }

        if (_backgroundMusicToggle != null)
        {
            _backgroundMusicToggle.OnValueChanged -= BackgroundMusicToggleChanged;
        }

        if (_sfxToggle != null)
        {
            _sfxToggle.OnValueChanged -= SFXToggleChanged;
        }
    }

    public void Close()
    {
        if (_animator == null)
        {
            Debug.LogWarning("[SettingsPanelView] No Animator is assigned!");
            OnClose?.Invoke();
            gameObject.SetActive(false);
        }
        else
        {
            _animator.SetTrigger(_closeTrigger);
        }

    }

    public void AnimationFinished()
    {
        OnClose?.Invoke();
        gameObject.SetActive(false);
    }

    public void SetBackgroundMusicToggle(bool value)
    {
        _backgroundMusicToggle.SetToggle(value);
    }

    private void BackgroundMusicToggleChanged(bool value)
    {
        OnBackgroundMusicToggleChanged?.Invoke(value);
    }

    public void SetSFXToggle(bool value)
    {
        _sfxToggle.SetToggle(value);
    }

    private void SFXToggleChanged(bool value)
    {
        OnSFXToggleChanged?.Invoke(value);
    }

    public void LoadData(SettingsData settingsData)
    {
        if (settingsData == null)
        {
            Debug.LogWarning("[SettingsPanelView] No settings data is assigned!");
            return;
        }

        SetBackgroundMusicToggle(settingsData.IsBackgroundMusicOn);
        SetSFXToggle(settingsData.IsSFXOn);
    }

}
