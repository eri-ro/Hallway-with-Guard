using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float crouchSpeed = 4f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = .1f;
    [SerializeField] private float controllerSensitivity = 120f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    [Header("Crouch")]
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchingHeight = 1.2f;
    [SerializeField] private float standingCameraY = 1.6f;
    [SerializeField] private float crouchingCameraY = 1.0f;
    [SerializeField] private float crouchTransitionSpeed = 10f;
    [SerializeField] private LayerMask ceilingMask;

    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction crouchAction;
    private InputAction pause;

    private float pitch;
    private float verticalVelocity;
    private bool isCrouching;

    public Vector2 MoveInput { get; private set; }
    public bool IsGrounded => controller != null && controller.isGrounded;
    public bool IsCrouching => isCrouching;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        crouchAction = playerInput.actions["Crouch"];
        pause = playerInput.actions["Pause"];

        cameraTransform = GetComponentInChildren<Camera>()?.transform;

        standingHeight = controller.height;
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (pause.WasPressedThisFrame() && GameManager.Instance.gameOver == false)
        {
            GameManager.Instance.TogglePause();
        }

        Crouch();
        Look();
        Move();
    }

    private void Look()
    {
        if (cameraTransform == null) return;

        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        bool usingGamepad = Gamepad.current != null && Gamepad.current.rightStick.IsActuated();

        float lookX;
        float lookY;

        if (usingGamepad)
        {
            lookX = lookInput.x * controllerSensitivity * Time.deltaTime;
            lookY = lookInput.y * controllerSensitivity * Time.deltaTime;
        }
        else
        {
            lookX = lookInput.x * mouseSensitivity;
            lookY = lookInput.y * mouseSensitivity;
        }

        transform.Rotate(Vector3.up * lookX);

        pitch -= lookY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void Move()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        MoveInput = moveInput;

        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y);
        move = Vector3.ClampMagnitude(move, 1f);

        float currentSpeed = isCrouching ? crouchSpeed : walkSpeed;

        Vector3 velocity = move * currentSpeed;

        controller.Move(velocity * Time.deltaTime);
    }

    private void Crouch()
    {
        bool crouchHeld = crouchAction != null && crouchAction.IsPressed();

        if (crouchHeld)
        {
            isCrouching = true;
        }
        else
        {
            // Only stand up if there is room
            if (!IsBlockedAbove())
                isCrouching = false;
        }

        float targetHeight = isCrouching ? crouchingHeight : standingHeight;
        float targetCameraY = isCrouching ? crouchingCameraY : standingCameraY;

        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);

        Vector3 center = controller.center;
        center.y = controller.height * 0.5f;
        controller.center = center;

        Vector3 camPos = cameraTransform.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCameraY, Time.deltaTime * crouchTransitionSpeed);
        cameraTransform.localPosition = camPos;
    }

    private bool IsBlockedAbove()
    {
        float radius = controller.radius * 0.95f;
        float checkDistance = standingHeight - controller.height;

        Vector3 origin = transform.position + Vector3.up * controller.height;

        return Physics.SphereCast(origin, radius, Vector3.up, out _, checkDistance, ceilingMask);
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}