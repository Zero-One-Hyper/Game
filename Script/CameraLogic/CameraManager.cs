using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [Header("完整测试 相机位置为生成角色的位置")]
    public Transform CameraFollow;
    public Transform CameraAim;

    [Header("由此场景开始 相机位置为测试位置")]
    public Transform testCameraFollow;
    public Transform testCameraAim;

    [Header("Enemy瞄准位置")]
    public Transform CameraEnemyAim;
        

    protected override void OnStart()
    {
        
    }
    public void Init()
    {
    
    }
    //测试
    public void InitAim()
    {
        this.CameraAim.SetParent(testCameraAim);
        this.CameraAim.localPosition = Vector3.zero;
        this.CameraFollow.SetParent(testCameraFollow);
        this.CameraFollow.localPosition = Vector3.zero;
    }

    public void InitAim(Transform aim, Transform foll)
    {
        this.CameraAim.SetParent(aim);
        this.CameraAim.localPosition = Vector3.zero;
        this.CameraFollow.SetParent(foll);
        this.CameraFollow.localPosition = Vector3.zero;
    }

    internal void SetLockTarget(Transform enemyAimPoint)
    {
        this.CameraEnemyAim.SetParent(enemyAimPoint);
        BattleManager.Instance.OnCharaLock(enemyAimPoint);
        this.CameraEnemyAim.localPosition = Vector3.zero;
    }
}
