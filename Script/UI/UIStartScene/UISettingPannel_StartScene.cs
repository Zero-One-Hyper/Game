using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISettingPannel_StartScene : MonoBehaviour
{
    private float PlayInterVal = 0.3f;
    private float LastPlaySoundTime = 0;

    public Slider allSoundSlider;
    public Slider musicSlider;
    public Slider soundSlider;
    public Slider environmentSlider;

    private void Start()
    {
        allSoundSlider.value = SoundManager.Instance.AllVolume;
        musicSlider.value = SoundManager.Instance.MusicVolume;
        soundSlider.value = SoundManager.Instance.SoundVolume;
        environmentSlider.value = SoundManager.Instance.EnvSoundVolume;
    }

    private void OnEnable() 
    {
        this.LastPlaySoundTime = Time.realtimeSinceStartup;
        UIInput.Instance.SelectObject = this.allSoundSlider.gameObject;
    }

    public void OnClickExit()
    {
        UIInput.Instance.CloseUI();
    }
    public void ExitClick()
    {
        UIMain_StartScene.Instance.ButtonPannelMask.SetActive(false);
        SoundManager.Instance.PlaySound(SoundDataDefine.UIButtonSwitch_01);
        this.gameObject.SetActive(false);
    }
    public void OnAllSoundSliderChange(float val)
    {
        SoundManager.Instance.SetAllSoundVolem("MainAudioVolume", val);
        if (Time.realtimeSinceStartup - LastPlaySoundTime > PlayInterVal)
        {
            SoundManager.Instance.PlayMasterSound(SoundDataDefine.UIErrorSound);
            LastPlaySoundTime = Time.realtimeSinceStartup;
        }
    }
    public void OnSoundSliderChanged(float val)
    {
        SoundManager.Instance.SetSoundVolem("Sound", val);
        if (Time.realtimeSinceStartup - LastPlaySoundTime > PlayInterVal)
        {
            SoundManager.Instance.PlaySound(SoundDataDefine.UIErrorSound);
            LastPlaySoundTime = Time.realtimeSinceStartup;
        }
    }
    public void OnMusicSliderChanged(float val)
    {
        SoundManager.Instance.SetMusicVolume("Music", val);
        if (Time.realtimeSinceStartup - LastPlaySoundTime > PlayInterVal)
        {
            SoundManager.Instance.PlayMusicSound(SoundDataDefine.UIErrorSound);
            LastPlaySoundTime = Time.realtimeSinceStartup;
        }
    }
    public void OnEnvSoundSliderChanged(float val)
    {
        SoundManager.Instance.SetEnvSoundVolem("Environment", val);
        if (Time.realtimeSinceStartup - LastPlaySoundTime > PlayInterVal)
        {
            SoundManager.Instance.PlayEnvironmenmtSound(SoundDataDefine.UIErrorSound);
            LastPlaySoundTime = Time.realtimeSinceStartup;
        }
    }

}
