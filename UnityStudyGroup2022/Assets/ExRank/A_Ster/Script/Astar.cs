using UnityEngine;
using System.Collections.Generic;

namespace ASterSystem
{
    /// <summary>
    /// A*アルゴリズム
    /// </summary>
    public class AStar
    {
        //ルート遮断すると判断するトライ回数
        public int m_rootBlockMaxTryCounter = 999;
        //ルート検索トライ回数
        public int m_rootBlockTryCounter = 0;
        //フィールドサイズ
        private int m_fieldSize;
        //ノード設定
        private AstarNode[,] m_nodes;
        //オープンノード
        private AstarNode[,] m_openNodes;
        //クローズノード
        private AstarNode[,] m_closedNodes;

        // 斜め移動の場合のコスト(斜め移動を容認できない場合は高くする)
        private float m_diagonalMoveCost;

        // 使用する前に実行して初期化してください
        public void Initialize(int size)
        {
            //外部より、エリアサイズを指定するので、サイズを書き換える
            m_fieldSize = size;
            //ノードをエリアサイズX,Y分用意する(受け皿)
            m_nodes = new AstarNode[m_fieldSize, m_fieldSize];
            //オープンノードをエリアサイズX,Y分用意する(受け皿)
            m_openNodes = new AstarNode[m_fieldSize, m_fieldSize];
            //クローズドノードをエリアサイズX,Y分用意する(受け皿)
            m_closedNodes = new AstarNode[m_fieldSize, m_fieldSize];
            //ダイアグラム移動コストを2と設定する
            //SetDiagonalMoveCost(DiagonalMoveCost/*Mathf.Sqrt(2f)*/);

            //ノード系の初期化
            //総当たりで、ノード、オープンノード、クローズドノートをブランク状態にする。
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    m_nodes[x, y] = AstarNode.CreateBlankNode(new Vector2Int(x, y));
                    m_openNodes[x, y] = AstarNode.CreateBlankNode(new Vector2Int(x, y));
                    m_closedNodes[x, y] = AstarNode.CreateBlankNode(new Vector2Int(x, y));
                }
            }
        }

        /// <summary>
        /// 斜め移動による移動コスト設定
        /// このコスト分が移動コストとして計上される
        /// </summary>
        /// <param name="cost"></param>
        public void SetDiagonalMoveCost(float cost)
        {
            //該当するノードのコストを設定する
            m_diagonalMoveCost = cost;
        }


        /// <summary>
        /// ルート検索開始
        /// </summary>
        public bool SearchRoute(
            Vector2Int startNodeId,         //検索元スタートのノードId
            Vector2Int goalNodeId,          //検索終了(目的地)のノードID2
            List<Vector2Int> routeList)     //ノードリスト
        {
            //ノードを初期化
            ResetNode();
            //検索スタート地点と検索終了地点が同一
            if (startNodeId == goalNodeId)
            {
                //同一上なので検索終了
                Debug.Log($"{startNodeId}/{goalNodeId}/同じ場所なので終了");
                return false;
            }

            // 全ノード更新
            for (int x = 0; x < m_fieldSize; x++)
            {
                for (int y = 0; y < m_fieldSize; y++)
                {
                    //現地点でのゴールノードIDによるチェック
                    m_nodes[x, y].UpdateGoalNodeId(goalNodeId);
                    m_openNodes[x, y].UpdateGoalNodeId(goalNodeId);
                    m_closedNodes[x, y].UpdateGoalNodeId(goalNodeId);
                }
            }

            // スタート地点の初期化
            m_openNodes[startNodeId.x, startNodeId.y] = AstarNode.CreateNode(startNodeId, goalNodeId);
            m_openNodes[startNodeId.x, startNodeId.y].SetFromNodeId(startNodeId);
            m_openNodes[startNodeId.x, startNodeId.y].Add();


            Vector2Int bestScoreNodeId;
            m_rootBlockTryCounter = 0;
            while (true)
            {
                m_rootBlockTryCounter++;
                bestScoreNodeId = GetBestScoreNodeId();

                OpenNode(
                    bestScoreNodeId,
                    goalNodeId
                );

                // ゴールに辿り着いたら終了
                if (bestScoreNodeId == goalNodeId)
                {
                    break;
                }

                if (m_rootBlockTryCounter > m_rootBlockMaxTryCounter)
                {
                    Debug.Log($"◆ルート遮断が発生した。◆\n" +
                        $"トライ回数 : {m_rootBlockTryCounter}回到達、" +
                        $"有効なルート検索が不可能の為、機能を停止します。");
                    return false;
                }
            }
            ///辿った経路は逆順なので、逆転させる
            ResolveRoute(startNodeId, goalNodeId, routeList);
            Debug.Log($"◆ルートが確立しました。◆\n" +
                $"トライ回数 : {m_rootBlockTryCounter}回目で到達");
            return true;
        }

        /// <summary>
        /// ノードを初期化する(リセット)
        /// </summary>
        void ResetNode()
        {
            for (int x = 0; x < m_fieldSize; x++)
            {
                for (int y = 0; y < m_fieldSize; y++)
                {
                    m_nodes[x, y].Clear();
                    m_openNodes[x, y].Clear();
                    m_closedNodes[x, y].Clear();
                }
            }
        }

        // ノードを展開する
        void OpenNode(Vector2Int bestNodeId, Vector2Int goalNodeId)
        {
            // 4方向走査
            for (int dx = -1; dx < 2; dx++)
            {
                for (int dy = -1; dy < 2; dy++)
                {
                    int cx = bestNodeId.x + dx;
                    int cy = bestNodeId.y + dy;
                    //対象の走査範囲内に移動可能箇所があるか?
                    if (CheckOutOfRange(
                        dx,
                        dy,
                        bestNodeId.x,
                        bestNodeId.y) == false)
                    {
                        //以後のすべての処理をスキップする
                        continue;
                    }
                    //対象のブロックがロックされているか?
                    if (m_nodes[cx, cy].m_IsLock)
                    {
                        //以後のすべての処理をスキップする
                        continue;
                    }

                    // 縦横で動く場合はコスト : 1
                    // 斜めに動く場合はコスト : _diagonalMoveCost
                    System.Single addCost = dx * dy == 0 ? 1 : m_diagonalMoveCost;

                    m_nodes[cx, cy].SetMoveCost(
                        m_openNodes[bestNodeId.x,
                        bestNodeId.y].m_MoveCost + addCost);

                    m_nodes[cx, cy].SetFromNodeId(bestNodeId);

                    // ノードのチェック
                    UpdateNodeList(cx, cy, goalNodeId);
                }
            }

            // 展開が終わったノードは closed に追加する
            m_closedNodes[bestNodeId.x, bestNodeId.y] 
                = m_openNodes[bestNodeId.x, bestNodeId.y];

            // closedNodesに追加
            m_closedNodes[bestNodeId.x, bestNodeId.y].Add();

            // openNodesから削除
            m_openNodes[bestNodeId.x, bestNodeId.y].Remove();
        }

        /// <summary>
        /// 走査範囲内チェック
        /// </summary>
        bool CheckOutOfRange(int dx, int dy, int x, int y)
        {
            //幅がない??　走査する気あるの??
            if (dx == 0 && dy == 0)
            {
                //ここで終了
                return false;
            }

            int cx = x + dx;
            int cy = y + dy;

            //cxのみを対象とする
            //以下の条件が1つでもある場合、終了する事
            //1.cxが0以下
            //2.cxがフィールドサイズと同値
            //3.cyが0以下
            //4.cyがフィールドサイズと同値
            if (cx < 0
                || cx == m_fieldSize
                || cy < 0
                || cy == m_fieldSize
            )
            {
                //失敗終了
                return false;
            }
            //成功終了
            return true;
        }

        /// <summary>
        /// ノードリストの更新
        /// オープンノードとクローズドノードは対比関係にある事を注意!!
        /// </summary>
        void UpdateNodeList(int x, int y, Vector2Int goalNodeId)
        {
            //オープンノード[x,y]地点がアクティブ状態にある
            if (m_openNodes[x, y].m_IsActive)
            {
                // より優秀なスコアであるならMoveCostとfromを更新する
                if (m_openNodes[x, y].GetScore() > m_nodes[x, y].GetScore())
                {
                    // Node情報の更新を行う
                    m_openNodes[x, y].SetMoveCost(m_nodes[x, y].m_MoveCost);
                    m_openNodes[x, y].SetFromNodeId(m_nodes[x, y].m_FromNodeId);
                }
            }
            //クローズドノード[x,y]地点がアクティブ状態にある
            else if (m_closedNodes[x, y].m_IsActive)
            {
                // より優秀なスコアであるなら closedNodesから除外しopenNodesに追加する
                if (m_closedNodes[x, y].GetScore() > m_nodes[x, y].GetScore())
                {
                    //該当するクローズドノードを終了する
                    m_closedNodes[x, y].Remove();
                    //同一地点のオープンノードを追加
                    m_openNodes[x, y].Add();
                    //Node情報の更新を行う
                    m_openNodes[x, y].SetMoveCost(m_nodes[x, y].m_MoveCost);
                    m_openNodes[x, y].SetFromNodeId(m_nodes[x, y].m_FromNodeId);
                }
            }
            else
            {
                //オープン・クローズド両方がない場合は、新しく、オープンノードを開設する
                m_openNodes[x, y] = new AstarNode(new Vector2Int(x, y), goalNodeId);
                //Node情報の更新を行う(逆順式)
                m_openNodes[x, y].SetFromNodeId(m_nodes[x, y].m_FromNodeId);
                m_openNodes[x, y].SetMoveCost(m_nodes[x, y].m_MoveCost);
                //同一地点のオープンノードを追加
                m_openNodes[x, y].Add();
            }
        }

        void ResolveRoute(Vector2Int startNodeId, Vector2Int goalNodeId, List<Vector2Int> result)
        {
            //リザルドではない
            if (result == null)
            {
                // 本来はGCを発生させないために生成済みのリストを渡す
                result = new List<Vector2Int>();
            }
            else
            {
                //リザルドをクリアする
                result.Clear();
            }

            var node = m_closedNodes[goalNodeId.x, goalNodeId.y];
            result.Add(goalNodeId);

            int cnt = 0;
            // 捜査トライ回数を1000と決め打ち(無限ループ対応)
            int tryCount = 1000;
            //結果フラグ、デフォルトでFalse
            bool isSuccess = false;
            //whileでトライ回数分、ぶん回す!!
            while (cnt++ < tryCount)
            {
                Vector2Int beforeNode = result[0];
                if (beforeNode == node.m_FromNodeId)
                {
                    // 同じポジションなので終了
                    Debug.LogError("同じポジションなので終了失敗" + beforeNode + " / " + node.m_FromNodeId + " / " + goalNodeId);
                    break;
                }

                if (node.m_FromNodeId == startNodeId)
                {
                    isSuccess = true;
                    break;
                }
                else
                {
                    // 開始座標は結果リストには追加しない
                    result.Insert(0, node.m_FromNodeId);
                }

                node = m_closedNodes[node.m_FromNodeId.x, node.m_FromNodeId.y];
            }

            if (isSuccess == false)
            {
                Debug.LogError("失敗" + startNodeId + " / " + node.m_FromNodeId);
            }
        }

        /// <summary>
        /// 最良のノードIDを返却
        /// </summary>
        Vector2Int GetBestScoreNodeId()
        {
            Vector2Int result = new Vector2Int(0, 0);
            double min = double.MaxValue;
            for (int x = 0; x < m_fieldSize; x++)
            {
                for (int y = 0; y < m_fieldSize; y++)
                {
                    if (m_openNodes[x, y].m_IsActive == false)
                    {
                        continue;
                    }

                    if (min > m_openNodes[x, y].GetScore())
                    {
                        // 優秀なコストの更新(値が低いほど優秀)
                        min = m_openNodes[x, y].GetScore();
                        result = m_openNodes[x, y].m_NodeId;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// ノードのロックフラグを変更
        /// </summary>
        public void SetLock(Vector2Int lockNodeId, bool isLock)
        {
            m_nodes[lockNodeId.x, lockNodeId.y].SetIsLock(isLock);
        }
    }

}