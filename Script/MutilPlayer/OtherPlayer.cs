using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    public int CooperatorID;
    public Vector3 SummonPosition;
    public Transform UIPosition;
    public GameObject UISummonPlayerPrefab;
    private UISummonPlayer UISummonPlayer;

    public void Init(int Id, Vector3 pos)
    {
        this.CooperatorID = Id;
        this.SummonPosition = pos;
        CharacterManager.Instance.OnChareacterEnterCallBack += this.OnSummonDown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(this.UISummonPlayer == null)
            {
                GameObject go = Instantiate(UISummonPlayerPrefab);
                this.UISummonPlayer = go.GetComponent<UISummonPlayer>();
                this.UISummonPlayer.Init(this.UIPosition, this.CooperatorID, this.SummonPosition);
            }
            this.UISummonPlayer.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        this.UISummonPlayer.OnClickClose();
    }
    private void OnSummonDown()
    {
        this.gameObject.SetActive(false);
        this.UISummonPlayer.gameObject.SetActive(false);
    }
}
