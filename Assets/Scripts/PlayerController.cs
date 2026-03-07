using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Movement")]
    [SerializeField] private float speed = 4.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float controllerSensitivity = 120f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;

    private float pitch;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];

        if (cameraTransform == null)
            cameraTransform = GetComponentInChildren<Camera>()?.transform;
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

        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y);
        move = Vector3.ClampMagnitude(move, 1f);
       
        Vector3 velocity = move * speed;

        controller.Move(velocity * Time.deltaTime);
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}