using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIInput : MonoSingleton<UIInput>, InputSetting.IUIActions
{
    //[SerializeField]
    private EventSystem Eventsystem;
    private EventSystem eventsystem
    {
        get
        {
            if(this.Eventsystem == null)
                this.Eventsystem = FindObjectOfType<EventSystem>();
            return this.Eventsystem;
        }
    }
    [SerializeField]
    private GameObject CurrentActiveUI;
    public int ccc;

    public Stack<GameObject> UIStartActiveUI = new Stack<GameObject>();
    //[SerializeField]
    private GameObject selectObject;
    public GameObject SelectObject
    {
        get { return this.selectObject; }
        set
        {
            this.selectObject = value;
            this.eventsystem.SetSelectedGameObject(value);
        }
    }
    private Action CloseCallBack;
    protected override void OnStart()
    {
        this.selectObject = this.eventsystem.firstSelectedGameObject;
    }
   
    public void Init()
    {
        this.InitUIInput();
    }
    public void InitUIInput()
    {
        InputManager.Instance.Init();
        InputManager.Instance.SetCallBack(this, InputType.UI);
    }

    public void SetSelectedUI(GameObject activeUI, Action CloseCallBack)
    {
        UIStartActiveUI.Push(eventsystem.currentSelectedGameObject);
        
        activeUI.SetActive(true);
        this.CloseCallBack = CloseCallBack;
        this.CurrentActiveUI = activeUI;
    }
    public void SetSelectedPopUI(GameObject gameObject, Action CloseCallBack)
    {
        UIStartActiveUI.Push(eventsystem.currentSelectedGameObject);

        this.CloseCallBack = CloseCallBack;
        this.CurrentActiveUI = gameObject;
    }
    public void CloseUI()
    {
        if (this.CurrentActiveUI != null)
        {
            this.CloseCallBack?.Invoke();
            this.CurrentActiveUI = null;
            this.eventsystem.SetSelectedGameObject(this.UIStartActiveUI.Pop());
        }
    }

    public void OnCloseCurrentUI(InputAction.CallbackContext context)
    {
        if (context.control.IsPressed())
        {
            this.CloseUI();
        }
    }
    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (context.control.IsPressed())
        {
            if (TestBaseLogic.isPlaying)
            {
                Debug.Log("OpenBag");
                InputManager.Instance.DisAbleInputSetting(InputType.Player);
                UImainCanvas.Instance.OpenMenu();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.control.IsPressed())
        {
            if (this.eventsystem.currentSelectedGameObject == null)
                this.eventsystem.SetSelectedGameObject(this.selectObject);
        }
    }
    public void OnPressSpeace_TItle(InputAction.CallbackContext context)
    {
        if(context.control.IsPressed())
        {
            if(UIMain_StartScene.Instance != null)
            {
                UIMain_StartScene.Instance.StopShowStartTitle();
                return;
            }
            if (!GameManager.Instance.GameEnd)
                return;
            if (UImainCanvas.Instance != null)
            {

                InputManager.Instance.DisAbleInputSetting(InputType.Player);
                SceneManager.Instance.LoadScene("SceneStart");
            }
        }
    }
    public void OnCancel(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }



    public void OnPoint(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

}
