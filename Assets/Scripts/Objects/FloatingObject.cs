using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Float Speed Settings")]
    [SerializeField] bool _useRandomFloatSpeed = false;
    [SerializeField] float _defaultFloatSpeed = 5.0f;
    [SerializeField] float _minFloatSpeed = 1.0f;
    [SerializeField] float _maxFloatSpeed = 10f;

    [Header("Amplitude Settings")]
    [SerializeField] bool _useRandomAmplitude = false;
    [SerializeField] float _defaultAmplitude = 2f;
    [SerializeField] float _minAmplitude = 1.0f;
    [SerializeField] float _maxAmplitude = 3f;

    [Header("Rotation Settings")]
    [SerializeField] bool _useRandomStartAngle = true;
    [SerializeField] float _defaultStartAngle = 0.0f;
    [SerializeField] float _minStartAngle = 0.0f;
    [SerializeField] float _maxStartAngle = 365f;

    [SerializeField] bool _useRandomRotationSpeed = false;
    [SerializeField] float _defaultRotationSpeed = 30f;
    [SerializeField] float _minRotationSpeed = 10f;
    [SerializeField] float _maxRotationSpeed = 30f;

    private float _floatSpeed;
    private float _amplitude;
    private float _startAngle;
    private float _rotationSpeed;

    // store initial position so floating is stable relative to start
    private Vector3 _startPosition;

    private void Start()
    {
        InitValues();
        InitState();
    }

    private void InitValues()
    {
        _floatSpeed = (_useRandomFloatSpeed) ?
            Random.Range(_minFloatSpeed, _maxFloatSpeed) :
            _defaultFloatSpeed;

        _amplitude = (_useRandomAmplitude) ?
            Random.Range(_minAmplitude, _maxAmplitude) :
            _defaultAmplitude;

        _startAngle = (_useRandomStartAngle) ?
            Random.Range(_minStartAngle, _maxStartAngle) :
            _defaultStartAngle;

        _rotationSpeed = (_useRandomRotationSpeed) ?
            Random.Range(_minRotationSpeed, _maxRotationSpeed) :
            _defaultRotationSpeed;
    }

    private void InitState()
    {
        transform.rotation = Quaternion.Euler(0, _startAngle, 0);
        _startPosition = transform.position;
    }

    private void Update()
    {
        Float();
        Rotate();
    }

    private void Float()
    {
        // Use Mathf.Sin with Time.time to create smooth up/down motion.
        // _floatSpeed controls how fast the oscillation runs,
        // _amplitude controls the vertical range.
        float offset = Mathf.Sin(Time.time * _floatSpeed) * _amplitude;
        Vector3 next = _startPosition;
        next.y += offset;
        transform.position = next;
    }

    private void Rotate()
    {
        // Rotate around local Y axis using _rotationSpeed (degrees per second).
        if (Mathf.Approximately(_rotationSpeed, 0f)) return;
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.Self);
    }
}
