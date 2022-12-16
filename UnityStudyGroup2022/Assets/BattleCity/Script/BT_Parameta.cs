using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BT_Parameta : MonoBehaviour
{
    [Header("�v���C���[��")]
    public string m_PlayerName;
    [Header("�ő�ϋv��")]
    public int m_MaxHP;
    [Header("�ϋv��")]
    public int m_HP;
    [Header("���E�L�[��")]
    public string m_LRKeyName;
    [Header("�㉺�L�[��")]
    public string m_UDKeyName;
    [Header("���C�L�[��")]
    public string m_FireKeyName;
    [Header("Message�e�L�X�g")]
    public Text m_Mes;
    void Start()
    {
        //message�e�L�X�g�ƃ����N���`���ł��Ă��Ȃ�
        if (!m_Mes)
        {
            //message�e�L�X�g�ƒT��
            GameObject Dummy = GameObject.Find("���b�Z�[�W�e�L�X�g");
            //message�e�L�X�g�����݂���ꍇ
            if (Dummy)
            {
                //message�e�L�X�g�ƃ����N����
                m_Mes = Dummy.GetComponent<Text>();
            }
        }
    }

    void Update()
    {
        //���S�`�F�b�N���s��
        DeadCheck();
    }
    /// <summary>
    /// �ďo������
    /// </summary>
    public void RePop()
    {
        //�ϋv�͂��񕜂�����
        m_HP = m_MaxHP;
        //�o���ʒu�������_���Ō��߂�
        this.transform.position = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
    }
    /// <summary>
    /// ���S�`�F�b�N
    /// </summary>
    public void DeadCheck()
    {
        //�ϋv�͂��O�ȉ��̏ꍇ
        if (m_HP <= 0)
        {
            //���b�Z�[�W�ɔs�k��`�B
            m_Mes.text += m_PlayerName + "�͌��j���ꂽ!!\n";
            //�ďo�����������s
            RePop();
        }
    }
    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <param name="DMG"></param>
    public void Damage(int DMG)
    {
        //�_���[�W���ϋv���猸��
        m_HP -= DMG;
        //�^����ꂽ�̗͂�0�ȉ��ɂȂ�Ȃ��悤�ɒ���
        if(m_HP <= 0)
            m_HP = 0;
    }
}
