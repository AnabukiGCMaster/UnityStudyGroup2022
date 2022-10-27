using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    [Header("�e")]
    public GameObject m_Bullet;
    [Header("�e��")]
    public Transform m_Muzzle;

    void Update()
    {
        //���C����
        Fire();
    }
    /// <summary>
    /// ���C����
    /// </summary>
    public void Fire()
    {
        //�}�E�X���N���b�N
        if(Input.GetMouseButtonDown(0))
        {
            //�e���o�������A�e���̍��W�ƌ����ɍ��킹��
            GameObject Dummy = Instantiate(m_Bullet, m_Muzzle.position, m_Muzzle.rotation);
            //�e�ɕ������Ȃ��ꍇ�A�e�ɕ�������
            if (!Dummy.GetComponent<Rigidbody>())
                Dummy.AddComponent<Rigidbody>();
            //�e�̐��ʂ֌������āA�Η�10000�Ŏˏo����
            Dummy.GetComponent<Rigidbody>().AddForce(Dummy.transform.forward * 10000.0f);
            //�e��5�b��Ɏ������ł���(�\��)
            Destroy(Dummy,5.0f);
        }
    }
}
