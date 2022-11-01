using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTranslate : MonoBehaviour
{
    [Header("��b�ړ���")]
    public float m_MoveSpeed;

    void Update()
    {
        //�ړ�����
        Vector2 InputData = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //�g�����X���[�g�ňړ�
        this.transform.Translate(new Vector3(InputData.x * m_MoveSpeed, 0, InputData.y * m_MoveSpeed));
    }
}
