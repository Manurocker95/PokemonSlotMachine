using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokemonCasino.Save;

namespace PokemonCasino
{
    public class PC_CasinoMenu : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        private void Update()
        {
            if (PC_InputManager.IsConfirmInputPressedDown())
                SlotMachine();
        }


        public void SlotMachine()
        {
            PC_SceneManager.LoadScene(2, 2f);
        }


        void VoltorbFlip()
        {

        }

        void TilePuzzle()
        {

        }

        void TripleTriad()
        {

        }

        void Mining()
        {

        }

        void Duel()
        {

        }

        void TryLottery()
        {

        }
    }

}
