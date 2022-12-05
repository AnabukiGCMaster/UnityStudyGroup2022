using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static DijkstraCore;
/// <summary>
/// ダイクストラによるノード最短検索
/// By井上
/// </summary>
public class DijkstraCore : MonoBehaviour
{
    #region ノードデータ
    [System.Serializable]
    public struct PointLine
    {
        //各ノード間の相対距離(=重さ)
        public List<float> m_PointNode;
    }

    [System.Serializable]
    public struct LinkData
    {
        //ノードの位置(トランスフォーム)
        public Transform m_Pos;
        //各ノード間の相対距離データ構造体
        public PointLine m_PointLineData;
    }
    #endregion

    //ノード数を獲得
    private int m_Count = 0;

    //出発地から目的地までの最短経路上の地点の地点番号を
    //目的地から出発地の順に設定する1次元配列(ルート逆順)
    private int[] m_StartEndRoute = new int[0];

    //出発地から目的地までの最短距離
    private float m_StartEndDistance;

    //その他使う変数
    private float[] m_CheckDistance = new float[0];

    //出発地から各地点までの最短距離を設定する配列
    private int[] m_CheckRoute = new int[0];

    //出発地から各地点までの最短距離が確定しているかどうかを識別するための配列
    private bool[] m_CheckFixed = new bool[0];

    //最短ノードid
    private int sPoint;

    //最終距離
    private float NewDistance;

    [Header("デバックライン[進行ルート]")]
    public LineRenderer m_DebugLine;

    [Header("テストケース:スタート地点")]
    public Transform m_StartPos;

    [Header("テストケース:ゴール地点")]
    public Transform m_EndPos;

    [Header("テストケース:ノードを子にしているオブジェクト")]
    public Transform m_Master;

    [Header("テストケース:返された次の目的地")]
    public Transform m_ReturnMovePoint;

    [Header("最終的なルート順番")]
    public List<Transform> RootCounterLinePoint;

    /// <summary>
    /// ノードデータ
    /// </summary>
    [Header("ノードデータ")]
    public List<LinkData> m_Node;

    [Header("デバックフラグ[計測デバッグ]")]
    public bool m_Debug;

    private void Start()
    {
        //ダイクストラ測定開始
        DijkstraTry();
    }

    #region ダイクストラ測定開始
    /// <summary>
    /// ダイクストラ測定開始
    /// </summary>
    public void DijkstraTry()
    {
        //マスター(ノードを格納したオブジェクト)が存在しているか?
        if (m_Master)
        {
            //自動ノードセットアップ
            AutoNodeSetUp(m_Master);
        }

        //ストップウォッチセットアップ
        System.Diagnostics.Stopwatch m_StopWatch = new System.Diagnostics.Stopwatch();

#if m_Debug
        //測定開始
        m_StopWatch.Start();
#endif

        //ダイクストラ実行(※この一行で、ダイクストラを計算する)
        m_ReturnMovePoint = D_System(m_StartPos, m_EndPos, true);

#if m_Debug
        //測定終了
        m_StopWatch.Stop();
#endif

        //ルートの正規化(※この一行で、ダイクストラで計算されたルートを正規化する)
        RootUpdate();

        //デバックラインをセットする
        DebugRootLine();

        //デバックフラグが立っている場合、計測時間を表示する
#if m_Debug
        Debug.Log("チェックに使用した時間: " + m_StopWatch.ElapsedMilliseconds + "マイクロ秒");
        Debug.Log("チェックに使用した時間: " + m_StopWatch.ElapsedMilliseconds * 0.001f + "秒");
#endif
    }
    #endregion

    #region 自動ノードセットアップ
    /// <summary>
    /// ノードを自動でセットアップする
    /// </summary>
    /// <param name="Master"> ノード群の親オブジェクト</param>
    public void AutoNodeSetUp(Transform Master)
        {
            //ノードをクリア
            m_Node.Clear();
            //ノードを子にしているオブジェクトから、ノードを検出
            foreach (Transform AutoMaster in Master)
            {
                //ノード受け皿作成
                LinkData AutoNode = new LinkData();
                //ノード内のリンクするノードとの相対距離リストを初期化
                AutoNode.m_PointLineData.m_PointNode = new List<float>();
                //ノードの座標(Transform)を登録
                AutoNode.m_Pos = AutoMaster;
                //ノードに追加
                m_Node.Add(AutoNode);
            }
            //相対距離をすべて獲得する
            RelativeDataInput();
        }
#endregion

