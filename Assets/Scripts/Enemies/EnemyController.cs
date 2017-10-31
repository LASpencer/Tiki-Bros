using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour {

    public State CurrentState;
    public List<Transform> Waypoints;
    public Transform CurrentWaypoint
    {
        get { return Waypoints[waypointIndex]; }
    }
    public NavMeshAgent navAgent;
    [HideInInspector]
    public float TimeInState = 0;

    protected int waypointIndex;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        waypointIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
        TimeInState += Time.deltaTime;
        CurrentState.UpdateState(this);
	}

    public void ChangeState(State nextState)
    {
        if(CurrentState != nextState)
        {
            CurrentState = nextState;
            TimeInState = 0;
        }
    }

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
