using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable, IHealable, IHittable
{
    [SerializeField] private int _initHealth = 1;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _takeDamageCooldownTime = 0.5f;

    private int _health;

    private Coroutine _takeDamageCoolDownCoroutine = null;

    private void Start()
    {
        _health = _initHealth;
    }

    public void Heal(int heal)
    {
        if (_health >= _maxHealth) return;

        _health += heal;

        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }

        Debug.Log("Current health: " + _health);
    }

    public void TakeDamage(int damage)
    {
        if (_health <= 0 || _takeDamageCoolDownCoroutine != null) return;

        _health -= damage;

        if (_health < 0)
        {
            _health = 0;
        }

        _takeDamageCoolDownCoroutine = StartCoroutine(TakeDamageCooldownCoroutine());

        Debug.Log("Current health: " + _health);
    }

    private IEnumerator TakeDamageCooldownCoroutine()
    {
        yield return new WaitForSeconds(_takeDamageCooldownTime);
    }

    public void OnMinorHit(GameObject hitObj)
    {
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
        Debug.Log("Die");
    }
}
