using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace StateMachineAI
{
    public class S_TypeB : State<AITester>
    {
        /// <summary>
        /// 切り替え時間
        /// </summary>
        float m_Times;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="owner">オーナー</param>
        public S_TypeB(AITester owner) : base(owner) { }


        /// <summary>
        /// このステートが起動した瞬間に実行(Startと同義)
        /// </summary>
        public override void Enter()
        {
            ///切り替え時間を初期化
            m_Times = 0.0f;
            Debug.Log("▼S_TypeBを起動しました!!");
        }


        /// <summary>
        /// このステートが起動中に常に実行(Updateと同義)
        /// </summary>
        public override void Stay()
        {
            ///思考開始
            BrainCheck();
        }


        /// <summary>
        /// このステートが終了した場合
        /// </summary>
        public override void Exit()
        {
            Debug.Log("▼S_TypeBが終了しました!!");
        }


        /// <summary>
        /// Stayに記述する事をこちらで纏める
        /// Updateにソース書くのはよくない理由
        /// </summary>
        public void BrainCheck()
        {
            ///キューブをZ軸移動
            owner.transform.Translate(new Vector3(0, 0, 0.1f));
            ///1秒経ったら...
            if (m_Times > 1.0f)
            {
                ///S_TypeA(A_Mode)へステート移動
                owner.ChangeState(AIState_ABType.A_Mode);
            }
            else
            {
                ///秒間で代入
                m_Times += 1.0f * Time.deltaTime;
            }
        }
    }
}