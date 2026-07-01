using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float _acceleration;
    [SerializeField] AudioClip _gameOverAudio;
    [SerializeField] float _gameOverVolume = 0.3f;

    [SerializeField] float _restartDelay = 1f;

    private bool _isGameRunning = false;

    private Coroutine _restartCoroutine = null;

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

        if (_restartCoroutine == null)
        {
            _restartCoroutine = StartCoroutine(RestartGameCoroutine());
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private IEnumerator RestartGameCoroutine()
    {
        yield return new WaitForSeconds(_restartDelay);
        _restartCoroutine = null;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
