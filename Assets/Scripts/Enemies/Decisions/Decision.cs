using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for decisions used for AI state transitions
/// </summary>
public abstract class Decision : ScriptableObject {

    /// <summary>
    /// Performs some test on the enemy
    /// </summary>
    /// <param name="controller">Enemy performing test</param>
    /// <returns>Whether the test passed or failed</returns>
    public abstract bool Decide(EnemyController controller);
}
