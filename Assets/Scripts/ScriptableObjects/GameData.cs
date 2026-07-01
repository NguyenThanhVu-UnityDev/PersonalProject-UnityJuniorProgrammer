using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] private int _collectedGrape;

    private bool _isGameStarted = false;

    public bool IsGameStarted => _isGameStarted;

    public Action OnGameStarted;
    public Action OnGameEnded;
    public Action OnCollectedGrapeChanged;
    public Action OnDead;

    private void OnEnable()
    {
        _isGameStarted = false;
    }

    public void StartGame()
    {
        if (!_isGameStarted)
        {
            OnGameStarted?.Invoke();
        }
        _isGameStarted = true;
    }

    public void StopGame()
    {
        if (_isGameStarted)
        {
            OnGameEnded?.Invoke();
        }
        _isGameStarted = false;
    }

    public int CollectedGrape
    {
        get => _collectedGrape;
        set
        {
            Debug.Log($"[GameData] Set collected grape: {value}");

            if (value < 0) _collectedGrape = 0;
            _collectedGrape = value;
            OnCollectedGrapeChanged?.Invoke();
        }
    }

    public void AddGrape(int amount)
    {
        CollectedGrape += amount;
    }

    public void RemoveGrape(int amount)
    {
        if (_collectedGrape <= 0)
        {
            _collectedGrape = 0;
            return;
        }

        CollectedGrape -= amount;

        if (_collectedGrape <= 0)
        {
            _collectedGrape = 0;
            OnDead?.Invoke();
        }
    }
}