    #region ルートをラインで表示する
            /// <summary>
            /// ルートをラインで表示する
            /// </summary>
            void OnDrawGizmosSelected()
            {
                //ギズモラインの色を青と設定する
                Gizmos.color = Color.blue;
                //ノードの総数が1以上である
                if (m_Node.Count > 1)
                {
                    //まず主軸となるノードリストから、ノードを取り出す
                    foreach (LinkData MainLD in m_Node)
                    {
                        //現在測定しているノードに繋がるノード数が0以上である
                        if (MainLD.m_PointLineData.m_PointNode.Count > 0)
                        {
                            //総当たりで、繋がるノードを走査する
                            for (int i = 0; i < MainLD.m_PointLineData.m_PointNode.Count; i++)
                            {
                                //ノード間の相対距離が0以上、且つ、無限ではない
                                if (MainLD.m_PointLineData.m_PointNode[i] > 0.0f &&
                                    MainLD.m_PointLineData.m_PointNode[i] < Mathf.Infinity)
                                {
                                    //ノード間に青い線を描画する
                                    Gizmos.DrawLine(
                                        MainLD.m_Pos.position,          //現在測定中の位置
                                        m_Node[i].m_Pos.position);      //測定している位置と繋がるノード位置
                                }
                            }
                        }
                    }
                }
            }
    #endregion

    #region 現在登録されているノードの相対データを算出する
            /// <summary>
            /// 現在登録されているノードの相対データを算出する
            /// </summary>
            public void RelativeDataInput()
            {
                //まず主軸となるノードリストから、ノードを取り出す
                foreach (LinkData MainLD in m_Node)
                {
                    //主軸が持っているリンク先のノードを全て破棄する
                    MainLD.m_PointLineData.m_PointNode.Clear();
                    //ターゲットとなるノードリストから、ノードを取り出す
                    foreach (LinkData TargetLD in m_Node)
                    {
                        //距離データの受け皿を作成
                        float m_Distance = 0.0f;
                        //主軸ノードとターゲットノードが同じではない
                        if(MainLD.m_Pos != TargetLD.m_Pos)
                        {
                            //距離を測定する
                            m_Distance = Vector3.Distance(MainLD.m_Pos.position, TargetLD.m_Pos.position);

                            //ルートをレイキャストでチェックする
                            RaycastHit hit;

    #region これは覚えよう! 指定の向きを獲得するには?
                            //ターゲットオブジェクトの方向を指す
                            Vector3 heading = TargetLD.m_Pos.position - MainLD.m_Pos.position;
                            //ベクトルの大きさ確定
                            float distance = heading.magnitude;
                            // 向き情報を正規化
                            Vector3 forward = heading / distance;
    #endregion

                            //レイを飛ばす方向をセット
                            Ray ray = new Ray(MainLD.m_Pos.position, forward);

                            //レイキャストが物体に当たった?
                            if (Physics.Raycast(ray, out hit, m_Distance))
                            {
                                //接触しているルートを10秒間表示
                                //Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 10.0f, false);
                                //経路は遮断されている
                                m_Distance = Mathf.Infinity;
                            }
                        }
                        //距離測定データを追加する
                        //同じノードはこの時0として登録される
                        MainLD.m_PointLineData.m_PointNode.Add(m_Distance);
                    }
                }
            }
    #endregion

    #region ノードの中から、スタート、エンドのidを検索
            /// <summary>
            /// ダイクストラノードのスタートとエンドのトランスフォームから
            /// Listのナンバーであるスタートポイントとエンドポイントを割り出す
            /// </summary>
            /// <param name="Start">　Transform型のスタート位置</param>
            /// <param name="End">　Transform型のエンド位置</param>
            /// <param name="StartPoint">　ダイクストラノードのスタートID</param>
            /// <param name="EndPoint">　ダイクストラノードのエンドID</param>
            public void DijkstraCheck(Transform Start,Transform End,ref int StartPoint,ref int EndPoint)
            {
                //ノードフルチェック
                for (int i = 0; i < m_Node.Count; i++)
                {
                    //スタートIDを算出する
                    if (m_Node[i].m_Pos == Start)
                    {
                        //スタートポイントとする
                        StartPoint = i;
                    }
                    //エンドIDを算出する
                    if (m_Node[i].m_Pos == End)
                    {
                        //エンドポイントとする
                        EndPoint = i;
                    }
                }
            }
    #endregion

