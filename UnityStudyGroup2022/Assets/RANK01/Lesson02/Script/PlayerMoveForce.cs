using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// force���g���Ă̈ړ��V�X�e��
/// ����~�܂�ɂȂ���@
/// </summary>
public class PlayerMoveForce : MonoBehaviour
{
    public Rigidbody m_Rigidbody;
    [Header("��b�ړ���")]
    public float m_MoveSpeed;
    [Header("���͒Ǐ]�x[����������b�ړ��͂Ɠ����l]")]
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
        //�ړ�����
        Vector2 InputData = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //���[�J���ړ�
        Vector3 MoveVector 
            = this.transform.right * (m_MoveSpeed * InputData.x)
            + this.transform.forward * (m_MoveSpeed * InputData.y);

        //�ŏI�ړ��m��
        m_Rigidbody.AddForce(m_MoveForceMultiplier * (MoveVector - m_Rigidbody.velocity));
    }
}
