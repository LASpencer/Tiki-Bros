using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for AI behaviour states. These are assets, shared by all
/// enemies using the same state machine. A state is made up of the actions
/// which are performed while in the state, and the possible transitions out 
/// of the state
/// </summary>
[CreateAssetMenu (menuName = "EnemyAI/State")]
public class State : ScriptableObject {

    [Tooltip("Actions which will be taken every update")]
    public List<EnemyAction> actions;

    [Tooltip("Transitions from this state")]
    public List<Transition> transitions;
    public Color sceneGizmoColor = Color.gray;

    /// <summary>
    /// Performs all actions and checks if a transition is necessary
    /// </summary>
    /// <param name="controller">Enemy in this state</param>
    public void UpdateState(EnemyController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    /// <summary>
    /// Calls the Act method on each action in the state
    /// </summary>
    /// <param name="controller">Enemy performing the actions</param>
    private void DoActions(EnemyController controller)
    {
        foreach(EnemyAction action in actions)
        {
            action.Act(controller);
        }
    }

    /// <summary>
    /// Calls OnEnter method for each action in the state
    /// </summary>
    /// <param name="controller"> Enemy entering the state</param>
    public void OnEnter(EnemyController controller)
    {
        foreach(EnemyAction action in actions)
        {
            action.OnEnter(controller);
        }
    }

    /// <summary>
    /// Calls OnExit method for each action in the state
    /// </summary>
    /// <param name="controller">Enemy leaving this state</param>
    public void OnExit(EnemyController controller)
    {
        foreach(EnemyAction action in actions)
        {
            action.OnExit(controller);
        }
    }

    /// <summary>
    /// Tests each transition's decision. If the state resulting from that decision
    /// is not this state, transition to that other state
    /// </summary>
    /// <param name="controller">Enemy checking for new state</param>
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
                // Once a state that's not this is found, stop looking and transition to it
                controller.ChangeState(nextState);
                break;
            }
        }
    }
}

/// <summary>
/// Class representing a state transition. It contains a decision which is tested,
/// and the classes which should be transitioned to based on the test's result
/// </summary>
[System.Serializable]
public class Transition
{
    public Decision decision;
    public State TrueState;
    public State FalseState;
}