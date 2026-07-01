using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] SettingsData _settingsData;
    [SerializeField] AudioSource _backgroundMusic;
    [SerializeField] AudioSource _sfx;

    private static AudioManager _instance;

    public static AudioManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        UIEvents.PlayBackgroundMusic += PlayBackgroundMusic;
        UIEvents.PlaySFX += PlaySFX;

        if (_settingsData == null)
        {
            Debug.LogWarning("[AudioManager] Please assign a settings data!");
        }
        else
        {
            _settingsData.OnBackgroundMusicToggleChanged += OnBackgroundMusicDataChanged;
            _settingsData.OnSFXToggleChanged += OnSFXDataChanged;
            _settingsData.LoadData();
        }
    }

    private void OnDisable()
    {
        UIEvents.PlayBackgroundMusic -= PlayBackgroundMusic;
        UIEvents.PlaySFX -= PlaySFX;
        if (_settingsData != null)
        {
            _settingsData.OnBackgroundMusicToggleChanged -= OnBackgroundMusicDataChanged;
            _settingsData.OnSFXToggleChanged -= OnSFXDataChanged;
        }
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[AudioManager] Attempting to play a null clip!");
            return;
        }

        if (_backgroundMusic == null)
        {
            Debug.LogWarning("[AudioManager] Please assign a background music audio source!");
        }
        else
        {
            _backgroundMusic.Stop();
            _backgroundMusic.clip = clip;
            _backgroundMusic.Play();
        }
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        if (clip == null)
        {
            Debug.LogWarning("[AudioManager] Attempting to play a null clip!");
            return;
        }

        if (_sfx == null)
        {
            Debug.LogWarning("[AudioManager] Please assign an sfx audio source!");
        }
        else
        {
            _sfx.PlayOneShot(clip, volume);
        }
    }

    private void OnBackgroundMusicDataChanged(bool value)
    {
        if (_backgroundMusic == null)
        {
            Debug.LogWarning("[AudioManager] Please assign a background music audio source!");
        }
        else
        {
            _backgroundMusic.mute = !value;
        }
    }

    private void OnSFXDataChanged(bool value)
    {
        if (_sfx == null)
        {
            Debug.LogWarning("[AudioManager] Please assign an sfx audio source!");
        }
        else
        {
            _sfx.mute = !value;
        }
    }
}
