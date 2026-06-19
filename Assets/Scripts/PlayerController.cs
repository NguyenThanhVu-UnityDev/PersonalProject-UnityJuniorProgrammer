using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float cooldownTime = 0.1f;
    [Min(1)]
    [SerializeField] int tracksCount = 1;
    [SerializeField] float trackWidth = 10;
    [SerializeField] float trackSpacing = 5;
    [SerializeField] Vector3 centerPosition = new();
    [SerializeField] float switchingTrackTime = 0.5f;

    [SerializeField] InputAction moveAction;
    [SerializeField] InputAction jumpAction;

    private int previousTrackIndex = 0;
    private int currentTrackIndex = 0;
    private Vector3 targetPosition = new();

    private Coroutine switchTrackCoroutine = null;
    private Coroutine cooldownCoroutine = null;
    private Rigidbody playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        transform.position = GetCurrentTrackPosition();
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        Move();
        if (jumpAction.triggered) Jump();
    }

    private void Move()
    {
        if (cooldownCoroutine != null) return;

        Vector2 moveInput = moveAction.ReadValue<Vector2>().normalized;
        float horizontalInput = moveInput.x;

        if (horizontalInput > 0) MoveRight();
        else if (horizontalInput < 0) MoveLeft();
    }

    public void MoveLeft()
    {
        if (switchTrackCoroutine != null || playerRb == null) return;

        Debug.Log("Try move left");

        if (currentTrackIndex <= 0)
        {
            currentTrackIndex = 0;
            previousTrackIndex = 0;
            return;
        }

        previousTrackIndex = currentTrackIndex;
        currentTrackIndex--;
        currentTrackIndex = Mathf.Clamp(currentTrackIndex, 0, tracksCount - 1);

        // update target and start switching routine (stop any existing one first)
        targetPosition = GetCurrentTrackPosition();
        switchTrackCoroutine = StartCoroutine(SwitchTrackRoutine());
    }

    public void MoveRight()
    {
        if (switchTrackCoroutine != null || playerRb == null) return;

        Debug.Log("Try move right");

        if (currentTrackIndex >= tracksCount - 1)
        {
            previousTrackIndex = currentTrackIndex = tracksCount - 1;
            return;
        }

        previousTrackIndex = currentTrackIndex;
        currentTrackIndex++;
        currentTrackIndex = Mathf.Clamp(currentTrackIndex, 0, tracksCount - 1);

        // update target and start switching routine (stop any existing one first)
        targetPosition = GetCurrentTrackPosition();
        switchTrackCoroutine = StartCoroutine(SwitchTrackRoutine());
    }

    public void CancelMove()
    {
        if (switchTrackCoroutine != null) StopCoroutine(switchTrackCoroutine);
        switchTrackCoroutine = null;

        if (transform.position.x < targetPosition.x) MoveLeft();
        else MoveRight();
    }

    public void Jump()
    {
        if (playerRb != null) playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private Vector3 GetCurrentTrackPosition()
    {
        // Clamp index to be safe
        int index = Mathf.Clamp(currentTrackIndex, 0, tracksCount - 1);

        // total width occupied by lane centers
        float totalWidth = tracksCount * trackWidth + (tracksCount - 1) * trackSpacing;

        // x position of the first (left-most) lane center
        float firstLaneX = centerPosition.x - totalWidth * 0.5f + trackWidth * 0.5f;

        // distance between adjacent lane centers
        float stride = trackWidth + trackSpacing;

        float x = firstLaneX + index * stride;
        return new Vector3(x, centerPosition.y, centerPosition.z);
    }

    IEnumerator SwitchTrackRoutine()
    {
        Vector3 start = transform.position;
        Vector3 end = targetPosition;

        if (switchingTrackTime <= 0f)
        {
            transform.position = end;
            switchTrackCoroutine = null;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < switchingTrackTime)
        {
            end.y = transform.position.y;
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / switchingTrackTime);
            Vector3 nextPos = Vector3.Lerp(start, end, t);
            if (playerRb != null) playerRb.MovePosition(nextPos);
            yield return null;
        }

        transform.position = end;
        switchTrackCoroutine = null;
    }

    private void StartCooldown()
    {
        if (cooldownCoroutine != null) return;

        cooldownCoroutine = StartCoroutine(CooldownCoroutine());
    }

    IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(cooldownTime);
        cooldownCoroutine = null;
    }
}
