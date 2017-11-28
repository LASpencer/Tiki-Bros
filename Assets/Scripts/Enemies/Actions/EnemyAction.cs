using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for enemy AI actions, which can be executed by State assets
/// </summary>
public abstract class EnemyAction : ScriptableObject {

    /// <summary>
    /// Called when state containing this action is entered
    /// </summary>
    /// <param name="controller">Enemy entering the state</param>
    public abstract void OnEnter(EnemyController controller);

    /// <summary>
    /// Called when state containing this action is exited
    /// </summary>
    /// <param name="controller">Enemy exiting the state</param>
    public abstract void OnExit(EnemyController controller);

    /// <summary>
    /// Behaviour to perform every update
    /// </summary>
    /// <param name="controller">Enemy performing action</param>
    public abstract void Act(EnemyController controller);
}
