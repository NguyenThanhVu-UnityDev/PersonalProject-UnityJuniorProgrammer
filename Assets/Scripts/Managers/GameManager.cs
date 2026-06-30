using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float _acceleration;
    [SerializeField] AudioClip _gameOverAudio;
    [SerializeField] float _gameOverVolume = 0.3f;

    private bool _isGameRunning = false;

    private void OnEnable()
    {
        PlayerEvents.OnPlayerDead += OnPlayerDead;
        GameEvents.OnPause += PauseGame;
        GameEvents.OnResume += ResumeGame;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerDead -= OnPlayerDead;
        GameEvents.OnPause -= PauseGame;
        GameEvents.OnResume -= ResumeGame;
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
            PlayerController.CurrentPlayer.AddRunSpeed(_acceleration * Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        ResumeGame();
    }

    private void OnPlayerDead()
    {
        _isGameRunning = false;
        UIEvents.PlaySFX(_gameOverAudio, _gameOverVolume);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
