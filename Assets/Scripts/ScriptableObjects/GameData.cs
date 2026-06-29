using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] private int _collectedGrape;

    public Action OnCollectedGrapeChanged;
    public Action OnDead;

    public int CollectedGrape 
    { 
        get => _collectedGrape; 
        set
        {
            if (value < 0) _collectedGrape = 0;
            _collectedGrape = value;
            OnCollectedGrapeChanged?.Invoke();
        } 
    }

    public void AddGrape(int amount)
    {
        CollectedGrape += amount;
        OnCollectedGrapeChanged?.Invoke();
    }

    public void RemoveGrape(int amount)
    {
        if (_collectedGrape <= 0)
        {
            _collectedGrape = 0;
            return;
        }

        _collectedGrape -= amount;
        OnCollectedGrapeChanged?.Invoke();

        if (_collectedGrape <= 0)
        {
            _collectedGrape = 0;
            OnDead?.Invoke();
        }
    }
}
