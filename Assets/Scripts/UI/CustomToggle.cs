using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class CustomToggle : MonoBehaviour
{
    [SerializeField] Image _targetGraphic;
    [SerializeField] Sprite _isOnSprite;
    [SerializeField] Sprite _isOffSprite;

    [SerializeField] AudioClip _clickSound;
    [SerializeField] float _clickVolume = 0.3f;

    private Toggle _toggle;

    public Action<bool> OnValueChanged;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        _toggle.onValueChanged.AddListener(ValueChanged);
    }

    private void OnDisable()
    {
        _toggle.onValueChanged.RemoveListener(ValueChanged);
    }

    public void SetToggle(bool value)
    {
        _toggle.isOn = value;
    }

    private void ValueChanged(bool value)
    {
        if (_targetGraphic == null)
        {
            Debug.LogWarning("[CustomToggle] Please assign a target graphic");
        }
        else
        {
            _targetGraphic.sprite = (value) ? _isOnSprite : _isOffSprite;
        }

        UIEvents.PlaySFX(_clickSound, _clickVolume);
        OnValueChanged?.Invoke(value);
    }
}
