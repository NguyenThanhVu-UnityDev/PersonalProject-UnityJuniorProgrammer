using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _grapeCollectedText;
    [SerializeField] private string _grapeCollectedTextBounceTrigger = "Bounce";
    [SerializeField] private IncrementText _grapeCollectedIncrementText;

    private static UIManager _instance = null;
    private Animator _grapeCollectedTextAnimator;

    public static UIManager Instance { get => Instance; }

    private void Awake()
    {
        if (_instance != null & _instance != this)
        {
            Destroy(_instance);
        }
        _instance = this;

        if (_grapeCollectedText != null)
        {
            _grapeCollectedTextAnimator = _grapeCollectedText.GetComponent<Animator>();
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(DemoIncrement), 0f, 0.2f);
    }

    public void DemoIncrement()
    {
        UpdateGrapeCollectedUI(Random.Range(0, 100));
    }
    private void UpdateGrapeCollectedUI(int newAmount)
    {
        if (_grapeCollectedText != null)
        {
            // Set increment
            if (_grapeCollectedIncrementText != null && 
                PoolManager.Instance != null &&
                int.TryParse(_grapeCollectedText.text, out int previousAmount))
            {
                int increment = newAmount - previousAmount;
                Debug.Log("Increment: " + increment);
                var newIncrementText = PoolManager.Instance.SpawnObject(_grapeCollectedIncrementText, _grapeCollectedIncrementText.transform.position, Quaternion.identity);
                
                if (newIncrementText != null)
                {
                    newIncrementText.SetIncrement(increment);
                }
            }

            _grapeCollectedText.text = newAmount.ToString();
            if (_grapeCollectedTextAnimator != null)
            {
                _grapeCollectedTextAnimator.SetTrigger(_grapeCollectedTextBounceTrigger);
            }
        }
    }
}
