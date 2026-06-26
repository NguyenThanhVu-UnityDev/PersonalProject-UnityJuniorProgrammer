using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool _flipped = false;
    private void Update()
    {
        if (Camera.main == null)
        {
            Debug.LogError($"{name} cannot find main camera to look at!");
        }
        else
        {
            transform.LookAt(Camera.main.transform, (_flipped) ? Vector3.down : Vector3.up);
        }
    }
}
