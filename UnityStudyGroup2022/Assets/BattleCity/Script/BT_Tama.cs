using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Tama : MonoBehaviour
{
    [Header("�_���[�W�l")]
    public int m_DamagePoint;
    void Start()
    {
        //�T�b��ɒe�����ł�����
        Destroy(this.gameObject, 5.0f);   
    }
    private void OnCollisionEnter(Collision collision)
    {
        //���������ΏۂɃp�����[�^�[������
        if (collision.gameObject.GetComponent<BT_Parameta>())
        {
            //�Ώۂ̃p�����[�^�[�Ƀ_���[�W�l������������
            collision.gameObject.GetComponent<BT_Parameta>().Damage(m_DamagePoint);
        }
        //�����ɐڐG���Ă��邽�߁A�������ȏ���
        Destroy(this.gameObject);
    }
}