    #region 該当のノード間の相対距離を渡す
            /// <summary>
            /// 該当のノード間の相対距離を渡す
            /// </summary>
            /// <param name="StartPoint">　スタート位置</param>
            /// <param name="TartgetPoint">　ターゲット位置</param>
            /// <returns></returns>
            public float CheckDistance(int StartPoint,int TartgetPoint)
            {
                //相対距離を返す
                return m_Node[StartPoint].m_PointLineData.m_PointNode[TartgetPoint];
            }
    #endregion

    #region 計算データの初期化
            /// <summary>
            /// 計算データの初期化
            /// </summary>
            public void CalculationDataInitialization(int StartPoint)
            {
                //ノード数を獲得
                m_Count = m_Node.Count;

                //出発地から目的地までの最短経路上の地点の地点番号を目的地から出発地の順に設定する1次元配列 
                m_StartEndRoute = new int[m_Count];
                //出発地から目的地までの最短距離に初期値を格納する（変更しなくてよい）
                m_StartEndDistance = Mathf.Infinity;

                //その他使う変数
                m_CheckDistance = new float[m_Count];
                //出発地から各地点までの最短距離を設定する配列
                m_CheckRoute = new int[m_Count];
                //出発地から各地点までの最短距離が確定しているかどうかを識別するための配列
                m_CheckFixed = new bool[m_Count];

                //最短ノードid
                sPoint = -1;
                //最終距離
                NewDistance = 0;

                //全初期化
                for (int i = 0; i < m_Count; i++)
                {
                    //最短経路上の地点の地点番号に初期値を格納する
                    m_StartEndRoute[i] = -1;
                    //出発地から各地点までの最短距離に初期値を格納する
                    m_CheckDistance[i] = Mathf.Infinity;
                    //各地点の最短距離の確定状態に初期値を格納する
                    m_CheckFixed[i] = false;
                }

                //出発地から出発地自体への最短距離に0を設定する
                m_CheckDistance[StartPoint] = 0;
            }
    #endregion

    #region 最短ルートにつながるリンクを作成する
            public void RootLinkTest()
            {
                //繰り返しを実行
                while (true)
                {
                    //カウンター
                    int m_CheckCounter = 0;
                    //未確定の地点を1つ探す
                    while (m_CheckCounter < m_Count)
                    {
                        //チェックが終わっている場合
                        if (m_CheckFixed[m_CheckCounter] == false)
                        {
                            //再内側の繰り返しから抜ける
                            break;
                        }
                        m_CheckCounter = m_CheckCounter + 1;
                    }

                    //出発地から全ての地点までの最短経路が確定していれば
                    if (m_CheckCounter == m_Count)
                    {
                        //最短経路探索処理を抜ける
                        break;
                    }
                    //最短距離がより短いノードを探す
                    for (int i = m_CheckCounter + 1; i < m_Count; i++)
                    {
                        //チェックしていない&相対距離を比較
                        if ((m_CheckFixed[i] == false) &&
                            (m_CheckDistance[i] < m_CheckDistance[m_CheckCounter]))
                        {
                            //比較して、相対距離が短い場合、iを上書き
                            m_CheckCounter = i;
                        }
                    }

                    //最短ルートid(この場合、汎用変数iがそれ)を代入する
                    sPoint = m_CheckCounter;
                    //出発地からの最短距離を確定する
                    m_CheckFixed[sPoint] = true;

                    ///
                    for (int i = 0; i < m_Count; i++)
                    {
                        if (CheckDistance(sPoint, i) > 0 &&
                            (m_CheckFixed[i] == false))
                        {
                            //ルートの総距離を更新(加算)
                            NewDistance = m_CheckDistance[sPoint] + CheckDistance(sPoint, i);
                            if (NewDistance < m_CheckDistance[i])
                            {
                                m_CheckDistance[i] = NewDistance;
                                m_CheckRoute[i] = sPoint;
                            }
                        }
                    }
                }
            }
    #endregion

    #region 最短ルートのノードを設定する
            public int RootLinkTestNodeConfirmed(int StartPoint,int EndPoint)
            {
                m_StartEndDistance = m_CheckDistance[EndPoint];
                int ReCounter = 0;
                int ReEndPoint = EndPoint;
                while (ReEndPoint != StartPoint)
                {
                    m_StartEndRoute[ReCounter] = ReEndPoint;
                    ReEndPoint = m_CheckRoute[ReEndPoint];
                    ReCounter = ReCounter + 1;
                }
                m_StartEndRoute[ReCounter] = StartPoint;

                return ReCounter;
            }
    #endregion

