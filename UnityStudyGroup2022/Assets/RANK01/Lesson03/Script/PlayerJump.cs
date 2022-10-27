using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    #region ■変数

    #region 外部干渉リンカー
    [Header("物理リンク")]
    public Rigidbody m_Rigidbody;
    #endregion

    #region ユニット基礎能力
    [Header("基本移動速度")]
    public float m_MoveSpeed;
    [Header("滞空中フラグ")]
    public bool m_JumpFlag;
    [Header("ジャンプ力")]
    public float m_JumpPower;
    [Header("次ジャンプまでのクールタイム")]
    public float m_JumpCoolTime = 0.5f;
    #endregion

    #region 着地関連
    [Header("地面着地当たり半径")]
    public float m_HitEarth = 0.6f;
    [Header("地面着地当たり距離")]
    public float m_HitEarthDistance = 0.2f;
    [Header("地面着地当たりスタート")]
    public Transform m_StartPos;
    [Header("地面着地当たりエンド")]
    public Transform m_EndPos;
    #endregion

    #endregion

    void Start()
    {
        //物理リンクを構築する
        m_Rigidbody = this.GetComponent<Rigidbody>();
    }
    void LateUpdate()
    {
        //ユニット制御のメイン処理
        UnitMain();
    }

    #region メイン処理
    /// <summary>
    /// メイン処理
    /// </summary>
    public void UnitMain()
    {
        //滞空中かチェック
        if (m_JumpFlag)
        {
            //■滞空中

            //ジャンプクールタイムが終了している
            if (m_JumpCoolTime <= 0.0f)
            {
                //滞空からの着地判定処理
                StayInTheAir();
                //ジャンプクールタイムを0設定
                m_JumpCoolTime = 0.0f;
            }
            else
            {
                //ジャンプクールタイム(実時間)減少
                m_JumpCoolTime -= 1.0f * Time.deltaTime;
            }
        }
        else
        {
            //■接地中の場合
            //移動処理
            UnitMove();
            //ジャンプ処理
            JumpStartCheck();
        }
    }
    #endregion

    #region 滞空中からの着地判定
    /// <summary>
    /// 滞空中からの着地判定
    /// </summary>
    public void StayInTheAir()
    {
        //地面接触判定フラグをfalseとする
        bool Hit = false;
        //地面接触点
        Vector3 Pos = new Vector3(0, 0, 0);

        //地面接触判定プログラムチェック
        FE_SphericalContactCheck(
            m_StartPos.position,        //プレイヤーの現在座標(バウンティングスフィアの中心点)
            m_HitEarth,                     //バウンティングスフィアの半径
            -this.transform.up,             //プレイヤーの下方向へチェック
            Vector3.Distance(m_StartPos.position, m_EndPos.position),                     //プレイヤーの地面との予測距離
            ref Hit,                        //接触フラグ
            ref Pos);                       //接触座標
                                            //接触データが存在する

        //地面との接触がある場合に処理される
        if (Hit)
        {
            //接触した地面の座標からキャラクターの足元までの距離が、m_HitEarthDistance以下の
            //場合、ジャンプフラグをOff(接地中)とする
            if (Vector3.Distance(this.transform.position, Pos) <= m_HitEarthDistance)
                m_JumpFlag = false;             //ジャンプフラグをOffにする
        }
    }
    #endregion

    #region ユニット移動処理
    /// <summary>
    /// ユニット移動処理
    /// </summary>
    public void UnitMove()
    {
        //物理移動で移動する
        this.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));

        //キーボードによる移動入力
        Vector3 InputPoint = new Vector3(
            Input.GetAxis("Horizontal"),
            -1.0f,
            Input.GetAxis("Vertical"));

        //物理ベロシティに移動量を代入すると、その移動量分移動する
        m_Rigidbody.velocity
            = this.transform.right * (m_MoveSpeed * InputPoint.x)       //キャラ左右移動量
            + this.transform.up * InputPoint.y                          //キャラ落下量
            + this.transform.forward * (m_MoveSpeed * InputPoint.z);    //キャラ前後移動量
    }
    #endregion

    #region ジャンプ実行処理
    /// <summary>
    /// ジャンプ実行
    /// </summary>
    public void JumpStartCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //上方向へ物理・衝撃Forceでジャンプ力分上方向へ飛ばす
            //但し、キー入力されている場合は、その方向も含む
            m_Rigidbody.AddForce(transform.up * m_JumpPower, ForceMode.Impulse);
            //滞空フラグをONにする
            m_JumpFlag = true;
            //ジャンプクールタイムを設定する
            m_JumpCoolTime = 0.25f;
        }
    }
    #endregion

    #region スフィア型コリジョン接触処理
    /// ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■\n
    /// スフィア型コリジョン接触処理
    /// 球体の当たり判定を生成後、それを向き移動量分動かす事で
    /// カプセル型当たり判定を生成する。
    /// これは、オンコリジョンの受動的な判定を使用せず、能動的に利用できる利点がある。
    /// 当たり判定が太くて長い(意味深)のビーム兵器の当たり判定等には重宝される
    /// 後は、コリジョンは物理的接触でお互い弾くが、これは弾く効果がない。
    /// ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■\n
    #region オンコリジョンを使わず、物理接触による弾かれ無しで当たり判定を判定する
    /// <summary>
    /// オンコリジョンを使わず、物理接触による弾かれ無しで当たり判定を判定する。
    /// </summary>
    /// <param name="m_BouncingSphereCenterPoint">球体の中心位置</param>
    /// <param name="m_BouncingSphereRadius">球体の半径</param>
    /// <param name="m_BouncingSphereCenterDirection">球体の当たり向き</param>
    /// <param name="m_BouncingSphereHitMaxDistance">球体の最大当たり距離</param>
    /// <param name="m_ContactJudgment">当たり結果(bool)</param>
    public static void FE_SphericalContactCheck(
        Vector3 m_BouncingSphereCenterPoint,
        float m_BouncingSphereRadius,
        Vector3 m_BouncingSphereCenterDirection,
        float m_BouncingSphereHitMaxDistance,
        ref bool m_ContactJudgment,
        ref Vector3 Pos)
    {
        ///バウンディングスフィアヒットを用意
        RaycastHit m_BouncingSphere_hit;
        ///接触したかどうかを判定、ヒットしていたらm_ContactJudgmentにtrueが入る
        m_ContactJudgment = Physics.SphereCast(
            //チェックスタート地点
            m_BouncingSphereCenterPoint,
            //チェックの幅はどれぐらいか?(半径の大きさ)
            m_BouncingSphereRadius,
            //チェックする方向はどこか?
            m_BouncingSphereCenterDirection,
            //当たった対象を獲得する
            out m_BouncingSphere_hit,
            //チェックスタートからチェックする方向へどれ位の距離チェックするか?
            m_BouncingSphereHitMaxDistance);

        //何かがぶつかっている場合をチェック
        if (m_ContactJudgment)
        {
            //何かがぶつかっている場合、当たった対象の座標(明確な接触地点)を抽出
            Pos = m_BouncingSphere_hit.point;
        }
        else
        {
            //当たっていない場合は、0で設定
            Pos = new Vector3(0, 0, 0);
        }
    }
    #endregion
    #endregion


}





public class ScriptHitTester : MonoBehaviour
{

}
