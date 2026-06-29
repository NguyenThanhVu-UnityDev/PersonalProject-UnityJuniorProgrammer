using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable, IHealable, IHittable
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private int _initCollectedGrape = 1;
    [SerializeField] private float _takeDamageCooldownTime = 0.5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _dieTrigger = "Die";

    private Coroutine _takeDamageCoolDownCoroutine = null;

    public bool IsDead => (_gameData != null) ? _gameData.CollectedGrape <= 0 : false;

    private void OnEnable()
    {
        if (_gameData != null)
        {
            _gameData.OnDead += OnDead;
        }
        else
        {
            Debug.LogWarning("[PlayerHealth] No game data is assigned!");
        }
    }

    private void OnDisable()
    {
        if (_gameData != null)
        {
            _gameData.OnDead -= OnDead;
        }
        else
        {
            Debug.LogWarning("[PlayerHealth] No game data is assigned!");
        }
    }

    private void Start()
    {
        _gameData.CollectedGrape = _initCollectedGrape;

        //PlayerEvents.RaiseCollectedGrapeChanged(_health);
    }

    public void Heal(int heal)
    {
        if (IsDead) return;

        if (_gameData == null)
        {
            Debug.LogWarning("[PlayerHealth] No game data is assigned!");
            return;
        }

        _gameData.AddGrape(heal);

        //PlayerEvents.RaiseCollectedGrapeChanged(_health);
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || _takeDamageCoolDownCoroutine != null) return;

        if (_gameData == null)
        {
            Debug.LogWarning("[PlayerHealth] No game data is assigned!");
            return;
        }

        _gameData.RemoveGrape(damage);

        _takeDamageCoolDownCoroutine = StartCoroutine(TakeDamageCooldownCoroutine());

        //PlayerEvents.RaiseCollectedGrapeChanged(_health);
    }

    private IEnumerator TakeDamageCooldownCoroutine()
    {
        yield return new WaitForSeconds(_takeDamageCooldownTime);
        _takeDamageCoolDownCoroutine = null;
    }

    private void OnDead()
    {
        Dead();
    }

    public void OnMinorHit(GameObject hitObj)
    {
        if (IsDead) return;

        if (TryGetComponent(out PlayerController playerController))
        {
            playerController.CancelMove();
            if (hitObj.transform.position.x < transform.position.x)
            {
                playerController.MoveRight();
            }
            else
            {
                playerController.MoveLeft();
            }
        }
    }

    public void OnMajorHit(GameObject hitObj)
    {
        Dead();
    }

    private void Dead()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(_dieTrigger);
        }

        //PlayerEvents.RaiseCollectedGrapeChanged(_health);
        PlayerEvents.RaisePlayerDead();

        if (TryGetComponent(out PlayerController playerController))
        {
            playerController.CancelMove();
            playerController.Stop();
        }
    }
}
