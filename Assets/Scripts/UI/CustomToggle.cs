using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class CustomToggle : MonoBehaviour
{
    [SerializeField] Image _targetGraphic;
    [SerializeField] Sprite _isOnSprite;
    [SerializeField] Sprite _isOffSprite;

    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        _toggle.onValueChanged.RemoveListener(OnValueChanged);
    }

    public void SetToggle(bool value)
    {
        _toggle.isOn = value;
    }

    private void OnValueChanged(bool value)
    {
        if (_targetGraphic == null)
        {
            Debug.LogWarning("[CustomToggle] Please assign a target graphic");
        }
        else
        {
            _targetGraphic.sprite = (value) ? _isOnSprite : _isOffSprite;
        }
    }
}
