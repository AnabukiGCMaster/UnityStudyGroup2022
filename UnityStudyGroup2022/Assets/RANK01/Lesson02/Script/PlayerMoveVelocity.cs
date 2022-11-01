using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �v���C���[�ړ�(�����x���V�e�B�ړ�����)
/// �����x���V�e�B�����́A�����I�Ȉړ������̒��Łu���̏u�ԁv�̈ړ��ʂňړ�������@
/// ���ׁ̈A���ʂȈړ��͌v�Z������Ȃ��B
/// �A���A�x���V�e�B�͂ǂ�Ȏ��ł������v�Z�ŗD�悳���ׁA�u���W�͊m����W�ƂȂ�v
/// �܂�d�͂��x���V�e�B�ɉ����Ȃ��Ɨ����������Ȃ��Ȃ錇�_������B
/// </summary>
public class PlayerMoveVelocity : MonoBehaviour
{
    [Header("���������N")]
    public Rigidbody m_Rigidbody;
    [Header("�L�����N�^�[�̊�b�ړ���")]
    public float m_MoveSpeed;
    void Start()
    {
        //��������荞��
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //�}�E�X�ɂ���]�ŁA�L�����N�^�[�̌�����ς���
        this.transform.Rotate(
            new Vector3(0, Input.GetAxis("Mouse X"), 0));

        //�L�[�{�[�h�ɂ��ړ�����
        Vector3 InputPoint = new Vector3(
            Input.GetAxis("Horizontal"),
            -1.0f,
            Input.GetAxis("Vertical"));

        //�����x���V�e�B�Ɉړ��ʂ�������ƁA���̈ړ��ʕ��ړ�����
        m_Rigidbody.velocity 
            = this.transform.right * (m_MoveSpeed * InputPoint.x)       //�L�������E�ړ���
            + this.transform.up * InputPoint.y                          //�L����������
            + this.transform.forward * (m_MoveSpeed * InputPoint.z);    //�L�����O��ړ���
    }
}
