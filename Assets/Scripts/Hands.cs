using UnityEngine;

public class Hands : MonoBehaviour
{
    public PlayerController playerController;

    public float bobSpeed = 8f;
    public float bobAmountY = 0.03f;
    public float bobAmountX = 0.02f;
    public float returnSpeed = 6f;
    public float crouchBobMultiplier = 0.5f;

    private Vector3 startLocalPosition;
    private float bobTimer;

    private void Awake()
    {
        startLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        Vector2 moveInput = playerController.MoveInput;
        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            float bobScale = playerController.IsCrouching ? crouchBobMultiplier : 1f;

            bobTimer += Time.deltaTime * bobSpeed;

            float bobX = Mathf.Sin(bobTimer * 0.5f) * bobAmountX * bobScale;
            float bobY = Mathf.Abs(Mathf.Sin(bobTimer)) * bobAmountY * bobScale;

            transform.localPosition = startLocalPosition + new Vector3(bobX, bobY, 0f);
        }
        else
        {
            bobTimer = 0f;
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                startLocalPosition,
                Time.deltaTime * returnSpeed
            );
        }
    }
}