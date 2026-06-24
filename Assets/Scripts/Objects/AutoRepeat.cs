using UnityEngine;

public class AutoRepeat : MonoBehaviour
{
    public enum RepeatAxis { Horizontal, Vertical, Both, None};

    [SerializeField] RepeatAxis _repeatAxis;
    [Tooltip("Can either use the initial position or the pivot as the repeating point")]
    [SerializeField] private bool _useInitialPosition = true;
    [SerializeField] private Vector3 _defaultPivot = Vector3.zero;
    [SerializeField] MeshRenderer _renderer;

    private Vector3 _pivot;
    private Vector3 _objectSize = new();

    private void Start()
    {
        if (_renderer != null)
        {
            _objectSize = _renderer.bounds.size;
        }

        if (_useInitialPosition) _pivot = transform.position;
        else _pivot = _defaultPivot;
    }

    private void Update()
    {
        CheckBoundExceed();
    }

    private void CheckBoundExceed()
    {
        switch (_repeatAxis)
        {
            case RepeatAxis.Horizontal:
                CheckHorizontalAxisExceed();
                break;
            case RepeatAxis.Vertical:
                CheckVerticalAxisExceed();
                break;
            case RepeatAxis.Both:
                CheckHorizontalAxisExceed();
                CheckVerticalAxisExceed();
                break;
            case RepeatAxis.None:
            default:
                return;
        }
    }

    private void CheckHorizontalAxisExceed()
    {
        Vector3 targetPosition = transform.position;
        if (targetPosition.x < _pivot.x - _objectSize.x * 0.5f || 
            targetPosition.x > _pivot.x + _objectSize.x * 0.5f) 
            targetPosition.x = _pivot.x;
        transform.position = targetPosition;
    }

    private void CheckVerticalAxisExceed()
    {
        Vector3 targetPosition = transform.position;
        if (targetPosition.z < _pivot.z - _objectSize.z * 0.5f || 
            targetPosition.z > _pivot.z + _objectSize.z * 0.5f) 
            targetPosition.z = _pivot.z;
        transform.position = targetPosition;
    }
}
