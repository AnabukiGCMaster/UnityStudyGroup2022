using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Tama : MonoBehaviour
{
    [Header("ダメージ値")]
    public int m_DamagePoint;
    void Start()
    {
        //５秒後に弾を消滅させる
        Destroy(this.gameObject, 5.0f);   
    }
    private void OnCollisionEnter(Collision collision)
    {
        //当たった対象にパラメーターがある
        if (collision.gameObject.GetComponent<BT_Parameta>())
        {
            //対象のパラメーターにダメージ値をあたたえる
            collision.gameObject.GetComponent<BT_Parameta>().Damage(m_DamagePoint);
        }
        //何かに接触しているため、強制自己消滅
        Destroy(this.gameObject);
    }
}
