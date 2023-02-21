using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [Header("�������� ���λ��Ϊ���ɽ�ɫ��λ��")]
    public Transform CameraFollow;
    public Transform CameraAim;

    [Header("�ɴ˳�����ʼ ���λ��Ϊ����λ��")]
    public Transform testCameraFollow;
    public Transform testCameraAim;

    [Header("Enemy��׼λ��")]
    public Transform CameraEnemyAim;
        

    protected override void OnStart()
    {
        
    }
    public void Init()
    {
    
    }
    //����
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
