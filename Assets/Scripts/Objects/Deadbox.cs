using UnityEngine;

public class Deadbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHittable hittable))
        {
            hittable.OnMajorHit(this.gameObject);
        }
    }
}
