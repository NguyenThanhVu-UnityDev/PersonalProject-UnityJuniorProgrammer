using TMPro;
using UnityEngine;

public class CollectedGrapeView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _grapeCollectedText;
    [SerializeField] private string _grapeCollectedTextBounceTrigger = "Bounce";
    [SerializeField] private IncrementText _grapeCollectedIncrementText;

    private Animator _grapeCollectedTextAnimator;

    private void Awake()
    {
        if (_grapeCollectedText != null)
        {
            _grapeCollectedTextAnimator = _grapeCollectedText.GetComponent<Animator>();
        }
    }

    public void UpdateCollectedGrape(int newAmount)
    {
        if (_grapeCollectedText != null)
        {
            // Set increment
            if (_grapeCollectedIncrementText != null &&
                PoolManager.Instance != null &&
                int.TryParse(_grapeCollectedText.text, out int previousAmount))
            {
                int increment = newAmount - previousAmount;
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
