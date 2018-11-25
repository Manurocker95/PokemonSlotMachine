using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokemonCasino.Save;
using PokemonCasino.Setup;

namespace PokemonCasino
{
    public class PC_Casino : PC_SingletonMonobehaviour<PC_Casino>
    {

        [SerializeField] private int m_coins = 0;
        [SerializeField] private bool m_hasCoinCase = false;

        public int Coins { get { return m_coins; } set { m_coins = value; } }
        public bool HasCoinCase { get { return m_hasCoinCase; } set { m_hasCoinCase = value; } }

        // Use this for initialization
        void Start()
        {
          
        }
        /// <summary>
        /// Set the game data at the beggining
        /// </summary>
        /// <param name="_data"></param>
        public void SetSaveData(PC_SaveData _data)
        {
            m_coins = _data.coins;
            m_hasCoinCase = _data.hasCoinCase;
        }
        /// <summary>
        /// Send this data to the save manager
        /// </summary>
        /// <returns></returns>
        public PC_SaveData GetCommonData()
        {
            return new PC_SaveData(m_hasCoinCase, m_coins);
        }
        /// <summary>
        /// We add coins to the coincase
        /// </summary>
        /// <param name="_value"></param>
        public void AddCoins(int _value)
        {
            int sum = m_coins + _value;
            bool check = false;
            if (sum >= PC_GameSetup.Casino.COIN_LIMIT)
            {
                //Show text msg
                Debug.Log("EXCEED COIN LIMIT");
                check = true;
            }

            m_coins = (check) ? PC_GameSetup.Casino.COIN_LIMIT : sum;
        }
        /// <summary>
        /// When the crupier gives ya the coincase, tha player can actually play
        /// </summary>
        public void ObtainCoinCase()
        {
            m_hasCoinCase = true;
        }

        public void InitData(int _coins = 100, bool _hasCoins = true)
        {
            m_coins = _coins;
            m_hasCoinCase = _hasCoins;
        }
    }
}
