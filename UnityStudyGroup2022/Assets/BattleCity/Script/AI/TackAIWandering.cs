using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace StateMachineAI
{
    public class TackAIWandering : State<TankAI>
    {
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="owner">�I�[�i�[</param>
        public TackAIWandering(TankAI owner) : base(owner) { }


        /// <summary>
        /// ���̃X�e�[�g���N�������u�ԂɎ��s(Start�Ɠ��`)
        /// </summary>
        public override void Enter()
        {
        }


        /// <summary>
        /// ���̃X�e�[�g���N�����ɏ�Ɏ��s(Update�Ɠ��`)
        /// </summary>
        public override void Stay()
        {
            ///�v�l�J�n
            BrainCheck();
        }


        /// <summary>
        /// ���̃X�e�[�g���I�������ꍇ
        /// </summary>
        public override void Exit()
        {

        }


        /// <summary>
        /// Stay�ɋL�q���鎖��������œZ�߂�
        /// Update�Ƀ\�[�X�����̂͂悭�Ȃ����R
        /// </summary>
        public void BrainCheck()
        {
            //�U����Ԃ�
            //owner.ChangeState(TankAIState.AttackMode);
        }

    }
}
