using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("�e")]
    public GameObject m_Bullet;
    [Header("�e���̊Ǘ��I�u�W�F�N�g")]
    public Transform m_AutoMasterMuzzle;
    [Header("�e��[Auto]")]
    public List<Transform> m_Muzzle;

    private void Start()
    {
        //�e�����X�g��������
        m_Muzzle = new List<Transform>();
        //�e���Ǘ��I�u�W�F�N�g�����̃I�u�W�F�N�g���e���Ƃ��Ď����o�^
        foreach (Transform Point in m_AutoMasterMuzzle)
            m_Muzzle.Add(Point);
    }
    /// <summary>
    /// �Ί�ǐ��V�X�e������A�N�Z�X����
    /// ���C����
    /// </summary>
    public void Fire()
    {
        //�e������������ꍇ�́A�e���̐��������C����
        foreach (Transform MuzzlePoint in m_Muzzle)
        {
            //�e���o�������A�e���̍��W�ƌ����ɍ��킹��
            GameObject Dummy = Instantiate(m_Bullet, MuzzlePoint.position, MuzzlePoint.rotation);
            //�e�ɕ������Ȃ��ꍇ�A�e�ɕ�������
            if (!Dummy.GetComponent<Rigidbody>())
                Dummy.AddComponent<Rigidbody>();
            //�e�̐��ʂ֌������āA�Η�10000�Ŏˏo����
            Dummy.GetComponent<Rigidbody>().AddForce(Dummy.transform.forward * 10000.0f);
            //�e��5�b��Ɏ������ł���(�\��)
            Destroy(Dummy, 5.0f);
        }
    }
}
