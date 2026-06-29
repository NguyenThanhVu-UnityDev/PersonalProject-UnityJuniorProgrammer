using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GrapeCollectible : Collectible, IPoolObject
{
    [SerializeField] private int _heal = 1;

    public Action<GameObject> OnReturnToPool { get; set; }

    public override void Collect(CollectiblesDetector target)
    {
        if (target.gameObject.TryGetComponent(out IHealable healable))
        {
            healable.Heal(_heal);
        }

        base.Collect(target);
    }

    private void OnDisable()
    {
        OnReturnToPool?.Invoke(gameObject);
    }
}
