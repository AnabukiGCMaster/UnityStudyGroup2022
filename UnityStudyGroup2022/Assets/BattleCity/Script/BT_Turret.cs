using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Turret : MonoBehaviour
{
    public Transform m_Turret;
    public BT_Parameta  m_Parameta;
    public Transform m_Camera;
    private void Start()
    {
        m_Parameta= GetComponent<BT_Parameta>();
    }

    void Update()
    {
        if (m_Parameta)
            m_Turret.Rotate(new Vector3(0, Input.GetAxis(m_Parameta.m_TurretKeyName), 0));
    }
    private void OnDestroy()
    {
        if(m_Camera)
            m_Camera.parent = null;
    }
}
