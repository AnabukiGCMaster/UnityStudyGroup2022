using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    //�L���[�u�̉�]���x
    [Header("�L���[�u�̉�]���x")]
    public Vector3 m_Kaiten;
    void Start()
    {
        this.gameObject.AddComponent<MoveTest>();
    }

    void Update()
    {
        this.transform.Rotate(m_Kaiten);
    }
}
