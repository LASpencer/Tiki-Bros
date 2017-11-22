﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour {

    public int Health = 1;

    // Set to true on frame when hit by player
    public bool Hit = false;

    [HideInInspector]
    public bool Invincible = false;

    [HideInInspector]
    public bool AttackActivated = true;

    [HideInInspector]
    public Vector3 KnockbackDirection;

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

    [Tooltip("Time spent in Hurt state")]
    public float RecoveryTime;

    [Tooltip("Speed while being knocked away from player")]
    public float KnockbackSpeed;

    [Tooltip("Time spent in Death state")]
    public float DeathTime = 1;

    [Tooltip("Time for particle emitter on death to exist")]
    public float DeathParticleTime = 2;

    public GameObject DeathEffect;

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
        CurrentState.OnEnter(this);
    }
	
	// Update is called once per frame
	void Update () {
        TimeInState += Time.deltaTime;
        CurrentState.UpdateState(this);
		if (animator != null) {
			//if (animator.get
				animator.SetFloat ("MovementSpeed", navAgent.velocity.magnitude);
		}
        Hit = false;
	}

    public void ChangeState(State nextState)
    {
        if(CurrentState != nextState)
        {
            CurrentState.OnExit(this);
            nextState.OnEnter(this);
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

    public void Damage(int damage = 1)
    {
        //HACK make up states for recovery, dying
        // TODO have invincibility frames
        if (!Invincible)
        {
            Debug.Log("Enemy punched");
            Health -= damage;
            Hit = true;
            animator.SetTrigger("hasBeenHit");
        }

    }

    public void Die()
    {
        //TODO particle effects to cover up disappearance
        // Spawn particle emitter prefab, and destroy after DeathParticleTime
        navAgent.isStopped = true;
        GameObject particles = Instantiate(DeathEffect, transform.position, transform.rotation);
        Destroy(particles, DeathParticleTime);
    }

    public void Attack()
    {
        // Do all animation etc changes
        animator.SetTrigger("hasPunched");
    }
}
