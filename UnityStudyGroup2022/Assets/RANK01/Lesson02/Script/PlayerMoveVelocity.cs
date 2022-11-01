using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// プレイヤー移動(物理ベロシティ移動方式)
/// 物理ベロシティ方式は、物理的な移動方式の中で「その瞬間」の移動量で移動する方法
/// その為、無駄な移動力計算がいらない。
/// 但し、ベロシティはどんな時でも物理計算で優先される為、「座標は確定座標となる」
/// つまり重力もベロシティに加えないと落下さえしなくなる欠点がある。
/// </summary>
public class PlayerMoveVelocity : MonoBehaviour
{
    [Header("物理リンク")]
    public Rigidbody m_Rigidbody;
    [Header("キャラクターの基礎移動量")]
    public float m_MoveSpeed;
    void Start()
    {
        //物理を取り込む
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //マウスによる回転で、キャラクターの向きを変える
        this.transform.Rotate(
            new Vector3(0, Input.GetAxis("Mouse X"), 0));

        //キーボードによる移動入力
        Vector3 InputPoint = new Vector3(
            Input.GetAxis("Horizontal"),
            -1.0f,
            Input.GetAxis("Vertical"));

        //物理ベロシティに移動量を代入すると、その移動量分移動する
        m_Rigidbody.velocity 
            = this.transform.right * (m_MoveSpeed * InputPoint.x)       //キャラ左右移動量
            + this.transform.up * InputPoint.y                          //キャラ落下量
            + this.transform.forward * (m_MoveSpeed * InputPoint.z);    //キャラ前後移動量
    }
}
