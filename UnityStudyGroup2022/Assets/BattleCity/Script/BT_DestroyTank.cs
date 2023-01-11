using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BT_DestroyTank : MonoBehaviour
{
    public bool m_DestroyFlag = false;
    void Start()
    {
        foreach (Transform Dummy in this.transform)
        {
            Dummy.parent = null;
            Dummy.AddComponent<Rigidbody>();
            Destroy(Dummy.gameObject, 3.0f);
        }
        m_DestroyFlag = true;
    }
    private void Update()
    {
        if(m_DestroyFlag)
            Destroy(this.gameObject);
    }
}
