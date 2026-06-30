using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelView : MonoBehaviour
{
    [SerializeField] string _closeTrigger = "Close";

    [Header("UI Elements")]
    [SerializeField] CustomButton _closeButton;

    private Animator _animator;

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

}
