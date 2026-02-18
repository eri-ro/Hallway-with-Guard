using UnityEngine;

public class GuardVision : MonoBehaviour
{
    public Transform eye;   // where vision originates (VisionPivot)
    public Transform player;        // assign Player transform

    public float viewDistance = 10f;
    public float viewAngle = 90f;   // cone angle
    public LayerMask obstructionMask; // Environment layer
    public LayerMask playerMask;    // Player layer

    public bool CanSeePlayer()
    {
        Vector3 toPlayer = player.position - eye.position;
        float dist = toPlayer.magnitude;
        if (dist > viewDistance) return false;

        // Angle check (half-angle on each side)
        float halfAngle = viewAngle * 0.5f;
        float angleToPlayer = Vector3.Angle(eye.forward, toPlayer);
        if (angleToPlayer > halfAngle) return false;

        // Line-of-sight check
        Vector3 origin = eye.position;
        Vector3 dir = toPlayer.normalized;

        // Raycast: if we hit an obstruction before the player, can't see them
        if (Physics.Raycast(origin, dir, out RaycastHit hit, viewDistance, obstructionMask | playerMask))
        {
            // We only "see" if the first hit is the player
            if (((1 << hit.collider.gameObject.layer) & playerMask) != 0)
                return true;
        }

        return false;
    }
}
