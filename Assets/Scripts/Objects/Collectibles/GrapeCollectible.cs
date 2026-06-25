using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GrapeCollectible : Collectible
{
    [SerializeField] private int _heal = 1;
    public override void Collect(CollectiblesDetector target)
    {
        if (target.gameObject.TryGetComponent(out IHealable healable))
        {
            healable.Heal(_heal);
        }

        base.Collect(target);
    }
}
