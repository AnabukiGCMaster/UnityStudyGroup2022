using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControlSystem : MonoBehaviour
{
    [Header("�A�E�g�|�C���g")]
    public Transform m_OutOpint;
    [Header("���탊�X�g")]
    public List<Gun> m_Gun;

    void Start()
    {
        //���탊�X�g������
        m_Gun = new List<Gun>();
        //�A�E�g�|�C���g�̃I�u�W�F�N�g�ɓ���ꂽ�I�u�W�F�N�g�𑖍�
        foreach (Transform Dummy in m_OutOpint)
        {
            //�Y���I�u�W�F�N�g��Gun�R���|�[�l���g������΁A��������X�g�ɒǉ�����
            if (Dummy.GetComponent<Gun>())
                m_Gun.Add(Dummy.GetComponent<Gun>());
        }
    }

    void Update()
    {
        //���C�`�F�b�N
        Fire();
    }
    /// <summary>
    /// ���C�`�F�b�N
    /// </summary>
    public void Fire()
    {
        //���}�E�X�{�^���������ꂽ��A���탊�X�g�o�^���ꂽ���ׂĂɔ��C����^����
        if (Input.GetMouseButtonDown(0))
            foreach (Gun Dummy in m_Gun)
                Dummy.Fire();
    }
}
