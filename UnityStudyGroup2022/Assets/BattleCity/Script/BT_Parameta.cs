using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BT_Parameta : MonoBehaviour
{
    [Header("プレイヤー名")]
    public string m_PlayerName;
    [Header("最大耐久力")]
    public int m_MaxHP;
    [Header("耐久力")]
    public int m_HP;
    [Header("左右キー名")]
    public string m_LRKeyName;
    [Header("上下キー名")]
    public string m_UDKeyName;
    [Header("発砲キー名")]
    public string m_FireKeyName;
    [Header("Messageテキスト")]
    public Text m_Mes;
    void Start()
    {
        //messageテキストとリンクが形成できていない
        if (!m_Mes)
        {
            //messageテキストと探す
            GameObject Dummy = GameObject.Find("メッセージテキスト");
            //messageテキストが存在する場合
            if (Dummy)
            {
                //messageテキストとリンクする
                m_Mes = Dummy.GetComponent<Text>();
            }
        }
    }

    void Update()
    {
        //死亡チェックを行う
        DeadCheck();
    }
    /// <summary>
    /// 再出現処理
    /// </summary>
    public void RePop()
    {
        //耐久力を回復させる
        m_HP = m_MaxHP;
        //出現位置をランダムで決める
        this.transform.position = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
    }
    /// <summary>
    /// 死亡チェック
    /// </summary>
    public void DeadCheck()
    {
        //耐久力が０以下の場合
        if (m_HP <= 0)
        {
            //メッセージに敗北を伝達
            m_Mes.text += m_PlayerName + "は撃破された!!\n";
            //再出現しょりを実行
            RePop();
        }
    }
    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="DMG"></param>
    public void Damage(int DMG)
    {
        //ダメージ分耐久から減少
        m_HP -= DMG;
        //与えられた体力が0以下にならないように注意
        if(m_HP <= 0)
            m_HP = 0;
    }
}
