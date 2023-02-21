using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class UISummonPlayer : MonoBehaviour
{
    public int CooperatorID;
    public Vector3 CooperatorPosition;
    public GameObject FirstSelected;
    public UIWorldCanvas UIWorldCanvas;
    internal void Init(Transform transform, int Id, Vector3 Pos)
    {
        this.CooperatorID = Id;
        this.CooperatorPosition = Pos;
        if (this.UIWorldCanvas == null)
        {
            this.UIWorldCanvas = FindObjectOfType<UIWorldCanvas>();
        }
        this.transform.SetParent(this.UIWorldCanvas.transform);
        this.transform.position = transform.position;
    }
    private void OnEnable()
    {
        UIInput.Instance.SetSelectedUI(this.gameObject, this.CloseUI);
        UIInput.Instance.SelectObject = this.FirstSelected;
    }
    private void LateUpdate()
    {
        //this.transform.position = transform.position;
        this.transform.forward = Camera.main.transform.forward;
    }
    public void OnClickSummonPlayer()
    {
        MapService.Instance.SendSummonRequest(
            this.CooperatorID, 
            this.CooperatorPosition, 
            Vector3.ProjectOnPlane(this.transform.forward, Vector3.up));  
    }
    public void OnClickClose()
    {
        UIInput.Instance.CloseUI();
    }
    private void CloseUI()
    {
        this.gameObject.SetActive(false);
    }

}
