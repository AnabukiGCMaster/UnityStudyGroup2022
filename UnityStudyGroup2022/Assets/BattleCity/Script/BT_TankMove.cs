using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_TankMove : MonoBehaviour
{
    [Header("�p�����[�^�[�����N")]
    public BT_Parameta m_Pamarata;
    [Header("�ړ���")]
    public float m_MovePower = 2.0f;
    void Start()
    {
        //�p�����[�^�[�����N
        m_Pamarata = GetComponent<BT_Parameta>();
    }

    void Update()
    {
        //�p�b�h�̏㉺�L�[�Ńx���V�e�B�Ŏԑ̂�O��ނ�����
        GetComponent<Rigidbody>().velocity =
            -this.transform.forward * (Input.GetAxis(m_Pamarata.m_UDKeyName) * m_MovePower) +
            this.transform.up * -1.0f;
        //�p�b�h�̍��E�L�[�Ŏԑ̂���񂳂���
        this.transform.Rotate(0, Input.GetAxis(m_Pamarata.m_LRKeyName), 0);
    }
}
