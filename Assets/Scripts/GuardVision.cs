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
        Vector3 origin = eye.position;
        Vector3 target = player.position + Vector3.up * 1.2f;

        Vector3 toPlayer = target - origin;
        float dist = toPlayer.magnitude;

        if (dist > viewDistance)
            return false;

        Vector3 dir = toPlayer.normalized;

        Debug.DrawRay(origin, eye.forward * viewDistance, Color.green);
        Debug.DrawRay(origin, dir * dist, Color.red);

        // Close-range forgiveness
        if (dist < 2f)
        {
            if (Physics.Raycast(origin, dir, out RaycastHit closeHit, dist + 0.25f))
            {
                if (closeHit.transform == player || closeHit.transform.IsChildOf(player))
                    return true;
            }
        }

        float halfAngle = viewAngle * 0.5f;
        float angleToPlayer = Vector3.Angle(eye.forward, dir);

        if (angleToPlayer > halfAngle + 5f)
            return false;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, dist + 0.25f))
        {
            if (hit.transform == player || hit.transform.IsChildOf(player))
                return true;
        }

        return false;
    }
}
