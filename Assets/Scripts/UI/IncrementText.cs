using System;
using TMPro;
using UnityEngine;

public class IncrementText : MonoBehaviour, IPoolObject
{
    [SerializeField] private TextMeshProUGUI _mainText;
    [SerializeField] private Color _increaseColor;
    [SerializeField] private Color _decreaseColor;

    public Action<GameObject> OnReturnToPool { get; set; }

    public void SetIncrement(int increment)
    {
        _mainText.text = (increment >= 0) ?
            "+" + increment.ToString() :
            increment.ToString();

        _mainText.color = (increment >= 0) ?
            _increaseColor :
            _decreaseColor;
    }

    public void Finished()
    {
        OnReturnToPool?.Invoke(gameObject);
    }
}
