using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StateMachineAI
{
    /// <summary>
    /// �X�e�[�g�������I�ɕς���������@
    /// �������A�������s���̏ꍇ�\������ꍇ������
    /// (�I�[�o�[���C�h�������͖��Ȃ��ғ�����?)
    /// </summary>
    public class TestChange : MonoBehaviour
    {
        /// <summary>
        /// �I�[�i�[�Ƃ̃����N
        /// </summary>
        public AITester m_AITester;

        void Update()
        {
            //2�Ԗڂ̃X�e�[�g����]�X�e�[�g�ɕς���
            if (Input.GetMouseButtonDown(0))
            {
                m_AITester.stateList[1] = new S_TypeA(m_AITester);
                Debug.Log("2�Ԗڂ̃X�e�[�g����]�X�e�[�g�ɐ؂�ւ���");
            }

            //2�Ԗڂ̃X�e�[�g���ړ��X�e�[�g�ɕς���
            if (Input.GetMouseButtonDown(1))
            {
                m_AITester.stateList[1] = new S_TypeB(m_AITester);
                Debug.Log("2�Ԗڂ̃X�e�[�g���ړ��X�e�[�g�ɐ؂�ւ���");
            }
        }
    }
}