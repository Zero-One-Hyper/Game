using Common;
using Protocol;
using System;
using UnityEngine;

internal class EnemyService : Singleton<EnemyService>, IDisposable
{
    public EnemyService() 
    {
        MessageDistributer.Instance.Subscribe<EnemyEntityResponse>(this.OnEnemyEntityResponse);    
        MessageDistributer.Instance.Subscribe<EnemyAttackMessage>(this.OnEnemyAttack);    
        MessageDistributer.Instance.Subscribe<EnemyGetHit>(this.OnEnemyGetHit);    
    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<EnemyEntityResponse>(this.OnEnemyEntityResponse);    
        MessageDistributer.Instance.Unsubscribe<EnemyAttackMessage>(this.OnEnemyAttack);    
        MessageDistributer.Instance.Unsubscribe<EnemyGetHit>(this.OnEnemyGetHit);    
    }
    public void Init()
    {

    }
    private void OnEnemyEntityResponse(object sender, EnemyEntityResponse message)
    {
        Debug.LogFormat("[{0}] Enemy Update", message.EnemyEntityResponses.Count);
        Enemy enemy;
        foreach(var msg in message.EnemyEntityResponses)
        {
            Debug.LogFormat("Enemy [{0}] Update!!", msg.EnemyId);
            enemy = EnemyManager.Instance.GetEnemy(msg.EnemyId);
            if (enemy == null) continue;

            enemy.NEnemyInfo.EnemyID = msg.EnemyId;
            EnemyManager.Instance.OnEnemyEntitySync(msg);
        }
    }
    internal void SendEnemyEntitySync(EnemyEntityRequest enemyEntityRequest)
    {
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.EnemyEntityRequest = enemyEntityRequest;

        NetClient.Instance?.SendMessage(message);
    }
    private void OnEnemyAttack(object sender, EnemyAttackMessage message)
    {
        EnemyManager.Instance.EnemyAttack(message.EnemyID);
    }

    internal void SendAttack(int enemyID)
    {
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.EnemyAttackMsg = new EnemyAttackMessage()
        {
            EnemyID = enemyID
        };
        NetClient.Instance?.SendMessage(message);
    }
    private void OnEnemyGetHit(object sender, EnemyGetHit message)
    {
        Enemy enemy = EnemyManager.Instance.GetEnemy(message.EnemyID);
        enemy.GetHit(message.Damage);
    }
    public void SendEnemyGetHit(int enemyID, int Damage)
    {
        Debug.Log("");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.EnemyGetHitRequest = new EnemyGetHit()
        { 
            EnemyID = enemyID,
            Damage = Damage, 
        };
        NetClient.Instance?.SendMessage(message);
    }
}

