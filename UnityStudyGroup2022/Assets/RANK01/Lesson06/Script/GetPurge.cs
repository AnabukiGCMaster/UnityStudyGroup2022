using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPurge : MonoBehaviour
{
    [Header("アウトポイント")]
    public Transform m_OutPoint;
    private void Update()
    {
        //Purgeをチェック
        PurgeSystem();
    }
    /// <summary>
    /// パージシステム
    /// </summary>
    void PurgeSystem()
    {
        //Pキーが押された
        if (Input.GetKeyDown(KeyCode.P))
        {
            //アウトポイント全てチェック
            foreach (Transform Dummy in m_OutPoint)
            {
                //装備の親子リンクを外す
                Dummy.transform.parent = null;
                //物理重力復活
                Dummy.GetComponent<Rigidbody>().useGravity = true;
                //物理移動回転抑制解除
                Dummy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                //当たり判定を復活
                Dummy.GetComponent<BoxCollider>().enabled = true;
                //該当武器を装備から外す
                this.GetComponent<FireControlSystem>().m_Gun.Remove(Dummy.GetComponent<Gun>());
            }
            //全てのNull武器制御を解除する
            this.GetComponent<FireControlSystem>().m_Gun.RemoveAll(item => item == null);
        }
    }
    /// <summary>
    /// 接触し続けている場合
    /// </summary>
    /// <param name="collision">接触物</param>
    private void OnCollisionStay(Collision collision)
    {
        //接触が【Gun】コンポーネントを持ち、現在所持していない、
        //かつEキーが押された(押し続けている)
        if (collision.gameObject.GetComponent<Gun>()
            && collision.transform.parent != m_OutPoint
            && Input.GetKey(KeyCode.E))
        {
            //当たり判定をカット
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            //重力カット
            collision.gameObject.GetComponent<Rigidbody>().useGravity = false;
            //移動回転抑制
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //位置をアウトポイントに合わせる
            collision.transform.position = m_OutPoint.position;
            //向きをアウトポイントに合わせる
            collision.transform.rotation = m_OutPoint.rotation;
            //武器を左右0.2～-0.2のランダムで動かす
            collision.transform.Translate(
                new Vector3(Random.Range(0.2f, -0.2f), 0, 0));
            //武器をアウトポイントを親としてリンクする
            collision.transform.parent = m_OutPoint;
            //武器を火器管制システムに登録する
            this.GetComponent<FireControlSystem>().m_Gun.Add(collision.transform.GetComponent<Gun>());
        }
    }
}
