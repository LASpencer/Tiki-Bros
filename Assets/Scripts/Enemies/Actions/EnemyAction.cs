﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAction : ScriptableObject {

    public abstract void Act(EnemyController controller);
}
