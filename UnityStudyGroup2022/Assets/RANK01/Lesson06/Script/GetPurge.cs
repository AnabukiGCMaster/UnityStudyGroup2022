using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPurge : MonoBehaviour
{
    [Header("�A�E�g�|�C���g")]
    public Transform m_OutPoint;
    private void Update()
    {
        //Purge���`�F�b�N
        PurgeSystem();
    }
    /// <summary>
    /// �p�[�W�V�X�e��
    /// </summary>
    void PurgeSystem()
    {
        //P�L�[�������ꂽ
        if (Input.GetKeyDown(KeyCode.P))
        {
            //�A�E�g�|�C���g�S�ă`�F�b�N
            foreach (Transform Dummy in m_OutPoint)
            {
                //�����̐e�q�����N���O��
                Dummy.transform.parent = null;
                //�����d�͕���
                Dummy.GetComponent<Rigidbody>().useGravity = true;
                //�����ړ���]�}������
                Dummy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                //�����蔻��𕜊�
                Dummy.GetComponent<BoxCollider>().enabled = true;
                //�Y������𑕔�����O��
                this.GetComponent<FireControlSystem>().m_Gun.Remove(Dummy.GetComponent<Gun>());
            }
            //�S�Ă�Null���퐧�����������
            this.GetComponent<FireControlSystem>().m_Gun.RemoveAll(item => item == null);
        }
    }
    /// <summary>
    /// �ڐG�������Ă���ꍇ
    /// </summary>
    /// <param name="collision">�ڐG��</param>
    private void OnCollisionStay(Collision collision)
    {
        //�ڐG���yGun�z�R���|�[�l���g�������A���ݏ������Ă��Ȃ��A
        //����E�L�[�������ꂽ(���������Ă���)
        if (collision.gameObject.GetComponent<Gun>()
            && collision.transform.parent != m_OutPoint
            && Input.GetKey(KeyCode.E))
        {
            //�����蔻����J�b�g
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            //�d�̓J�b�g
            collision.gameObject.GetComponent<Rigidbody>().useGravity = false;
            //�ړ���]�}��
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //�ʒu���A�E�g�|�C���g�ɍ��킹��
            collision.transform.position = m_OutPoint.position;
            //�������A�E�g�|�C���g�ɍ��킹��
            collision.transform.rotation = m_OutPoint.rotation;
            //��������E0.2�`-0.2�̃����_���œ�����
            collision.transform.Translate(
                new Vector3(Random.Range(0.2f, -0.2f), 0, 0));
            //������A�E�g�|�C���g��e�Ƃ��ă����N����
            collision.transform.parent = m_OutPoint;
            //������Ί�ǐ��V�X�e���ɓo�^����
            this.GetComponent<FireControlSystem>().m_Gun.Add(collision.transform.GetComponent<Gun>());
        }
    }
}
