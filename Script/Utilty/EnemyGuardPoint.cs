using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class EnemyGuardPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position + Vector3.up * 2.0f, 0.05f);
        Gizmos.DrawLine(this.transform.position + Vector3.up * 2.0f,
            this.transform.position + Vector3.up * 2.0f + this.transform.forward);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(this.transform.position, 0.1f);
    }
}
#endif
