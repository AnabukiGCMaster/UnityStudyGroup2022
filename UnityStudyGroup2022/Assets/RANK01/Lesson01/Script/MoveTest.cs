using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    [Header("�L���[�u�̃��[�J�����W�ł̈ړ����x")]
    public Vector3 m_MoveSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        this.transform.Translate(m_MoveSpeed);
    }
}
