using UnityEngine;

public class MovingObjectsDespawnZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MovingObject>() != null)
        {
            other.gameObject.SetActive(false);
            Debug.Log($"[MovingObjectDespawnZone] Detect moving object: {other.name}");
        }
    }
}
