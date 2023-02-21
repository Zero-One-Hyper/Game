using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlType
{
    None = 0,
    NET = 1,
    LOCAL = 2,
}
public struct EnemyState
{
    public float MaxHealth;
    public float CurrentHealth;
}
public class EnemyEventArg : EventArgs
{
    public int EnemyID;
}
public class Enemy : Entity
{
    public EnemyEventArg EnemyEventArg;
    public int EnemyID;

    public EnemyBase EnemyBase;
    public GameObject EnemyGameObject;
    public float AttackDamage = 16.0f;
    public EnemyState EnemyState = new EnemyState()
    {
        MaxHealth = 100,
        CurrentHealth = 100,
    };
    public Enemy(EnemyBase enemyBase, int enemyID)
    {
        this.EnemyBase = enemyBase;
        this.EnemyID = enemyID;
        this.EnemyEventArg = new EnemyEventArg()
        {
            EnemyID = this.EnemyID,
        };
    }
    public NEnemyInfo NEnemyInfo
    {
        get
        {
            this.EntityHealth = this.EnemyState.CurrentHealth;
            NEnemyInfo info = new NEnemyInfo()
            {
                EnemyID = this.EnemyID,
                CurrentPosition = this.NPosition,
                CurrentDirection = this.NDirection,
                EnemyHealth = this.NEntityHealth,
            };
            return info;
        }
        set
        {
            this.EnemyID = value.EnemyID;
            this.NPosition = value.CurrentPosition;
            this.NDirection = value.CurrentDirection;
            this.NEntityHealth = value.EnemyHealth;
            this.EnemyState.CurrentHealth = this.EntityHealth;
        }
    }
    public void EnemyInit(NEnemyInfo nEnemyInfo)
    {
        this.NEnemyInfo = nEnemyInfo;
        EnemyBase.Set(VectorTool.NVectorToVector(nEnemyInfo.CurrentPosition), 
            VectorTool.NVectorToVector(NEnemyInfo.CurrentDirection));
    }
    public void EnemyAttack()
    {
        this.EnemyBase.EnemyController.EnemyAttack();
    }

    internal void GetHit(int damage)
    {
        this.EnemyBase.GetHit(damage / 100.0f);
    }
}
