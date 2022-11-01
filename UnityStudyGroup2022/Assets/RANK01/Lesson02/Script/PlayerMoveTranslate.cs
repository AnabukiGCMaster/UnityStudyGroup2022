using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTranslate : MonoBehaviour
{
    [Header("基礎移動力")]
    public float m_MoveSpeed;

    void Update()
    {
        //移動入力
        Vector2 InputData = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //トランスレートで移動
        this.transform.Translate(new Vector3(InputData.x * m_MoveSpeed, 0, InputData.y * m_MoveSpeed));
    }
}
