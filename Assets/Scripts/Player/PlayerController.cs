using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _currentPlayer;
    public static PlayerController CurrentPlayer { get => _currentPlayer; }

    [SerializeField] float _defaultRunSpeed = 20f;
    [SerializeField] float _maxRunSpeed = 100f;
    [SerializeField] float _jumpForce = 5f;
    [SerializeField] float _dropForce = 5f;
    [SerializeField] float _cooldownTime = 0.1f;
    [Min(1)]
    [SerializeField] int _tracksCount = 1;
    [SerializeField] float _trackWidth = 10;
    [SerializeField] float _trackSpacing = 5;
    [SerializeField] Vector3 _centerPosition = new();
    [SerializeField] float _switchingTrackTime = 0.5f;

    [SerializeField] InputAction _moveAction;
    [SerializeField] InputAction _jumpAction;
    [SerializeField] InputAction _instantDropAction;

    private bool _isRunning = false;

    private int _currentTrackIndex = 0;
    private Vector3 _targetPosition = new();

    private Coroutine _switchTrackCoroutine = null;
    private Coroutine _cooldownCoroutine = null;
    private CustomPhysics _customPhysics;
    private float _runSpeed;

    public float RunSpeed { get => _runSpeed; }

    private void Awake()
    {
        _customPhysics = GetComponent<CustomPhysics>();
        _currentPlayer = this;
    }

    private void Start()
    {
        _isRunning = true;
        transform.position = GetCurrentTrackPosition();
        ResetRunSpeed();
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
        _instantDropAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _instantDropAction.Disable();
    }

    private void Update()
    {
        if (!_isRunning) return;

        Move();
        if (_jumpAction.triggered) Jump();
        if (_instantDropAction.triggered) InstantDrop();
    }

    public void ResetRunSpeed()
    {
        _runSpeed = _defaultRunSpeed;
    }

    public void AddRunSpeed(float increment)
    {
        _runSpeed += increment;
        if (_runSpeed > _maxRunSpeed) _runSpeed = _maxRunSpeed;
    }

    private void Move()
    {
        if (_cooldownCoroutine != null || !_isRunning) return;

        Vector2 moveInput = _moveAction.ReadValue<Vector2>().normalized;
        float horizontalInput = moveInput.x;

        if (horizontalInput > 0) MoveRight();
        else if (horizontalInput < 0) MoveLeft();
    }

    public void MoveLeft()
    {
        if (_switchTrackCoroutine != null || !_isRunning) return;

        if (_currentTrackIndex <= 0)
        {
            _currentTrackIndex = 0;
            return;
        }

        _currentTrackIndex--;
        _currentTrackIndex = Mathf.Clamp(_currentTrackIndex, 0, _tracksCount - 1);

        // update target and start switching routine (stop any existing one first)
        _targetPosition = GetCurrentTrackPosition();
        _switchTrackCoroutine = StartCoroutine(SwitchTrackRoutine());
    }

    public void MoveRight()
    {
        if (_switchTrackCoroutine != null || !_isRunning) return;

        if (_currentTrackIndex >= _tracksCount - 1)
        {
            return;
        }

        _currentTrackIndex++;
        _currentTrackIndex = Mathf.Clamp(_currentTrackIndex, 0, _tracksCount - 1);

        // update target and start switching routine (stop any existing one first)
        _targetPosition = GetCurrentTrackPosition();
        _switchTrackCoroutine = StartCoroutine(SwitchTrackRoutine());
    }

    public void CancelMove()
    {
        if (_switchTrackCoroutine != null) StopCoroutine(_switchTrackCoroutine);
        _switchTrackCoroutine = null;

        if (transform.position.x < _targetPosition.x) MoveLeft();
        else MoveRight();
    }

    public void Jump()
    {
        if (_customPhysics != null && _customPhysics.IsOnGround)
        {
            _customPhysics.AddInstantForce(Vector3.up * _jumpForce);
        }
    }

    public void InstantDrop()
    {
        if (_customPhysics != null && !_customPhysics.IsOnGround)
        {
            _customPhysics.SetVelocityY(0);
            _customPhysics.AddInstantForce(Vector3.down * _dropForce);
        }
    }

    private Vector3 GetCurrentTrackPosition()
    {
        // Clamp index to be safe
        int index = Mathf.Clamp(_currentTrackIndex, 0, _tracksCount - 1);

        // total width occupied by lane centers
        float totalWidth = _tracksCount * _trackWidth + (_tracksCount - 1) * _trackSpacing;

        // x position of the first (left-most) lane center
        float firstLaneX = _centerPosition.x - totalWidth * 0.5f + _trackWidth * 0.5f;

        // distance between adjacent lane centers
        float stride = _trackWidth + _trackSpacing;

        float x = firstLaneX + index * stride;
        return new Vector3(x, _centerPosition.y, _centerPosition.z);
    }

    public void Stop()
    {
        _isRunning = false;
        _runSpeed = 0;
    }

    IEnumerator SwitchTrackRoutine()
    {
        Vector3 start = transform.position;
        Vector3 end = _targetPosition;

        if (_switchingTrackTime <= 0f)
        {
            transform.position = end;
            _switchTrackCoroutine = null;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < _switchingTrackTime)
        {
            end.y = transform.position.y;
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _switchingTrackTime);
            Vector3 nextPos = transform.position;
            nextPos.x = Mathf.Lerp(start.x, end.x, t);
            transform.position = nextPos;

            yield return null;
        }

        transform.position = end;
        _switchTrackCoroutine = null;
        _cooldownCoroutine = StartCoroutine(CooldownCoroutine());
    }

    IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(_cooldownTime);
        _cooldownCoroutine = null;
    }
}
