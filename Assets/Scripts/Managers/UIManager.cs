using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;

    public static UIManager Instance { get => Instance; }

    private void Awake()
    {
        if (_instance != null & _instance != this)
        {
            Destroy(_instance);
        }
        _instance = this;
    }

    private void OnEnable()
    {
        //PlayerEvents.OnCollectedGrapeChanged += UpdateGrapeCollectedUI;
    }

    private void OnDisable()
    {
        //PlayerEvents.OnCollectedGrapeChanged -= UpdateGrapeCollectedUI;
    }

    private void UpdateGrapeCollectedUI(int newAmount)
    {
    }
}
