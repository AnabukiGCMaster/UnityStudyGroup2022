using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Fire : MonoBehaviour
{
    [Header("�e���ʒu")]
    public Transform m_Gun;
    [Header("���˒ePrefab")]
    public GameObject m_Tama;
    [Header("���˗�")]
    public float m_Power;
    [Header("���񔭎˃N�[���^�C��")]
    public float m_CoolTime = 0.0f;
    [Header("�ő唭�˃N�[���^�C��")]
    public float m_MaxCoolTime = 1.0f;
    [Header("�p�����[�^�ւ̃����N")]
    public BT_Parameta m_Pamarata;
    [Header("�������ςȂ����˗}���t���O")]
    public bool m_FireTriggerFlag;
    void Start()
    {
        //�p�����[�^�[�ƃ����N
        m_Pamarata = GetComponent<BT_Parameta>();
    }

    // Update is called once per frame
    void Update()
    {
        //�N�[���^�C����0�ȉ��ł���
        if (m_CoolTime <= 0.0f)
        {
            //���˃L�[�������ꂽ+�A�����˂��Ă��Ȃ�
            if (Input.GetAxis(m_Pamarata.m_FireKeyName) > 0 && !m_FireTriggerFlag)
            {
                //�A�����˗}�����s��
                m_FireTriggerFlag = true;
                //�e�𐶐�����
                GameObject Dummy = Instantiate(m_Tama, m_Gun.position, m_Gun.rotation);
                //�e�̃x���V�e�B�Ɋ����đO���ɑ΂��ĉ����o��
                Dummy.GetComponent<Rigidbody>().AddForce(m_Gun.forward * m_Power);
                //�N�[���^�C��������
                m_CoolTime = m_MaxCoolTime;
            }
            else
            {
                //�A�����˗}�����I������
                m_FireTriggerFlag = false;
            }
        }
        else
        {
            //�N�[���^�C��������
            m_CoolTime -= 1.0f * Time.deltaTime;
        }
    }
}
