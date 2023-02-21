using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILockCenter : MonoBehaviour
{
    public Image LockImageCenter;
    private Transform LockTarget;
    // Start is called before the first frame update
    void Start()
    {
        if(LockTarget == null)
            this.LockImageCenter.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        if (LockTarget == null)
            return;
        //Vector3 pos = Camera.main.WorldToScreenPoint(LockTarget.transform.position);
        SetPosition(Camera.main.WorldToScreenPoint(LockTarget.transform.position));
        //Debug.Log(pos);
    }
    public void SetLockTarget(Transform lockTarget)
    {
        this.LockImageCenter.gameObject.SetActive(true);
        this.LockTarget = lockTarget;
    }
    public void UnLockTarget()
    {
        this.LockImageCenter.gameObject.SetActive(false);
        this.LockTarget = null;
    }
    void SetPosition(Vector3 pos)
    {
        LockImageCenter.rectTransform.position = pos;
    }
}
