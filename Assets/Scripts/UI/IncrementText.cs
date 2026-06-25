using TMPro;
using UnityEngine;

public class IncrementText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _mainText;
    [SerializeField] private Color _increaseColor;
    [SerializeField] private Color _decreaseColor;

    public void SetIncrement(int increment)
    {
        _mainText.text = increment.ToString();

        _mainText.color = (increment > 0) ?
            _increaseColor :
            _decreaseColor;
    }

    public void Finished()
    {
        gameObject.SetActive(false);
    }

}
