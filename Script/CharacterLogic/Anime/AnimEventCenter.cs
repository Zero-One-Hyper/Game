using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventCenter : Singleton<AnimEventCenter> 
{

    public Dictionary<string, EventHandler> AnimEvents = new Dictionary<string, EventHandler>();


    public void AddListener(string name, EventHandler eventHandler)
    {
        if(!AnimEvents.ContainsKey(name))
        {
            AnimEvents.Add(name, eventHandler);
        }
        else
        {
            AnimEvents[name] += eventHandler;
        }
    }
    public void RemoveListener(string name, EventHandler eventHandler)
    {
        if(AnimEvents.ContainsKey(name))
        {
            AnimEvents[name] -= eventHandler;
        }
    }

    private bool HasEvent(string name)
    {
        return AnimEvents.ContainsKey(name);
    }
    public void TriggerEvent(string name, object sender, EventArgs e = null)
    {
        if (this.HasEvent(name))
        {
            AnimEvents[name].Invoke(sender, e);
        }
        else
        {
            Debug.LogWarning("Event name " + name + " is not exist");
        }
    }

}
