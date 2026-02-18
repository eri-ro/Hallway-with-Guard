using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    public float speed = 4.5f;
    private float pitch;

    void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>()?.transform;
    }

    void Update()
    {
        Look();
        Move();
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * 2f; // x2 for faster mouse look, might make variable?
        float mouseY = Input.GetAxis("Mouse Y") * 2f;

        // Yaw rotates the player body
        transform.Rotate(Vector3.up * mouseX);

        // Pitch rotates camera up/down
        pitch -= mouseY;
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z);
        Vector3 velocity = move * speed;
        GetComponent<CharacterController>().Move(velocity * Time.deltaTime);
    }
}
