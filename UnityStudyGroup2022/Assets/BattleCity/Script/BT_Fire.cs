using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Fire : MonoBehaviour
{
    [Header("銃口位置")]
    public Transform m_Gun;
    [Header("発射弾Prefab")]
    public GameObject m_Tama;
    [Header("発射力")]
    public float m_Power;
    [Header("次回発射クールタイム")]
    public float m_CoolTime = 0.0f;
    [Header("最大発射クールタイム")]
    public float m_MaxCoolTime = 1.0f;
    [Header("パラメータへのリンク")]
    public BT_Parameta m_Pamarata;
    [Header("押しっぱなし発射抑制フラグ")]
    public bool m_FireTriggerFlag;
    void Start()
    {
        //パラメーターとリンク
        m_Pamarata = GetComponent<BT_Parameta>();
    }

    // Update is called once per frame
    void Update()
    {
        //クールタイムが0以下である
        if (m_CoolTime <= 0.0f)
        {
            //発射キーが押された+連続発射していない
            if (Input.GetAxis(m_Pamarata.m_FireKeyName) > 0 && !m_FireTriggerFlag)
            {
                //連続発射抑制を行う
                m_FireTriggerFlag = true;
                //弾を生成する
                GameObject Dummy = Instantiate(m_Tama, m_Gun.position, m_Gun.rotation);
                //弾のベロシティに干渉して前方に対して押し出す
                Dummy.GetComponent<Rigidbody>().AddForce(m_Gun.forward * m_Power);
                //クールタイム初期化
                m_CoolTime = m_MaxCoolTime;
            }
            else
            {
                //連続発射抑制を終了する
                m_FireTriggerFlag = false;
            }
        }
        else
        {
            //クールタイム減少中
            m_CoolTime -= 1.0f * Time.deltaTime;
        }
    }
}
