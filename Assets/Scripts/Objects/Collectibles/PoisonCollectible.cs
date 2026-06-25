using UnityEngine;

public class PoisonCollectible : Collectible
{
    [SerializeField] private int _damage = 1;

    public override void Collect(CollectiblesDetector target)
    {
        if (target.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
        }

        base.Collect(target);
    }
}
