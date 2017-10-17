using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public State CurrentState;
    [HideInInspector]
    float TimeInState = 0;

	// Use this for initialization
	void Start () {
		
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
}
