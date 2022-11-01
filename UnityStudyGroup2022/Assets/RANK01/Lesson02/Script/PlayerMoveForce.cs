using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// forceを使っての移動システム
/// 滑り止まりになる方法
/// </summary>
public class PlayerMoveForce : MonoBehaviour
{
    public Rigidbody m_Rigidbody;
    [Header("基礎移動力")]
    public float m_MoveSpeed;
    [Header("入力追従度[だいたい基礎移動力と同じ値]")]
    public float m_MoveForceMultiplier;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        //移動入力
        Vector2 InputData = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //ローカル移動
        Vector3 MoveVector 
            = this.transform.right * (m_MoveSpeed * InputData.x)
            + this.transform.forward * (m_MoveSpeed * InputData.y);

        //最終移動確定
        m_Rigidbody.AddForce(m_MoveForceMultiplier * (MoveVector - m_Rigidbody.velocity));
    }
}
