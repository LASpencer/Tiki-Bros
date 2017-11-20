using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAction : ScriptableObject {

    public abstract void OnEnter(EnemyController controller);

    public abstract void OnExit(EnemyController controller);

    public abstract void Act(EnemyController controller);
}
