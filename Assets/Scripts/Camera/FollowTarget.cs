using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public enum UpdateMode { Update, LateUpdate}

    [SerializeField] GameObject _target;
    [SerializeField] UpdateMode _updateMode = UpdateMode.LateUpdate;
    [Tooltip("Choose to use initial distance or a specific value as offset")]
    [SerializeField] bool _useInitDistance = true;
    [SerializeField] Vector3 _defaultOffset = Vector3.zero;
    [SerializeField] bool _followX;
    [SerializeField] bool _followY;
    [SerializeField] bool _followZ;

    private Vector3 _offset = Vector3.zero;

    private void Awake()
    {
        if (_target == null)
        {
            Debug.LogWarning("[FollowCamera] No target is assigned!");
            return;
        }

        _offset = (_useInitDistance) ?
            transform.position - _target.transform.position :
            _defaultOffset;
    }

    private void Update()
    {
        if (_updateMode != UpdateMode.Update)
        {
            return;
        }

        Follow();
    }

    private void LateUpdate()
    {
        if (_updateMode != UpdateMode.LateUpdate)
        {
            return;
        }

        Follow();
    }

    private void Follow()
    {
        if (_target == null)
        {
            Debug.LogWarning("[FollowCamera] No target is assigned!");
            return;
        }

        Vector3 targetPos = transform.position;

        if (_followX) targetPos.x = _target.transform.position.x + _offset.x;
        if (_followY) targetPos.y = _target.transform.position.y + _offset.y;
        if (_followZ) targetPos.z = _target.transform.position.z + _offset.z;

        transform.position = targetPos;
    }
}
