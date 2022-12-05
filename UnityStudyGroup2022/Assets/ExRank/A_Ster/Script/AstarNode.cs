using UnityEngine;

namespace ASterSystem
{
    /// <summary>
    /// Astarで使用するノードデータ
    /// </summary>
    public struct AstarNode
    {
        /// <summary>
        /// ノードのポジション
        /// </summary>
        internal Vector2Int m_NodeId { get; }

        /// <summary>
        /// このノードにたどり着く前のノードポジション
        /// </summary>
        internal Vector2Int m_FromNodeId { get; private set; }

        /// <summary>
        /// 経路として使用できないフラグ
        /// </summary>
        internal bool m_IsLock { get; private set; }

        /// <summary>
        /// ノードの有無
        /// </summary>
        internal bool m_IsActive { get; private set; }

        /// <summary>
        /// 必要コスト
        /// </summary>
        internal double m_MoveCost { get; private set; }

        /// <summary>
        /// ヒューリスティックなコスト
        /// </summary>
        private double m_heuristicCost;

        /// <summary>
        /// 空のノードの生成
        /// </summary>
        internal static AstarNode CreateBlankNode(Vector2Int position)
        {
            return new AstarNode(position, new Vector2Int(-1, -1));
        }

        /// <summary>
        /// ノード生成
        /// </summary>
        internal static AstarNode CreateNode(Vector2Int position, Vector2Int goalPosition)
        {
            return new AstarNode(position, goalPosition);
        }

        /// <summary>
        /// CreateBlankNode,CreateNodeを使用してください
        /// </summary>
        internal AstarNode(Vector2Int nodeId, Vector2Int goalNodeId) : this()
        {
            m_NodeId = nodeId;
            //経路として使用できないフラグをオフ(通行可能)とする
            m_IsLock = false;
            //ノードをなしと処理する
            Remove();
            //移動コストを0とする
            m_MoveCost = 0;
            //コール更新、ヒューリスティックコスト再計算
            UpdateGoalNodeId(goalNodeId);
        }

        /// <summary>
        /// ゴール更新 ヒューリスティックコストの更新
        /// </summary>
        internal void UpdateGoalNodeId(Vector2Int goal)
        {
            // 直線距離をヒューリスティックコストとする
            m_heuristicCost = Mathf.Sqrt(
                Mathf.Pow(goal.x - m_NodeId.x, 2) +
                Mathf.Pow(goal.y - m_NodeId.y, 2)
            );
        }

        /// <summary>
        /// 現在のスコアを返す
        /// 移動コスト+ヒューリスティックコスト
        /// </summary>
        /// <returns></returns>
        internal double GetScore()
        {
            //(移動コスト+ヒューリスティックコスト)して返す
            return m_MoveCost + m_heuristicCost;
        }

        /// <summary>
        /// このノードにたどり着く前のノードをセットする
        /// </summary>
        /// <param name="value">このノードにたどり着く前のノード位置</param>
        internal void SetFromNodeId(Vector2Int value)
        {
            //このノードにたどり着く前のノードポジションを代入
            m_FromNodeId = value;
        }

        /// <summary>
        /// ノードがないと処理する
        /// </summary>
        internal void Remove()
        {
            //ノードがないと判断
            m_IsActive = false;
        }
        /// <summary>
        /// ノードを追加する(ノードあり)
        /// </summary>
        internal void Add()
        {
            //ノードありと判断
            m_IsActive = true;
        }

        /// <summary>
        /// 移動コストをセットする
        /// </summary>
        /// <param name="cost">セットしたいコスト</param>
        internal void SetMoveCost(double cost)
        {
            //移動コストをセットする
            m_MoveCost = cost;
        }

        /// <summary>
        /// 経路として使用できないフラグをセットする
        /// </summary>
        /// <param name="isLock">通路封鎖か通行可能化のフラグ</param>
        internal void SetIsLock(bool isLock)
        {
            //経路として使用できないフラグをセットする
            m_IsLock = isLock;
        }

        /// <summary>
        /// クリアし初期化する
        /// </summary>
        internal void Clear()
        {
            //ノードをオフにする
            Remove();
            //移動コストを0にする
            m_MoveCost = 0;
            //ゴールノード初期化
            UpdateGoalNodeId(new Vector2Int(-1, -1));
        }
    }
}