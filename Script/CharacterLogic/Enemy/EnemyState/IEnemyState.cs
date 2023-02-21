using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyStates 
{
    public void OnEnterState();
    public void OnUpdateState(float deltaTime);
    public void OnFixedUpdateState(float fixedDeltaTime);
    public void OnExitState();
}
