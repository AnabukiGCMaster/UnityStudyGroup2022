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
    /// �G�̃X�e�[�g���X�g
    /// �����ŃX�e�[�g��o�^���Ă��Ȃ��ꍇ�A
    /// �Y������s�����S���łȂ����B
    /// </summary>
    /// 
    public enum TankAIState
    {
        //�p�j���[�h
        WanderingMode,
        //�ǐՃ��[�h
        ChaserMode,
        //�U�����[�h
        AttackMode,
        //���󃂁[�h
        DestroyMode,
        //�P�ރ��[�h
        EscapeMode,
    }

    /// <summary>
    /// �������I�[�i�[�ł�
    /// Unity�̃Q�[���I�u�W�F�N�g������̂͂��̃X�N���v�g�̂�
    /// </summary>
    public class TankAI : StatefulObjectBase<TankAI, TankAIState>
    {
        void Start()
        {
            ///�������̍ۂɁA�X�e�[�g���X�g�ɑ΂��āA�e�X�e�[�g�v���O������o�^����
            /*
            ///S_TypeA �X�e�[�g��o�^����(�X�e�[�g���X�g0�Ԗ�)
            stateList.Add(new S_TypeA(this));
            ///S_TypeB �X�e�[�g��o�^����(�X�e�[�g���X�g1�Ԗ�)
            stateList.Add(new S_TypeB(this));
            ///S_TypeC �X�e�[�g��o�^����(�X�e�[�g���X�g2�Ԗ�)
            stateList.Add(new S_TypeC(this));
            */

            ///�X�e�[�g�}�V�[�������g�Ƃ��Đݒ�
            stateMachine = new StateMachine<TankAI>();

            ///�����N�����́AA_Mode������s����ׁA�X�e�[�g��A_Mode�Ɉڍs������
            ChangeState(TankAIState.WanderingMode);
        }
    }
}
