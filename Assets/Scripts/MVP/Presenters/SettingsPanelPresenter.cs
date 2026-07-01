using UnityEngine;

public class SettingsPanelPresenter : MonoBehaviour
{
    [SerializeField] private SettingsData _settingsData;
    [SerializeField] private SettingsPanelView _view;

    private void OnEnable()
    {
        if (_settingsData == null)
        {
            Debug.LogWarning("[SettingsPanelPresenter] Please assign a settings data!");
        }
        else
        {
            _settingsData.LoadData();
        }

        if (_view == null)
        {
            Debug.LogWarning("[SettingsPanelPresenter] Please assign a view!");
        }
        else
        {
            _view.OnBackgroundMusicToggleChanged += OnBackgroundMusicToggleChanged;
            _view.OnSFXToggleChanged += OnSFXToggleChanged;
        }
    }

    private void OnDisable()
    {
        if (_settingsData != null)
        {
            _settingsData.SaveData();
        }
    }

    private void Start()
    {
        if (_settingsData == null)
        {
            Debug.LogWarning("[SettingsPanelPresenter] No settings data is found!");
            return;
        }

        if (_view == null)
        {
            Debug.LogWarning("[SettingsPanelPresenter] No view is found!");
            return;
        }

        _view.LoadData(_settingsData);
    }

    private void OnBackgroundMusicToggleChanged(bool value)
    {
        if (_settingsData == null)
        {
            Debug.LogWarning("[SettingsPanelPresenter] No settings data is found!");
            return;
        }
        else
        {
            _settingsData.IsBackgroundMusicOn = value;
        }
    }

    private void OnSFXToggleChanged(bool value)
    {
        if (_settingsData == null)
        {
            Debug.LogWarning("[SettingsPanelPresenter] No settings data is found!");
            return;
        }
        else
        {
            Debug.Log("SFX Toggle Changed: " + value);
            _settingsData.IsSFXOn = value;
        }
    }
}
