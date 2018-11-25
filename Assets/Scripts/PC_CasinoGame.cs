using PokemonCasino.Setup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokemonCasino
{
    /// <summary>
    /// Parent class with common methods
    /// </summary>
    public class PC_CasinoGame : PC_SingletonMonobehaviour<PC_CasinoGame>
    {
        [Header("GENERAL"), Space(10)]
        [SerializeField] protected DIFFICULTY m_gameDifficulty;
        [SerializeField] protected bool m_gameRunning;
        [SerializeField] protected bool m_gameEnded;
        [SerializeField] protected AudioClip m_bgmClip;

        protected override void Awake()
        {
            m_destroyOnLoad = true;
            base.Awake();
        }

        // Use this for initialization
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        protected virtual void EndGame()
        {
            
        }
        /// <summary>
        /// Initialize common data
        /// </summary>
        /// <param name="_dif">Difficulty.  As it is an enumeration we can cast from int</param>
        public virtual void InitializeGame(int _dif)
        {
            m_gameDifficulty = (DIFFICULTY)_dif;
            PC_AudioManager.PlayBGM(m_bgmClip);
            PC_EventManager.TriggerEvent(PC_EventSetup.LOAD_GAME);
        }

        /// <summary>
        /// Initialize common data
        /// </summary>
        /// <param name="_dif"></param>
        public virtual void InitializeGame(DIFFICULTY _dif)
        {
            m_gameDifficulty = _dif;
            PC_AudioManager.PlayBGM(m_bgmClip);
            PC_EventManager.TriggerEvent(PC_EventSetup.LOAD_GAME);
        }

        public enum DIFFICULTY
        {
            EASY,
            MID,
            HARD
        }
    }

}

