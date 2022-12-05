using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StateMachineAI
{
    /// <summary>
    /// ステートを強制的に変え消える方法
    /// ただし、処理実行中の場合暴走する場合もある
    /// (オーバーライド成功時は問題なく稼働する?)
    /// </summary>
    public class TestChange : MonoBehaviour
    {
        /// <summary>
        /// オーナーとのリンク
        /// </summary>
        public AITester m_AITester;

        void Update()
        {
            //2番目のステートを回転ステートに変える
            if (Input.GetMouseButtonDown(0))
            {
                m_AITester.stateList[1] = new S_TypeA(m_AITester);
                Debug.Log("2番目のステートを回転ステートに切り替えた");
            }

            //2番目のステートを移動ステートに変える
            if (Input.GetMouseButtonDown(1))
            {
                m_AITester.stateList[1] = new S_TypeB(m_AITester);
                Debug.Log("2番目のステートを移動ステートに切り替えた");
            }
        }
    }
}