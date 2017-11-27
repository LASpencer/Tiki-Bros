using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/State")]
public class State : ScriptableObject {

    [Tooltip("Actions which will be taken every update")]
    public List<EnemyAction> actions;

    [Tooltip("Transitions from this state")]
    public List<Transition> transitions;
    public Color sceneGizmoColor = Color.gray;

    public void UpdateState(EnemyController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(EnemyController controller)
    {
        foreach(EnemyAction action in actions)
        {
            action.Act(controller);
        }
    }

    public void OnEnter(EnemyController controller)
    {
        foreach(EnemyAction action in actions)
        {
            action.OnEnter(controller);
        }
    }

    public void OnExit(EnemyController controller)
    {
        foreach(EnemyAction action in actions)
        {
            action.OnExit(controller);
        }
    }

    private void CheckTransitions(EnemyController controller)
    {
        State nextState;
        foreach(Transition transition in transitions)
        {
            if (transition.decision.Decide(controller))
            {
                nextState = transition.TrueState;
            } else
            {
                nextState = transition.FalseState;
            }
            if(nextState != controller.CurrentState)
            {
                controller.ChangeState(nextState);
                break;
            }
        }
    }
}

[System.Serializable]
public class Transition
{
    public Decision decision;
    public State TrueState;
    public State FalseState;
}