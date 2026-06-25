using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHittable hittable))
        {
            hittable.OnMinorHit(this.gameObject);
        }
    }
}
