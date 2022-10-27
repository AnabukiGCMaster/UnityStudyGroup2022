using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    #region ���ϐ�

    #region �O���������J�[
    [Header("���������N")]
    public Rigidbody m_Rigidbody;
    #endregion

    #region ���j�b�g��b�\��
    [Header("��{�ړ����x")]
    public float m_MoveSpeed;
    [Header("�؋󒆃t���O")]
    public bool m_JumpFlag;
    [Header("�W�����v��")]
    public float m_JumpPower;
    [Header("���W�����v�܂ł̃N�[���^�C��")]
    public float m_JumpCoolTime = 0.5f;
    #endregion

    #region ���n�֘A
    [Header("�n�ʒ��n�����蔼�a")]
    public float m_HitEarth = 0.6f;
    [Header("�n�ʒ��n�����苗��")]
    public float m_HitEarthDistance = 0.2f;
    [Header("�n�ʒ��n������X�^�[�g")]
    public Transform m_StartPos;
    [Header("�n�ʒ��n������G���h")]
    public Transform m_EndPos;
    #endregion

    #endregion

    void Start()
    {
        //���������N���\�z����
        m_Rigidbody = this.GetComponent<Rigidbody>();
    }
    void LateUpdate()
    {
        //���j�b�g����̃��C������
        UnitMain();
    }

    #region ���C������
    /// <summary>
    /// ���C������
    /// </summary>
    public void UnitMain()
    {
        //�؋󒆂��`�F�b�N
        if (m_JumpFlag)
        {
            //���؋�

            //�W�����v�N�[���^�C�����I�����Ă���
            if (m_JumpCoolTime <= 0.0f)
            {
                //�؋󂩂�̒��n���菈��
                StayInTheAir();
                //�W�����v�N�[���^�C����0�ݒ�
                m_JumpCoolTime = 0.0f;
            }
            else
            {
                //�W�����v�N�[���^�C��(������)����
                m_JumpCoolTime -= 1.0f * Time.deltaTime;
            }
        }
        else
        {
            //���ڒn���̏ꍇ
            //�ړ�����
            UnitMove();
            //�W�����v����
            JumpStartCheck();
        }
    }
    #endregion

    #region �؋󒆂���̒��n����
    /// <summary>
    /// �؋󒆂���̒��n����
    /// </summary>
    public void StayInTheAir()
    {
        //�n�ʐڐG����t���O��false�Ƃ���
        bool Hit = false;
        //�n�ʐڐG�_
        Vector3 Pos = new Vector3(0, 0, 0);

        //�n�ʐڐG����v���O�����`�F�b�N
        FE_SphericalContactCheck(
            m_StartPos.position,        //�v���C���[�̌��ݍ��W(�o�E���e�B���O�X�t�B�A�̒��S�_)
            m_HitEarth,                     //�o�E���e�B���O�X�t�B�A�̔��a
            -this.transform.up,             //�v���C���[�̉������փ`�F�b�N
            Vector3.Distance(m_StartPos.position, m_EndPos.position),                     //�v���C���[�̒n�ʂƂ̗\������
            ref Hit,                        //�ڐG�t���O
            ref Pos);                       //�ڐG���W
                                            //�ڐG�f�[�^�����݂���

        //�n�ʂƂ̐ڐG������ꍇ�ɏ��������
        if (Hit)
        {
            //�ڐG�����n�ʂ̍��W����L�����N�^�[�̑����܂ł̋������Am_HitEarthDistance�ȉ���
            //�ꍇ�A�W�����v�t���O��Off(�ڒn��)�Ƃ���
            if (Vector3.Distance(this.transform.position, Pos) <= m_HitEarthDistance)
                m_JumpFlag = false;             //�W�����v�t���O��Off�ɂ���
        }
    }
    #endregion

    #region ���j�b�g�ړ�����
    /// <summary>
    /// ���j�b�g�ړ�����
    /// </summary>
    public void UnitMove()
    {
        //�����ړ��ňړ�����
        this.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));

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
    #endregion

    #region �W�����v���s����
    /// <summary>
    /// �W�����v���s
    /// </summary>
    public void JumpStartCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //������֕����E�Ռ�Force�ŃW�����v�͕�������֔�΂�
            //�A���A�L�[���͂���Ă���ꍇ�́A���̕������܂�
            m_Rigidbody.AddForce(transform.up * m_JumpPower, ForceMode.Impulse);
            //�؋�t���O��ON�ɂ���
            m_JumpFlag = true;
            //�W�����v�N�[���^�C����ݒ肷��
            m_JumpCoolTime = 0.25f;
        }
    }
    #endregion

    #region �X�t�B�A�^�R���W�����ڐG����
    /// ������������������������������������������������������������������������������������������������\n
    /// �X�t�B�A�^�R���W�����ڐG����
    /// ���̂̓����蔻��𐶐���A����������ړ��ʕ�����������
    /// �J�v�Z���^�����蔻��𐶐�����B
    /// ����́A�I���R���W�����̎󓮓I�Ȕ�����g�p�����A�\���I�ɗ��p�ł��闘�_������B
    /// �����蔻�肪�����Ē���(�Ӗ��[)�̃r�[������̓����蔻�蓙�ɂ͏d�󂳂��
    /// ��́A�R���W�����͕����I�ڐG�ł��݂��e�����A����͒e�����ʂ��Ȃ��B
    /// ������������������������������������������������������������������������������������������������\n
    #region �I���R���W�������g�킸�A�����ڐG�ɂ��e���ꖳ���œ����蔻��𔻒肷��
    /// <summary>
    /// �I���R���W�������g�킸�A�����ڐG�ɂ��e���ꖳ���œ����蔻��𔻒肷��B
    /// </summary>
    /// <param name="m_BouncingSphereCenterPoint">���̂̒��S�ʒu</param>
    /// <param name="m_BouncingSphereRadius">���̂̔��a</param>
    /// <param name="m_BouncingSphereCenterDirection">���̂̓��������</param>
    /// <param name="m_BouncingSphereHitMaxDistance">���̂̍ő哖���苗��</param>
    /// <param name="m_ContactJudgment">�����茋��(bool)</param>
    public static void FE_SphericalContactCheck(
        Vector3 m_BouncingSphereCenterPoint,
        float m_BouncingSphereRadius,
        Vector3 m_BouncingSphereCenterDirection,
        float m_BouncingSphereHitMaxDistance,
        ref bool m_ContactJudgment,
        ref Vector3 Pos)
    {
        ///�o�E���f�B���O�X�t�B�A�q�b�g��p��
        RaycastHit m_BouncingSphere_hit;
        ///�ڐG�������ǂ����𔻒�A�q�b�g���Ă�����m_ContactJudgment��true������
        m_ContactJudgment = Physics.SphereCast(
            //�`�F�b�N�X�^�[�g�n�_
            m_BouncingSphereCenterPoint,
            //�`�F�b�N�̕��͂ǂꂮ�炢��?(���a�̑傫��)
            m_BouncingSphereRadius,
            //�`�F�b�N��������͂ǂ���?
            m_BouncingSphereCenterDirection,
            //���������Ώۂ��l������
            out m_BouncingSphere_hit,
            //�`�F�b�N�X�^�[�g����`�F�b�N��������ւǂ�ʂ̋����`�F�b�N���邩?
            m_BouncingSphereHitMaxDistance);

        //�������Ԃ����Ă���ꍇ���`�F�b�N
        if (m_ContactJudgment)
        {
            //�������Ԃ����Ă���ꍇ�A���������Ώۂ̍��W(���m�ȐڐG�n�_)�𒊏o
            Pos = m_BouncingSphere_hit.point;
        }
        else
        {
            //�������Ă��Ȃ��ꍇ�́A0�Őݒ�
            Pos = new Vector3(0, 0, 0);
        }
    }
    #endregion
    #endregion


}





public class ScriptHitTester : MonoBehaviour
{

}
