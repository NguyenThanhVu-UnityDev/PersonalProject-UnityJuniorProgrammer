using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float acceleration;

    private bool _isGameRunning = false;

    private void OnEnable()
    {
        PlayerEvents.OnPlayerDead += OnPlayerDead;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerDead -= OnPlayerDead;
    }

    private void Start()
    {
        _isGameRunning = true;
    }

    private void Update()
    {
        if (!_isGameRunning) return;

        if (PlayerController.CurrentPlayer != null)
        {
            PlayerController.CurrentPlayer.AddRunSpeed(acceleration * Time.deltaTime);
        }
    }

    private void OnPlayerDead()
    {
        _isGameRunning = false;
    }
}
