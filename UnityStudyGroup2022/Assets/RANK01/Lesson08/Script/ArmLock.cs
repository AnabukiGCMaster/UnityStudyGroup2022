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
        //右ドラッグ中に構える
        if (Input.GetMouseButton(1))
            m_ArmsLockFlag = true;
        else
            m_ArmsLockFlag = false;
    }
    // IK を計算するためのコールバック
    void OnAnimatorIK()
    {
        //Animatorがあるか?
        if (m_Animator)
        {
            //構えフラグが立っているか?
            if (m_ArmsLockFlag)
            {
                //武器構え位置が存在するか?
                if (m_ArmsLockPoint)
                {
                    //右手の位置ウェイト設定(0の場合、武器構え位置に依存)
                    m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    //右手の向きウェイト設定(0の場合、武器構え位置に依存)
                    m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    //右手位置を武器構え位置にウェイトを反映して変更
                    m_Animator.SetIKPosition(AvatarIKGoal.RightHand, m_ArmsLockPoint.position);
                    //右手向きを武器構え位置にウェイトを反映して変更
                    m_Animator.SetIKRotation(AvatarIKGoal.RightHand, m_ArmsLockPoint.rotation);
                }
            }
            else
            {
                //右手の位置ウェイト設定(0の場合、アニメーションに依存)
                m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                //右手の向きウェイト設定(0の場合、アニメーションに依存)
                m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
        }
    }

}
