using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public Dictionary<int, Enemy> Enemies = new Dictionary<int, Enemy>();
    public Action<ControlType> SwitchEnemyControlType;
    public int AliveEnemieCount = 0;

    public Dictionary<string, EventHandler> EnemyEvent = new Dictionary<string, EventHandler>();
    public void Init()
    {
        this.Enemies.Clear();
        this.EnemyEvent.Clear();
        this.AliveEnemieCount = 0;
        GameManager.Instance.OnEnemyDead += this.OnEnemyDead;
    }
    public void AddEnemyEntity(Enemy enemy)
    {
        this.Enemies[enemy.EnemyID] = enemy;
        this.AliveEnemieCount++;
    }
    public void RemoveEnemyEntity(Enemy entity)
    {
        if (this.Enemies.ContainsKey(entity.EnemyID))
        {
            this.Enemies.Remove(entity.EnemyID);
        }
    }
    public Enemy GetEnemy(int enemyId)
    {
        if(this.Enemies.TryGetValue(enemyId, out Enemy enemy)) 
            return enemy;
        return null;
    }
    public void UploadEnemyInfo()
    {

    }
    public void OnEnemyEntitySync(EnemyEntityRequest EnemySyncMsg)
    {
        if (Enemies.TryGetValue(EnemySyncMsg.EnemyId, out Enemy entity))
        {
            //先处理位置
            entity.NPosition = EnemySyncMsg.NPosition;
            entity.NDirection = EnemySyncMsg.NDirection;

            //再处理动画
            if (entity.AnimEventArg == null) entity.AnimEventArg = new EnemyAnimeEvent();

            ((EnemyAnimeEvent)entity.AnimEventArg).ActionType = EnemySyncMsg.ActionType;
        }
    }

    internal void EnemyAttack(int enemyID)
    {
        if(this.Enemies.TryGetValue(enemyID, out Enemy enemy))
        {
            enemy.EnemyBase.EnemyController.PlayAnim(AnimActionType.Attack);
        }
    }
    public void SendEnemyAttack(int enemyID)
    {
        EnemyService.Instance.SendAttack(enemyID);
    }
    public void AddEventListener(string name, EventHandler eventHandler)
    {
        this.EnemyEvent.Add(name, eventHandler);
    }
    public void TriggerEvent(string name, object sender, EventArgs e = null)
    {
        if(this.EnemyEvent.TryGetValue(name, out EventHandler eventHandler))
        {
            eventHandler.Invoke(sender, e);
        }
        else
        {
            Debug.LogWarning("Event name " + name + " is not exist");
        }
    }
    public void OnEnemyDead(int enemyID)
    {
        if(this.Enemies.TryGetValue(enemyID, out Enemy enemy))
        {
            this.Enemies.Remove(enemyID);
            this.AliveEnemieCount--;
        }
        if(this.AliveEnemieCount <= 0)
        {
            GameManager.Instance.GameEnd = true;
            UImainCanvas.Instance.OnWin();
        }

    }
}
