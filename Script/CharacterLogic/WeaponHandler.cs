using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    None,
    Player,
    Enemy,
}
public class WeaponHandler : MonoBehaviour
{
    public WeaponType type;
    public EnemyBase EnemyUse;
    public CharacterBase CharacterUse;
    public bool isDead = false;
    public bool canDoHit = false;
    private void Start()
    {
        AnimEventCenter.Instance.AddListener("StopAttack", this.OnHit);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (canDoHit  && !isDead)
        {
            switch (type)
            {
                case WeaponType.None:
                    break;
                case WeaponType.Enemy:
                    CharacterBase cb = other.gameObject.GetComponent<CharacterBase>();
                    if(cb == null )
                        return;                    
                    cb.GetHit(EnemyUse.enemy.AttackDamage);
                    BattleService.Instance.SendCharacterHit(cb.character.CharacterId, (int)(EnemyUse.enemy.AttackDamage * 100));
                    if(cb.character.characterState.CurrentHealth <= 0)
                    {
                        this.EnemyUse.EnemyStateMechine.ChaseTarget = null;
                        this.EnemyUse.EnemyStateMechine.SwitchStates(this.EnemyUse.EnemyStateMechine.defaultStates);
                    }
                    break;
                case WeaponType.Player:
                    EnemyBase eb = other.gameObject.GetComponent<EnemyBase>();
                    if (eb == null)
                        return;
                    eb.GetHit(CharacterUse.character.AttackDamage);
                    EnemyService.Instance.SendEnemyGetHit(eb.enemy.EnemyID, (int)(CharacterUse.character.AttackDamage * 100));
                    break;
            }
        }
    }
    public void SwitchAttack(bool canAttack)
    {
        this.canDoHit = canAttack;
    }
    private void OnHit(object sender, EventArgs e)
    {
        this.canDoHit = false;
    }
    public void OnEnemyDead()
    {
        this.canDoHit = false;
        this.isDead = true;
    }
}
