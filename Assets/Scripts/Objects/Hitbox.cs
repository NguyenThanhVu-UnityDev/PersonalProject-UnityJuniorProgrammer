using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] int _damage = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHittable hittable))
        {
            hittable.OnMinorHit(this.gameObject, other);
        }

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
        }
    }
}
