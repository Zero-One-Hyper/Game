using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class Entity
{
    public int EntityID;

    public float EntityHealth;
    public int NEntityHealth
    {
        get
        {
            return (int)(EntityHealth * 100.0f);
        }
        set 
        { 
            EntityHealth = value / 100.0f; 
        }
    }

    public Vector3 Position;
    public Vector3 Direction;

    //Enter时的位置
    public NVector3 CallPosition;
    public NVector3 CallDirection;

    //上一个位置
    public Vector3 LastPosition;
    public Vector3 LastDirection;

    private NVector3 nposition = new NVector3();
    public NVector3 NPosition
    {
        get
        {
            nposition.X = (int)(this.Position.x * 100);
            nposition.Y = (int)(this.Position.y * 100);
            nposition.Z = (int)(this.Position.z * 100);
            return nposition;
        }
        set
        {
            this.Position.x = value.X / 100.0f;
            this.Position.y = value.Y / 100.0f;
            this.Position.z = value.Z / 100.0f;
        }
    }
    private NVector3 ndirection = new NVector3();
    public NVector3 NDirection
    {
        get
        {
            ndirection.X = (int)(this.Direction.x * 100);
            ndirection.Y = (int)(this.Direction.y * 100);
            ndirection.Z = (int)(this.Direction.z * 100);
            return ndirection;
        }
        set
        {
            this.Direction.x = value.X / 100.0f;
            this.Direction.y = value.Y / 100.0f;
            this.Direction.z = value.Z / 100.0f;
        }
    }

    public EventArgs AnimEventArg;

    public void EntityUpdate(Vector3 position, Vector3 direction)
    {
        this.LastPosition = this.Position;
        this.LastDirection = this.Direction;

        this.Position = position;
        this.Direction = direction;
        //Debug.Log(this.Position);
    }
    public bool CanPosDirUpdate()
    {
        if (Vector3.Distance(this.Position, this.LastPosition) > 0.005f)
            return true;
        //Debug.Log(Vector3.Distance(this.Position, this.LastPosition));
        if (Mathf.Acos(Vector3.Dot(this.LastDirection.normalized, this.Direction.normalized)) * Mathf.Rad2Deg > 5.0f)
            return true;
        return false;
    }
}
