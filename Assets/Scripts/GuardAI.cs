using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardAI : MonoBehaviour
{
    private enum State {Patrol, Chase, ReturnToPatrol}

    private GuardVision vision;
    public Transform player;
    public Transform[] waypoints;
    // patrol
    public float arriveDistance = 0.4f;
    public float waitAtPointSeconds = 1.0f;
    // speeds
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3f;
    public float returnSpeed = 2.5f;
    // chase
    public float lostSightGraceSeconds = 1.5f;
    public float catchDistance = 1.5f;


    private NavMeshAgent agent;
    private State state;

    private int waypointIndex;
    private float waitTimer;

    private float lostSightTimer;
    private Vector3 lastSeenPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponent<GuardVision>();
        state = State.Patrol;

        waypointIndex = 0;
        SetSpeed(patrolSpeed);
        GoToWaypoint(waypointIndex);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= catchDistance)
        {
            GameManager.Instance.LoseGame();
            return;
        }

        bool seesPlayer = vision.CanSeePlayer();
        if (seesPlayer)
        {
            lastSeenPosition = player.position;
        }

        switch (state)
        {
            case State.Patrol:
                UpdatePatrol(seesPlayer);
                break;

            case State.Chase:
                UpdateChase(seesPlayer);
                break;

            case State.ReturnToPatrol:
                UpdateReturnToPatrol(seesPlayer);
                break;
        }
    }

    private void UpdatePatrol(bool seesPlayer)
    {
        if (seesPlayer)
        {
            EnterChase();
            return;
        }

        if (agent.remainingDistance <= arriveDistance)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitAtPointSeconds)
            {
                waitTimer = 0f;
                AdvanceWaypoint();
                GoToWaypoint(waypointIndex);
            }
        }
    }

    private void UpdateChase(bool seesPlayer)
    {
        if (seesPlayer)
        {
            lostSightTimer = 0f;
        }
        else
        {
            lostSightTimer += Time.deltaTime;
            if (lostSightTimer >= lostSightGraceSeconds)
            {
                EnterReturnToPatrol();
                return;
            }
        }

        agent.SetDestination(player.position);
    }

    private void UpdateReturnToPatrol(bool seesPlayer)
    {
        if (seesPlayer)
        {
            EnterChase();
            return;
        }

        if (agent.remainingDistance <= arriveDistance)
        {
            // Once we're back on patrol path, continue normal patrol
            state = State.Patrol;
            SetSpeed(patrolSpeed);

            GoToWaypoint(waypointIndex);
        }
    }

    private void EnterChase()
    {
        state = State.Chase;
        SetSpeed(chaseSpeed);

        lostSightTimer = 0f;
        agent.SetDestination(player.position);
    }

    private void EnterReturnToPatrol()
    {
        state = State.ReturnToPatrol;
        SetSpeed(returnSpeed);

        GoToWaypoint(waypointIndex);
    }

    private void GoToWaypoint(int idx)
    {
        agent.SetDestination(waypoints[idx].position);
    }

    private void AdvanceWaypoint()
    {
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
            waypointIndex = 0;
           
    }

    private void SetSpeed(float speed)
    {
        agent.speed = speed;
    }
}