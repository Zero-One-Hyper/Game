using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterState : MonoBehaviour
{
    public Image PlayerHealth;
    public Image PlayerEnergy;

    private bool StopEnergyRecover = false;
    private float StopRecoverTime = 1.0f;
    private float CalStopRecoverTime = 0;
    
    // Update is called once per frame
    void Update()
    {
        if(!StopEnergyRecover)
        {
            PlayerEnergy.fillAmount += CharacterDataDefine.EnergyRecover * Time.deltaTime;

            User.Instance.UserCharacter.characterState.CurrentEnergy = 
                User.Instance.UserCharacter.characterState.MaxEnergy * this.PlayerEnergy.fillAmount;

            if (User.Instance.UserCharacter.characterState.CurrentEnergy < 0)
                User.Instance.UserCharacter.characterState.CurrentEnergy = 0;
            if (User.Instance.UserCharacter.characterState.CurrentEnergy > User.Instance.UserCharacter.characterState.MaxEnergy)
                User.Instance.UserCharacter.characterState.CurrentEnergy = User.Instance.UserCharacter.characterState.MaxEnergy;
            
            //Debug.Log(User.Instance.UserCharacter.characterState.CurrentEnergy);
        }
        else
        {
            this.CalStopRecoverRecover();
        }
    }
    private void CalStopRecoverRecover()
    {
        if (this.CalStopRecoverTime >= this.StopRecoverTime)
        {
            this.StopEnergyRecover = false;
            this.CalStopRecoverTime = 0;
            return;
        }
        this.CalStopRecoverTime += Time.deltaTime;
    }
    public void EditHealth(float val)
    {
        //float temp = val / User.Instance.UserCharacter.characterState.MaxEnergy;
        PlayerHealth.fillAmount -= val / User.Instance.UserCharacter.characterState.MaxHealth;        
    }
    public void EditEnergy(float val)
    {
        //float temp = val / User.Instance.UserCharacter.characterState.MaxEnergy;
        PlayerEnergy.fillAmount -= val / User.Instance.UserCharacter.characterState.MaxEnergy;
        this.StopEnergyRecover = true;
        this.CalStopRecoverTime = 0;
    }
}
