using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool DoNotDestoryOnLoad;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = (T)FindObjectOfType<T>();
            if(instance == null)
            {
                if (typeof(T) == typeof(InputManager))
                {
                    SceneLogic sc = (SceneLogic)FindObjectOfType(typeof(SceneLogic));
                    instance = sc.transform.AddComponent<T>();
                }
                else if(typeof(T) == typeof(UIInput))
                {
                    SceneLogic sc = (SceneLogic)FindObjectOfType(typeof(SceneLogic));
                    instance = sc.transform.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this.DoNotDestoryOnLoad)
        {
            if (instance != null && instance != this.gameObject.GetComponent<T>())
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            instance = this.gameObject.GetComponent<T>();
        }
        this.OnStart();
        //Debug.Log(114514);
    }
    protected virtual void OnStart()
    {

    }
}
