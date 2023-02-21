using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    internal void OnCharaAttack(int playerAttackID)
    {
        CharacterManager.Instance.OnCharacterAttack(playerAttackID);
    }

    public void OnCharaRoll(int playerRollID)
    {
        CharacterManager.Instance.OnCharacterRoll(playerRollID);
    }
    public void OnCharaLock(Transform go)
    {
        UIBattle.Instance.LockTarget(go);
    }
    public void OnCharUnLock()
    {
        UIBattle.Instance.UnLockTarget();
    }
}
