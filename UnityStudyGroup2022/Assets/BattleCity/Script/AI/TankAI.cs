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
    public enum TankAIState
    {
        //徘徊モード
        WanderingMode,
        //追跡モード
        ChaserMode,
        //攻撃モード
        AttackMode,
        //自壊モード
        DestroyMode,
        //撤退モード
        EscapeMode,
    }

    /// <summary>
    /// こいつがオーナーです
    /// Unityのゲームオブジェクトを入れるのはこのスクリプトのみ
    /// </summary>
    public class TankAI : StatefulObjectBase<TankAI, TankAIState>
    {
        void Start()
        {
            ///初期化の際に、ステートリストに対して、各ステートプログラムを登録する
            /*
            ///S_TypeA ステートを登録する(ステートリスト0番目)
            stateList.Add(new S_TypeA(this));
            ///S_TypeB ステートを登録する(ステートリスト1番目)
            stateList.Add(new S_TypeB(this));
            ///S_TypeC ステートを登録する(ステートリスト2番目)
            stateList.Add(new S_TypeC(this));
            */

            ///ステートマシーンを自身として設定
            stateMachine = new StateMachine<TankAI>();

            ///初期起動時は、A_Modeから実行する為、ステートをA_Modeに移行させる
            ChangeState(TankAIState.WanderingMode);
        }
    }
}
