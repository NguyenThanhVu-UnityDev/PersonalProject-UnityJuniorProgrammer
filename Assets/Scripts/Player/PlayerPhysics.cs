using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Physics Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [Tooltip("This is the actual position (relative) of the overlap box of the player")]
    [SerializeField] private Vector3 _groundPoint = new();
    [Tooltip("Offst of the the overlap box position before using BoxRay, used for a more accurate detection")]
    [Min(0.01f)]
    [SerializeField] private float _safeZone = 0.01f;
    [SerializeField] private Vector3 _overlapBoxHalfExtents = new(0.5f, 0.5f, 0.5f);
    [Min(0.1f)]
    [Tooltip("If the overlap is this close to the ground, the player will be snapped onto the ground instantly")]
    [SerializeField] private float _minDistanceToSnap = 0.1f;
    [Tooltip("Custom gravity that apply to the player velocity over time")]
    [SerializeField] private float _gravityForce = 9.8f;

    [Header("Gizmos Settings")]
    [SerializeField] private bool _drawGizmos = true;
    [SerializeField] private Color _overlapBoxColor = Color.green;


    private Vector3 _velocity = new();
    private bool _isOnGround = false;

    public bool IsOnGround => _isOnGround;
    public bool IsFalling => Vector3.Dot(Vector3.down, _velocity) > 0;

    public void AddInstantForce(Vector3 force)
    {
        _velocity += force;
    }

    private void Update()
    {
        ApplyGravity();
        DetectGround();
        AdjustPosition();
        ApplyVelocity();
    }

    private void ApplyGravity()
    {
        _velocity += Vector3.down * _gravityForce * Time.deltaTime;
    }

    private void DetectGround()
    {
        if (IsFalling && Physics.BoxCast((transform.position + _groundPoint) + Vector3.up * _safeZone, _overlapBoxHalfExtents, Vector3.down, out RaycastHit hitInfo, Quaternion.identity, _safeZone + _minDistanceToSnap, _groundLayer))
        {
            float distanceToSnap = hitInfo.distance - _safeZone;
            transform.position += Vector3.down * distanceToSnap;

            _isOnGround = true;
            _velocity.y = 0;
        }
        else _isOnGround = false;
    }

    private void AdjustPosition()
    {

    }

    private void ApplyVelocity()
    {
        transform.position += _velocity;
    }

    private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;

        Gizmos.color = _overlapBoxColor;
        Gizmos.DrawWireCube(transform.position + _groundPoint, new(_overlapBoxHalfExtents.x * 2.0f, _overlapBoxHalfExtents.y * 2.0f, _overlapBoxHalfExtents.z * 2.0f));
    }

}
