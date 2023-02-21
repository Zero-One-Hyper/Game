using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : Singleton<User>
{
    private int playerId;
    public int PlayerId
    {
        get
        {
            return playerId;
        }
    }
    public int CharacterTypeID;
    
    public Character UserCharacter;

    public bool inRoom;
    public void Init(int id)
    {
        this.playerId = id;
    }

}
