using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    [Header("キューブのローカル座標での移動速度")]
    public Vector3 m_MoveSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        this.transform.Translate(m_MoveSpeed);
    }
}
