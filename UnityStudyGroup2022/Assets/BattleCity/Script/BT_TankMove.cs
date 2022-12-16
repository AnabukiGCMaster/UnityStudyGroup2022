using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_TankMove : MonoBehaviour
{
    [Header("パラメーターリンク")]
    public BT_Parameta m_Pamarata;
    [Header("移動力")]
    public float m_MovePower = 2.0f;
    void Start()
    {
        //パラメーターリンク
        m_Pamarata = GetComponent<BT_Parameta>();
    }

    void Update()
    {
        //パッドの上下キーでベロシティで車体を前後退させる
        GetComponent<Rigidbody>().velocity =
            -this.transform.forward * (Input.GetAxis(m_Pamarata.m_UDKeyName) * m_MovePower) +
            this.transform.up * -1.0f;
        //パッドの左右キーで車体を旋回させる
        this.transform.Rotate(0, Input.GetAxis(m_Pamarata.m_LRKeyName), 0);
    }
}
