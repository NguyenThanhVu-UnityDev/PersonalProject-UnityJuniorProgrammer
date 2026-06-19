using UnityEngine;

public class TrainDespawnZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Train>() != null)
        {
            other.gameObject.SetActive(false);
        }
    }
}
