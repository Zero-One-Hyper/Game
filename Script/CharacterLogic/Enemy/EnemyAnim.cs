using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    public EnemyBase EnemyBase;
    private void Awake()
    {
        this.EnemyBase = this.GetComponentInParent<EnemyBase>();
    }
    public void FootL()
    {

    }
    public void FootR()
    {

    }
    public void Hit()
    {
        //this.EnemyBase.AttackCheseTarget();
    }
    public void AttackStart()
    {
        this.EnemyBase.AttackCheseTarget(true);
    }
    public void AttackEnd()
    {
        this.EnemyBase.AttackCheseTarget(false);
    }
}