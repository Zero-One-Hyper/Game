using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneManager : MonoSingleton<SceneManager>
{
    private string SceneName;
    protected override void OnStart()
    {

    }

    public void LoadScene(string sceneName)
    {
        this.SceneName = sceneName;
        StartCoroutine(OnLoadScene());
    }
    IEnumerator OnLoadScene()
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        
        while (!asyncLoad.isDone)
        {
            Debug.Log("LoadingScenen" + SceneName);
            yield return null;
        }
        
    }

    public void OnApplicationQuit()
    {
        if(NetClient.Instance.IsConnected)
        {
            MapService.Instance.SendCharaExit(); 
        }
    }
}
