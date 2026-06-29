using UnityEngine;

public class CollectedGrapePresenter : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private CollectedGrapeView _view;

    private void OnEnable()
    {
        if (_gameData != null)
        {
            _gameData.OnCollectedGrapeChanged += OnCollectedGrapeChanged;
        }
        else
        {
            Debug.LogWarning("[PlayerHealth] No game data is assigned!");
        }
    }

    private void OnCollectedGrapeChanged()
    {
        if (_view == null)
        {
            Debug.LogWarning("[CollectedGrapePresenter] No view is found!");
            return;
        }

        _view.UpdateCollectedGrape(_gameData.CollectedGrape);
    }
}
