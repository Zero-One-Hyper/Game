using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Action<int> OnEnemyDead;
    public Action<int> OnPlayerDead;
    public bool GameEnd = false;

    public void Init()
    {
        this.OnPlayerDead += PlayerDead;
        this.GameEnd = false;
    }
    public void PlayerDead(int playerID)
    {
        this.GameEnd = true;
        if(playerID == User.Instance.UserCharacter.CharacterId)
        {            
            UImainCanvas.Instance.OnFail();
        }
    }
}
