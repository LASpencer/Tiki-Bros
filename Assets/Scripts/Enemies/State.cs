using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/State")]
public class State : ScriptableObject {

    public List<Action> actions;
    public List<Transition> transitions;
    public Color sceneGizmoColor = Color.gray;

    public void UpdateState(EnemyController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(EnemyController controller)
    {
        foreach(Action action in actions)
        {
            action.Act(controller);
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