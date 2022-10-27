using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("弾")]
    public GameObject m_Bullet;
    [Header("銃口の管理オブジェクト")]
    public Transform m_AutoMasterMuzzle;
    [Header("銃口[Auto]")]
    public List<Transform> m_Muzzle;

    private void Start()
    {
        //銃口リストを初期化
        m_Muzzle = new List<Transform>();
        //銃口管理オブジェクト直下のオブジェクトを銃口として自動登録
        foreach (Transform Point in m_AutoMasterMuzzle)
            m_Muzzle.Add(Point);
    }
    /// <summary>
    /// 火器管制システムからアクセスする
    /// 発砲処理
    /// </summary>
    public void Fire()
    {
        //銃口が複数ある場合は、銃口の数だけ発砲する
        foreach (Transform MuzzlePoint in m_Muzzle)
        {
            //弾を出現させ、銃口の座標と向きに合わせる
            GameObject Dummy = Instantiate(m_Bullet, MuzzlePoint.position, MuzzlePoint.rotation);
            //弾に物理がない場合、弾に物理を代入
            if (!Dummy.GetComponent<Rigidbody>())
                Dummy.AddComponent<Rigidbody>();
            //弾の正面へ向かって、火力10000で射出する
            Dummy.GetComponent<Rigidbody>().AddForce(Dummy.transform.forward * 10000.0f);
            //弾は5秒後に自動消滅する(予約)
            Destroy(Dummy, 5.0f);
        }
    }
}
