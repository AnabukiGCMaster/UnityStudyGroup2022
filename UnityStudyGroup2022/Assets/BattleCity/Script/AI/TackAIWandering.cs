using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace StateMachineAI
{
    public class TackAIWandering : State<TankAI>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="owner">オーナー</param>
        public TackAIWandering(TankAI owner) : base(owner) { }


        /// <summary>
        /// このステートが起動した瞬間に実行(Startと同義)
        /// </summary>
        public override void Enter()
        {
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

        }


        /// <summary>
        /// Stayに記述する事をこちらで纏める
        /// Updateにソース書くのはよくない理由
        /// </summary>
        public void BrainCheck()
        {
            //攻撃状態へ
            //owner.ChangeState(TankAIState.AttackMode);
        }

    }
}
