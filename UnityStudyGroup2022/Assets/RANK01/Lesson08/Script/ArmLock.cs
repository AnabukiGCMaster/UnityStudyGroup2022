using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLock : MonoBehaviour
{
    public Animator m_Animator;
    public bool m_ArmsLockFlag;
    public Transform m_ArmsLockPoint;

    void Update()
    {
        //�E�h���b�O���ɍ\����
        if (Input.GetMouseButton(1))
            m_ArmsLockFlag = true;
        else
            m_ArmsLockFlag = false;
    }
    // IK ���v�Z���邽�߂̃R�[���o�b�N
    void OnAnimatorIK()
    {
        //Animator�����邩?
        if (m_Animator)
        {
            //�\���t���O�������Ă��邩?
            if (m_ArmsLockFlag)
            {
                //����\���ʒu�����݂��邩?
                if (m_ArmsLockPoint)
                {
                    //�E��̈ʒu�E�F�C�g�ݒ�(0�̏ꍇ�A����\���ʒu�Ɉˑ�)
                    m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    //�E��̌����E�F�C�g�ݒ�(0�̏ꍇ�A����\���ʒu�Ɉˑ�)
                    m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    //�E��ʒu�𕐊�\���ʒu�ɃE�F�C�g�𔽉f���ĕύX
                    m_Animator.SetIKPosition(AvatarIKGoal.RightHand, m_ArmsLockPoint.position);
                    //�E������𕐊�\���ʒu�ɃE�F�C�g�𔽉f���ĕύX
                    m_Animator.SetIKRotation(AvatarIKGoal.RightHand, m_ArmsLockPoint.rotation);
                }
            }
            else
            {
                //�E��̈ʒu�E�F�C�g�ݒ�(0�̏ꍇ�A�A�j���[�V�����Ɉˑ�)
                m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                //�E��̌����E�F�C�g�ݒ�(0�̏ꍇ�A�A�j���[�V�����Ɉˑ�)
                m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
        }
    }

}