using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable, IHealable, IHittable
{
    [SerializeField] private int _initHealth = 1;
    [SerializeField] private float _takeDamageCooldownTime = 0.5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _dieTrigger = "Die";

    private int _health;

    private Coroutine _takeDamageCoolDownCoroutine = null;

    public bool IsDead => _health <= 0;

    private void Start()
    {
        _health = _initHealth;

        PlayerEvents.RaiseCollectedGrapeChanged(_health);
    }

    public void Heal(int heal)
    {
        if (IsDead) return;

        _health += heal;

        PlayerEvents.RaiseCollectedGrapeChanged(_health);
    }

    public void TakeDamage(int damage)
    {
        if (_health <= 0 || _takeDamageCoolDownCoroutine != null) return;

        _health -= damage;

        if (_health <= 0)
        {
            _health = 0;
            Dead();
        }

        _takeDamageCoolDownCoroutine = StartCoroutine(TakeDamageCooldownCoroutine());

        PlayerEvents.RaiseCollectedGrapeChanged(_health);
    }

    private IEnumerator TakeDamageCooldownCoroutine()
    {
        yield return new WaitForSeconds(_takeDamageCooldownTime);
        _takeDamageCoolDownCoroutine = null;
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

        _health = 0;
        PlayerEvents.RaiseCollectedGrapeChanged(_health);
        PlayerEvents.RaisePlayerDead();

        if (TryGetComponent(out PlayerController playerController))
        {
            playerController.CancelMove();
            playerController.Stop();
        }
    }
}
