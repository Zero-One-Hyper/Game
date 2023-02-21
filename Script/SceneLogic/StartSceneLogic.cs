using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneLogic : SceneLogic//, MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        UIInput.Instance.Init();
        InputManager.Instance.EnableInputSetting(InputType.UI);
        if(!PlayerPrefs.HasKey("FirstPlay"))
        {
            PlayerPrefs.SetInt("FirstPlay", 1);
        }
        //不是第一次进入游戏
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
