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

    private void Update()
    {
        if (trainRb == null) return;
        Vector3 nextPos = transform.position + transform.forward * -1 * speed * Time.deltaTime;
        trainRb.MovePosition(nextPos);
    }
}
