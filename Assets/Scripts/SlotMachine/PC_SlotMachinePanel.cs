using PokemonCasino.Setup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokemonCasino.SlotMachine
{
    public class PC_SlotMachinePanel : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;
        public bool m_lose = false;


        // Use this for initialization
        void Start()
        {
            if (!m_animator)
                m_animator = GetComponent<Animator>();

            PC_EventManager.StartListening(PC_EventSetup.SlotMachine.LOSE, Lose);
            PC_EventManager.StartListening<string>(PC_EventSetup.SlotMachine.PLAY_ANIMATION, PlayAnimation);
        }
        void OnDestroy()
        {
            //PC_EventManager.StopListening(PC_EventSetup.SlotMachine.LOSE, Lose);
            //PC_EventManager.StopListening<string>(PC_EventSetup.SlotMachine.PLAY_ANIMATION, PlayAnimation);
        }

        /// <summary>
        /// Play Animation
        /// </summary>
        /// <param name="_anim"></param>
        public void PlayAnimation(string _anim)
        {
            m_animator.SetTrigger(_anim);
        }

        void CanInsertAgain()
        {
            m_lose = false;
            PC_EventManager.TriggerEvent(PC_EventSetup.SlotMachine.CAN_INSERT);
        }

        public void Lose()
        {
            m_lose = true;
            m_animator.SetTrigger(PC_GameSetup.SlotMachine.LOSE);
        }
    }

}
