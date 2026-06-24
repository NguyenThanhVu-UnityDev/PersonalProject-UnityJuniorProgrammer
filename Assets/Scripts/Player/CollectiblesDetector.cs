using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectiblesDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Collectible collectible))
        {
            collectible.Collect(this);
        }
    }
}
