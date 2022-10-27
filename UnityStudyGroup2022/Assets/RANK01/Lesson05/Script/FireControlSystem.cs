using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControlSystem : MonoBehaviour
{
    [Header("アウトポイント")]
    public Transform m_OutOpint;
    [Header("武器リスト")]
    public List<Gun> m_Gun;

    void Start()
    {
        //武器リスト初期化
        m_Gun = new List<Gun>();
        //アウトポイントのオブジェクトに内包されたオブジェクトを走査
        foreach (Transform Dummy in m_OutOpint)
        {
            //該当オブジェクトにGunコンポーネントがあれば、それをリストに追加する
            if (Dummy.GetComponent<Gun>())
                m_Gun.Add(Dummy.GetComponent<Gun>());
        }
    }

    void Update()
    {
        //発砲チェック
        Fire();
    }
    /// <summary>
    /// 発砲チェック
    /// </summary>
    public void Fire()
    {
        //左マウスボタンが押されたら、武器リスト登録されたすべてに発砲許可を与える
        if (Input.GetMouseButtonDown(0))
            foreach (Gun Dummy in m_Gun)
                Dummy.Fire();
    }
}