    #region ダイクストラ法を使用した最短ルートを割り出す
            /// <summary>
            /// ダイクストラ法を使用した最短ルートを割り出す
            /// </summary>
            /// <param name="Start"> スタート地点のTransform</param>
            /// <param name="End"> ゴール地点のTransform</param>
            /// <param name="Flag">　ルート再定義フラグ</param>
            /// <returns> 次に進むべき座標(Transform型)</returns>
            public Transform D_System(Transform Start, Transform End,bool Flag = false)
            {
                int StartPoint = 0;
                int EndPoint = 0;

                //ルートを再定義している場合、ルート間の相対距離を割り出す
                //障害物がある場合インフィニティ(∞)扱いにする
                if (Flag)
                {
                    //相対距離をすべて獲得する
                    RelativeDataInput();
                }

                //ダイクストラチェック
                //スタート地点idとゴール地点のidを獲得する
                DijkstraCheck(Start, End, ref StartPoint, ref EndPoint);

                //計算データを初期化する
                CalculationDataInitialization(StartPoint);

                //全てのノードの相対距離から、最短距離を割り出しで連結していく
                RootLinkTest();

                //最短距離割り出しの各通るノードを確定する
                int ReCounter = RootLinkTestNodeConfirmed(StartPoint, EndPoint);

    #region 出力結果
                int No = 0;
                Transform m_ReturnTransform = null;
    #if m_Debug
                    Debug.Log("出発地"+ m_StartEndRoute[ReCounter] + "⇒" + "目的地" + m_StartEndRoute[0] + "への算出です。");
    #endif

                for (int i = ReCounter; i >= 0; i--)
                {
                    if (No == 0)
                    {
    #if m_Debug
                            Debug.Log("貴方がいる場所は" + m_StartEndRoute[i] + "です。");
    #endif

                    }
                    else
                    {
                        if (m_StartEndDistance == Mathf.Infinity)
                        {
    #if m_Debug
                                Debug.Log("ルートが遮断されているか、存在しません。");
    #endif

                            m_ReturnTransform = null;
                        }
                        else
                        {
                            //変数が入っていない場合のみ
                            if (!m_ReturnTransform)
                            {
                                //最初のルートが次へ進むべきルート
                                m_ReturnTransform = m_Node[m_StartEndRoute[i]].m_Pos;
                            }
    #if m_Debug
                                Debug.Log("次のポイントである、第[ " + No + " ]ノードの場所は、" + m_StartEndRoute[i] + "です。");
    #endif
                        }
                    }
                    No++;
                }
    #if m_Debug
                    Debug.Log("出発地から目的地までの最短距離は、 "+ m_StartEndDistance + "メートルです。\n迷わず、逝ってらっしゃい。");
    #endif

    #endregion

                return m_ReturnTransform;
            }
    #endregion

    #region 最終的なルートの順番に置き換える
        /// <summary>
        /// 最終的なルートの順番に置き換える
        /// </summary>
        void RootUpdate()
        {
            //ルート順トランスフォームを初期化
            RootCounterLinePoint.Clear();
            //まずは、逆順ルートから調べる
            foreach (int NoPoint in m_StartEndRoute)
            {
                //無効ポイントNo(-1)は候補から省く
                if (NoPoint != -1)
                {
                    //ルート番号のトランスフォームを登録(この地点では逆順)
                    RootCounterLinePoint.Add(m_Node[NoPoint].m_Pos);
                }
            }
            //逆順なので正しい順番にする(Listの反転を行う)
            RootCounterLinePoint.Reverse();
        }
    #endregion

    #region ラインレンダラーによるデバックレンダラー処理
        /// <summary>
        /// ラインレンダラーによるデバックレンダラー処理
        /// </summary>
        void DebugRootLine()
        {
            if (m_DebugLine)
            {
                //デバック用ラインレンダラーの接続地点を、ルート分だけ確保する
                m_DebugLine.positionCount = RootCounterLinePoint.Count;
                //順番にルートをラインレンダラーの接続地点にセットする
                for (int i = 0; i < RootCounterLinePoint.Count; i++)
                {
                    m_DebugLine.SetPosition(i, RootCounterLinePoint[i].position);
                }
            }
        }
    #endregion

    #region ノードポイントをギズモで明確にする
        void OnDrawGizmos()
        {
            foreach (LinkData PL in m_Node)
            {
                bool m_check = false;
                foreach (float Dummy in PL.m_PointLineData.m_PointNode)
                {
                    if (Dummy < float.PositiveInfinity && Dummy != 0.0f)
                    {
                        m_check = true;
                        break;
                    }
                }
                if (m_check)
                    Gizmos.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
                else
                    Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);

                Gizmos.DrawSphere(PL.m_Pos.position, 1.0f);
            }
        }
    #endregion
}
