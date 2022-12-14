using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    [Header("弾")]
    public GameObject m_Bullet;
    [Header("銃口")]
    public Transform m_Muzzle;

    void Update()
    {
        //発砲処理
        Fire();
    }
    /// <summary>
    /// 発砲処理
    /// </summary>
    public void Fire()
    {
        //マウス左クリック
        if(Input.GetMouseButtonDown(0))
        {
            //弾を出現させ、銃口の座標と向きに合わせる
            GameObject Dummy = Instantiate(m_Bullet, m_Muzzle.position, m_Muzzle.rotation);
            //弾に物理がない場合、弾に物理を代入
            if (!Dummy.GetComponent<Rigidbody>())
                Dummy.AddComponent<Rigidbody>();
            //弾の正面へ向かって、火力10000で射出する
            Dummy.GetComponent<Rigidbody>().AddForce(Dummy.transform.forward * 10000.0f);
            //弾は5秒後に自動消滅する(予約)
            Destroy(Dummy,5.0f);
        }
    }
}
