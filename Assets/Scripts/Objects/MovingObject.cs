using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] float speed;

    public float Speed { get => speed; set => speed = value; }
    public float RelativeSpeed { get => speed + ((PlayerController.CurrentPlayer != null) ? PlayerController.CurrentPlayer.RunSpeed : 0); }

    private void FixedUpdate()
    {
        Vector3 nextPos = transform.position + transform.forward * -1 * RelativeSpeed * Time.fixedDeltaTime;
        transform.position = nextPos;
    }
}
