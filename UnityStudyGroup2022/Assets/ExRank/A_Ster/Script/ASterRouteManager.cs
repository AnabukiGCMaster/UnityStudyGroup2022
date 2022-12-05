using System.Collections;
using System.Collections.Generic;
using ASterSystem;
using UnityEngine;

public class ASterRouteManager : MonoBehaviour
{

    [Header("マス用キューブオブジェクト")]
    public GameObject m_CubeMassObject;
    [Header("マス用キューブブロックオブジェクト")]
    public GameObject m_CubeBlockMassObject;
    [Header("スタートキューブオブジェクト")]
    public GameObject m_StartCubeMassObject;
    [Header("エンドキューブオブジェクト")]
    public GameObject m_EndCubeMassObject;
    [Header("ルートライン")]
    public LineRenderer m_RootLines;
    [Header("侵攻不能位置指定")]
    public List<Vector2Int> m_BlockPoint;
    [Header("ASterスクリプトを導入する")]
    public AStar m_ASter;
    [Header("ASter範囲、マス目上として計算する。[幅、奥行は同値とする]")]
    public int WH_Size = 10;
    [Header("スタートのマス目")]
    public Vector2Int StartPoint = new Vector2Int(0, 0);
    [Header("ゴールのマス目")]
    public Vector2Int GoalPoint = new Vector2Int(9, 9);
    [Header("ルートデータ")]
    public List<Vector2Int> RouteData;
    [Header("デバック用ランダムブロック数")]
    public int DebugRandomBlockCounter = 1;
    [Header("斜め移動コスト[初期10]")]
    public int DiagonalMoveCost = 10;
    // Start is called before the first frame update

    void Start()
    {
        //Asterプログラム実行
        MasterRun();
    }
    private void Update()
    {
        //左クリックするとすべて初期化してASter再設定
        if (Input.GetMouseButtonDown(0))
        {
            ReSeter();
        }
    }

    #region マス・スタート・ゴールの描画セット
    /// <summary>
    /// マス・スタート・ゴールの描画セット
    /// </summary>
    void SetUpData()
    {
        //マスをセットする
        CubeMassSet();
        //スタート位置セット
        if (m_StartCubeMassObject)
        {
            //スタート座標と、スタートオブジェクトをセットする
            StartGoalPointSet(m_StartCubeMassObject, StartPoint);
        }
        //ゴール位置セット
        if (m_EndCubeMassObject)
        {
            //ゴール座標と、ゴールオブジェクトをセットする
            StartGoalPointSet(m_EndCubeMassObject, GoalPoint);
        }
    }
    #endregion

    #region マスを敷き詰める
    /// <summary>
    /// マスをセットする
    /// </summary>
    void CubeMassSet()
    {
        if (m_CubeMassObject && m_CubeBlockMassObject)
        {
            for (int x = 0; x < WH_Size; x++)
            {
                for (int z = 0; z < WH_Size; z++)
                {
                    bool flag = false;
                    foreach (Vector2 Blocker in m_BlockPoint)
                    {
                        if (Blocker.x == x && Blocker.y == z)
                        {
                            flag = true;
                        }
                    }
                    GameObject Dummy = null;
                    if (flag)
                    {
                        Dummy =
                        Instantiate(
                            m_CubeBlockMassObject,
                            new Vector3(
                            this.transform.position.x + (x * 1.0f),
                            0,
                            this.transform.position.x + (z * 1.0f)),
                            Quaternion.identity);
                    }
                    else
                    {
                        Dummy =
                        Instantiate(
                            m_CubeMassObject,
                            new Vector3(
                            this.transform.position.x + (x * 1.0f),
                            0,
                            this.transform.position.x + (z * 1.0f)),
                            Quaternion.identity);
                    }
                    Dummy.transform.parent = this.transform;
                }
            }
        }
    }
    #endregion

    #region スタート・ゴールマーカーセット
    /// <summary>
    /// スタート・ゴールマーカーセット
    /// </summary>
    /// <param name="SetObject"></param>
    /// <param name="POS"></param>
    void StartGoalPointSet(GameObject SetObject ,Vector2Int POS)
    {
        GameObject Dummy = null;
        Dummy = 
        Instantiate(
            SetObject,
            new Vector3(
            this.transform.position.x + (POS.x * 1.0f),
            0,
            this.transform.position.x + (POS.y * 1.0f)),
            Quaternion.identity);
        Dummy.transform.parent = this.transform;
    }
    #endregion

