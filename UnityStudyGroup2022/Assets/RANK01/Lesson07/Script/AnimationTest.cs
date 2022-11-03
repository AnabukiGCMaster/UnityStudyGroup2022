using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    [Header("アニメーターリンク")]
    public Animator m_Animator;
    void Update()
    {
        Vector2 AnimatorPoint = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));
        m_Animator.SetFloat("MoveX", AnimatorPoint.x);
        m_Animator.SetFloat("MoveY", AnimatorPoint.y);
    }
}
