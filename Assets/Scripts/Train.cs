using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] float speed;
    private Rigidbody trainRb;

    private void Awake()
    {
        trainRb = GetComponent<Rigidbody>();
    }

    public float Speed { get => speed; set => speed = value; }
    public float RelativeSpeed { get => speed + ((PlayerController.CurrentPlayer != null) ? PlayerController.CurrentPlayer .RunSpeed : 0); }

    private void Update()
    {
        if (trainRb == null) return;
        Vector3 nextPos = transform.position + transform.forward * -1 * RelativeSpeed * Time.deltaTime;
        trainRb.MovePosition(nextPos);
    }
}
