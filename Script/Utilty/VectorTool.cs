using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class VectorTool
{
    public static Vector3 NVectorToVector(NVector3 nVector)
    {
        return new Vector3(nVector.X / 100.0f, nVector.Y / 100.0f, nVector.Z / 100.0f);
    }

    public static NVector3 VectorToNVector(Vector3 vector)
    {
        return new NVector3()
        {
            X = (int)(vector.x * 100), 
            Y = (int)(vector.y * 100), 
            Z = (int)(vector.z * 100) 
        };
    }
}
