/*===============================================================*
 *                                                               *
 *       Script made by Manuel Rodríguez Matesanz                *
 *          Free to use if credits are given                     *
 *                                                               *
 *===============================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokemonCasino.Save
{
    /// <summary>
    /// Serialized game data.Loaded when needed
    /// </summary>
    public class PC_SaveData
    {
        /// <summary>
        /// Coins we have in the casino
        /// </summary>
        public int coins = 0;
        /// <summary>
        /// Best score achieve in slot machine game
        /// </summary>
        public int bestScoreSlotMachine = 0;
        public bool hasCoinCase = false;
        /// <summary>
        /// Constructor
        /// </summary>
        public PC_SaveData()
        {
            coins = 0;
            bestScoreSlotMachine = 0;
            hasCoinCase = false;
        }
        /// <summary>
        /// Other consturctor
        /// </summary>
        /// <param name="_hasCoinCase"></param>
        /// <param name="_coins"></param>
        public PC_SaveData(bool _hasCoinCase, int _coins)
        {
            coins = _coins;
            hasCoinCase = _hasCoinCase;
        }
    }

}
