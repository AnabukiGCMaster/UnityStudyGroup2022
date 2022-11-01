using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    //キューブの回転速度
    [Header("キューブの回転速度")]
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
