using System;
using UnityEngine;

public class PoisonCollectible : Collectible, IPoolObject
{
    [SerializeField] private int _damage = 1;

    public Action<GameObject> OnReturnToPool { get; set; }

    public override void Collect(CollectiblesDetector target)
    {
        if (target.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
        }

        base.Collect(target);
    }

    private void OnDisable()
    {
        OnReturnToPool?.Invoke(gameObject);
    }
}
