using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Reflection;

namespace StateMachineAI
{
    /// <summary>
    /// 敵のステートリスト
    /// ここでステートを登録していない場合、
    /// 該当する行動が全くでなきい。
    /// </summary>
    /// 
    public enum AIState_ABType
    {
        //回転ステート
        A_Mode,
        //前進ステート
        B_Mode,
    }

    /// <summary>
    /// こいつがオーナーです
    /// Unityのゲームオブジェクトを入れるのはこのスクリプトのみ
    /// </summary>
    public class AITester 
        : StatefulObjectBase<AITester, AIState_ABType>
    {
        void Start()
        {
            ///初期化の際に、ステートリストに対して、各ステートプログラムを登録する
            ///S_TypeA ステートを登録する(ステートリスト0番目)
            stateList.Add(new S_TypeA(this));
            ///S_TypeB ステートを登録する(ステートリスト1番目)
            stateList.Add(new S_TypeB(this));

            ///ステートマシーンを自身として設定
            stateMachine = new StateMachine<AITester>();

            ///初期起動時は、A_Modeから実行する為、ステートをA_Modeに移行させる
            ChangeState(AIState_ABType.A_Mode);
        }

    }
}