    #region ルートラインレンダラー表示
    /// <summary>
    /// ルートライン表示
    /// </summary>
    /// <param name="Root"></param>
    void RootLines(List<Vector2Int> Root)
    {
        //ラインレンダラーのライン数を初期化(リスト初期化)
        m_RootLines.positionCount = 0;

        //ラインレンダラーが存在している
        if (m_RootLines)
        {
            //ラインレンダラー処理位置ポイント(リスト現在位置)
            int Point = 0;
            //ラインレンダラーのリスト数をRootカウント数にする
            m_RootLines.positionCount = Root.Count;
            //スタートポイントの座標をセット
            Vector3 SetRootPoint = new Vector3(
                StartPoint.x,
                0.3f,
                StartPoint.y);
            //ラインレンダラーに登録
            m_RootLines.SetPosition(Point, SetRootPoint);
            //Rootカウンター分繰り返す
            for (int RootCounter = 1; RootCounter < Root.Count; RootCounter++)
            {
                //Rootポイントの座標をセット
                SetRootPoint = new Vector3(
                    Root[RootCounter].x,
                    0.3f,
                    Root[RootCounter].y
                    );
                //ラインレンダラーに登録
                m_RootLines.SetPosition(RootCounter, SetRootPoint);
            }
        }
    }
    #endregion

    #region ランダムに障害物を置く
    /// <summary>
    /// ランダムに障害物を置く
    /// </summary>
    void RandomBlockers()
    {
        //Aster.csへ障害物情報を初期化する
        m_BlockPoint.Clear();

        int Counter = 0;
        //障害物の個数分繰り返す
        while (Counter < DebugRandomBlockCounter)
        {
            Counter++;
            //ランダムで障害物を設置
            int X = (int)Random.Range(0, WH_Size);
            int Y = (int)Random.Range(0, WH_Size);
            //スタート・ゴールが潰されないようにする
            if(StartPoint != new Vector2Int(X, Y) && GoalPoint != new Vector2Int(X, Y))
            {
                //ASter.csに障害物情報を送る
                m_BlockPoint.Add(new Vector2Int(X, Y));
            }
        }
    }
    #endregion

    #region ASterを実行
    /// <summary>
    /// ASter実行
    /// </summary>
    void MasterRun()
    {
        //テスト(ブロックランダム配置)
        RandomBlockers();
        //マス・スタート・ゴールセットアップ
        SetUpData();

        //システムストップウォッチを作成し、計測準備
        System.Diagnostics.Stopwatch StopWatch = new System.Diagnostics.Stopwatch();

        //時間計測開始
        StopWatch.Start();

        //A*生成
        m_ASter = new AStar();

        //A*初期化(サイズ代入)
        m_ASter.Initialize(WH_Size);

        //斜め移動のコストをセット(斜めを許可するなら、数値を低くする)
        m_ASter.SetDiagonalMoveCost(DiagonalMoveCost);

        //以後移動不可指定
        foreach (Vector2Int Blocks in m_BlockPoint)
        {
            //ランダムブロックの位置は移動不可能とする(ノート生成しない)
            m_ASter.SetLock(Blocks, true);
        }

        //ルートデータを初期化
        RouteData = new List<Vector2Int>();
        RouteData.Clear();

        //A*測定実行【実はこれだけでASterが実行される】
        //代入対象は、スタート地点、ゴール地点、ルートデータ(初期データ)
        if (m_ASter.SearchRoute(StartPoint, GoalPoint, RouteData))
        {
            Debug.Log("◆処理が正常に実行されました◆");
        }
        else
        {
            Debug.Log("◆処理に異常があります◆\n" +
                "可能性:<<スタート・エンドが同一>>もしくは、<<ルートが遮断されている>>");
        }

        //時間測定終了
        StopWatch.Stop();

        //想定結果をログで表示
        Debug.Log($"ルート走査終了時間は、{StopWatch.ElapsedMilliseconds}㍉秒");

        //ルート表示(ルートデータを基にルート表示)
        RootLines(RouteData);

    }
    #endregion

    #region リスタート
    void ReSeter()
    {
        foreach (Transform Dummy in this.transform)
        {
            Destroy(Dummy.gameObject);
        }
        Debug.ClearDeveloperConsole();
        MasterRun();
    }
    #endregion

}
