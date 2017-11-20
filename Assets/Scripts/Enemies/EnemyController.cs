using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour {

    [Tooltip("Current AI State")]
    public State CurrentState;

    [Tooltip("Patrol waypoints, in order they will be reached")]
    public List<Transform> Waypoints;

    // Current destination waypoint
    public Transform CurrentWaypoint
    {
        get { return Waypoints[waypointIndex]; }
    }
    public NavMeshAgent navAgent;

    [Tooltip("Angle of cone enemy can see in")]
    public float VisionAngle;

    [Tooltip("Distance enemy can see")]
    public float VisionRange;

    [Tooltip("Distance at which enemy give up chasing player")]
    public float EscapeRange;

    [HideInInspector]
    public float TimeInState = 0;

    [HideInInspector]
    public PlayerController player;

    protected int waypointIndex;

	Animator animator;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>();
        waypointIndex = 0;
		animator = GetComponent<Animator> ();
    }
	
	// Update is called once per frame
	void Update () {
        TimeInState += Time.deltaTime;
        CurrentState.UpdateState(this);
		if (animator != null) {
			//if (animator.get
				animator.SetFloat ("MovementSpeed", navAgent.velocity.magnitude);
		}
	}

    public void ChangeState(State nextState)
    {
        if(CurrentState != nextState)
        {
            CurrentState = nextState;
            TimeInState = 0;
        }
    }

    // Increment waypoints, returning to 0 when end reached
    public Transform NextWaypoint()
    {
        waypointIndex++;
        if(waypointIndex >= Waypoints.Count)
        {
            waypointIndex = 0;
        }
        return Waypoints[waypointIndex];
    }
}
