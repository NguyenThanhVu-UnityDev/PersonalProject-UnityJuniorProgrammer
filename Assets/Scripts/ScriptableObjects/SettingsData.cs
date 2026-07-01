using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "Data/SettingsData")]
public class SettingsData : ScriptableObject
{
    [SerializeField] string _isBackgroundMusicOnKey = "IsBackgroundMusicOn";
    [SerializeField] string _isSFXOnKey = "IsSFXOn";

    private bool _isBackgroundMusicOn;
    private bool _isSFXOn;

    public Action<bool> OnBackgroundMusicToggleChanged;
    public Action<bool> OnSFXToggleChanged;

    public bool IsBackgroundMusicOn
    {
        get => _isBackgroundMusicOn;
        set
        {
            if (value != _isBackgroundMusicOn)
            {
                OnBackgroundMusicToggleChanged?.Invoke(value);
            }
            _isBackgroundMusicOn = value;
            SaveData();
        }
    }
    public bool IsSFXOn
    {
        get => _isSFXOn;
        set
        {
            if (value != _isSFXOn)
            {
                OnSFXToggleChanged?.Invoke(value);
            }
            _isSFXOn = value;
            SaveData();
        }
    }

    public void LoadData()
    {
        if (!PlayerPrefs.HasKey(_isBackgroundMusicOnKey))
        {
            PlayerPrefs.SetInt(_isBackgroundMusicOnKey, 1);
        }
        _isBackgroundMusicOn = PlayerPrefs.GetInt(_isBackgroundMusicOnKey) == 1;

        if (!PlayerPrefs.HasKey(_isSFXOnKey))
        {
            PlayerPrefs.SetInt(_isSFXOnKey, 1);
        }
        _isSFXOn = PlayerPrefs.GetInt(_isSFXOnKey) == 1;

    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(_isBackgroundMusicOnKey, _isBackgroundMusicOn ? 1 : 0);
        PlayerPrefs.SetInt(_isSFXOnKey, _isSFXOn ? 1 : 0);
    }
}
