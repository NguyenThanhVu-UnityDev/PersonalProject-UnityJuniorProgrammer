using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour
{
    [SerializeField] AudioClip _clickSound;
    [SerializeField] float _clickVolume = 0.3f;
    [SerializeField] float _delayTime = 0.2f;

    private Button _button;
    private Coroutine _delayClickCoroutine = null;

    public Action OnClick { get; set; }

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(Click);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Click);
    }

    private void Click()
    {
        UIEvents.PlaySFX(_clickSound, _clickVolume);

        if (_delayClickCoroutine != null)
        {
            StopCoroutine(_delayClickCoroutine);
        }
        _delayClickCoroutine = StartCoroutine(DelayClickCoroutine());
    }

    private IEnumerator DelayClickCoroutine()
    {
        yield return new WaitForSecondsRealtime(_delayTime);

        OnClick?.Invoke();
        _delayClickCoroutine = null;
    }
}
