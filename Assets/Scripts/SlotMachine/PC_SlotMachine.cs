/*==============================================================================
* "Slot Machine" mini-game 
* By Manuel Rodríguez Matesanz for Virtual Phenix
*-------------------------------------------------------------------------------
* Based on Pokémon Heart Gold / Soul Silver Game and Maruno approximation
*=============================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PokemonCasino;
using PokemonCasino.Setup;
using PokemonCasino.Save;

namespace PokemonCasino.SlotMachine
{
    public class PC_SlotMachine : PC_CasinoGame
    {
        [Header("Slot Machine"), Space(10)]
        [SerializeField] int m_coins = 0;
        /// <summary>
        /// Current wager. Can be 0, 1, 2 or 3 depending on the line
        /// </summary>
        [SerializeField] private int m_wager = 0;
        [SerializeField] private int m_payout = 0;
        [SerializeField] private bool m_replay = false;
        [SerializeField] private PC_SlotMachineReel[] m_reels;
        [SerializeField] private PC_SlotMachineScore m_credit;
        [SerializeField] private PC_SlotMachineScore m_payOutScore;


        [Header("GameObject References "), Space(10)]
        [SerializeField] private GameObject[] m_buttons;
        [SerializeField] private GameObject[] m_lines;
        [SerializeField] private GameObject[] m_lights;

        [Header("Audio References "), Space(10)]
        [SerializeField] private AudioClip m_slotStop;
        [SerializeField] private AudioClip m_slotCoin;
        [SerializeField] private AudioClip m_slotsWin;
        [SerializeField] private AudioClip m_slotsBigWin;
        [SerializeField] private AudioClip m_error;

        [Header("panel"), Space(10)]
        [SerializeField] private PC_SlotMachinePanel m_panel;

        public Dictionary<int, Sprite> m_numberSprites;
        public Dictionary<int, Sprite> m_reelIcons;
        [SerializeField] private bool m_needToPay = false;

        protected override void Awake()
        {
            base.Awake();
            
            m_reelIcons = new Dictionary<int, Sprite>();
            m_numberSprites = new Dictionary<int, Sprite>();

            Sprite[] numSprites = Resources.LoadAll<Sprite>("Graphics/SlotMachine/numbers");

            for (int i = 0; i < numSprites.Length; i++)
                m_numberSprites.Add(i, numSprites[i]);
        }

        public override void InitializeGame(DIFFICULTY _dif)
        {
            base.InitializeGame(_dif);
            m_coins = PC_Casino.Instance.Coins;
            m_wager = 0;
            m_payout = 0;
            m_replay = false;

            if (m_credit == null)
                m_credit = new PC_SlotMachineScore(PC_Casino.Instance.Coins, this);
            else
                m_credit.Init(PC_Casino.Instance.Coins, this);

            if (m_payOutScore == null)
                m_payOutScore = new PC_SlotMachineScore(0, this);
            else
                m_payOutScore.Init(0, this);

            Sprite[] iconsArray = Resources.LoadAll<Sprite>("Graphics/SlotMachine/images");

            foreach (PC_SlotMachineReel r in m_reels)
                r.InitializeReel((int)_dif, this, iconsArray);
        }

        public override void InitializeGame(int _dif)
        {
            base.InitializeGame(_dif);
            m_coins = PC_Casino.Instance.Coins;
            m_wager = 0;
            m_payout = 0;
            m_replay = false;

            if (m_credit == null)
                m_credit = new PC_SlotMachineScore(PC_Casino.Instance.Coins, this);
            else
                m_credit.Init(PC_Casino.Instance.Coins, this);

            if (m_payOutScore == null)
                m_payOutScore = new PC_SlotMachineScore(0, this);
            else
                m_payOutScore.Init(0, this);

            Sprite[] iconsArray = Resources.LoadAll<Sprite>("Graphics/SlotMachine/images");

            foreach (PC_SlotMachineReel r in m_reels)
                r.InitializeReel(_dif, this, iconsArray);
        }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            InitializeGame(DIFFICULTY.EASY);
            PC_EventManager.StartListening(PC_EventSetup.SlotMachine.CAN_INSERT, ResetLines);
            PC_EventManager.StartListening(PC_EventSetup.GAME_LOADED, GameLoaded);

        }

        private void OnDestroy()
        {
            //PC_EventManager.StopListening(PC_EventSetup.SlotMachine.CAN_INSERT, ResetLines);
        }

        void GameLoaded()
        {
            m_coins = PC_Casino.Instance.Coins;
        }

        void ResetLines()
        {
            StopAllCoroutines();
            foreach (GameObject btn in m_buttons)
                btn.SetActive(false);

            foreach (GameObject ln in m_lines)
                ln.SetActive(false);

            m_gameEnded = false;
            m_needToPay = false;
            m_wager = 0;
            m_payout = 0;
            m_panel.m_lose = false;
            m_replay = false;
            m_payOutScore.SetScores(m_payout);
            PC_Casino.Instance.Coins = m_coins;
            PC_EventManager.TriggerEvent(PC_EventSetup.SlotMachine.PLAY_ANIMATION, PC_GameSetup.SlotMachine.INSERT);
        }

        IEnumerator Delay(float time = 2f)
        {
            float timer = 0f;

            while(timer<time)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            ResetLines();
        }


        void GivePayOut()
        {
            if (m_payout <= 0)
            {
                CancelInvoke("GivePayOut");
                StartCoroutine(Delay());

                return;
            }

            --m_payout;
            m_payOutScore.SetScores(m_payout);
            ++m_coins;
            m_credit.SetScores(m_coins);

            if (PC_InputManager.IsConfirmInputPressedDown() || m_credit.m_scoreValue == PC_GameSetup.Casino.COIN_LIMIT)
            {
                m_coins += m_payout;
                m_credit.SetScores(m_coins);

                CancelInvoke("GivePayOut");
                StartCoroutine(Delay());

                return;
            }
            InvokeRepeating("GivePayOut", 0, 1.0f);
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (m_credit.m_scoreValue >= PC_GameSetup.Casino.COIN_LIMIT)
            {
                Debug.LogWarning("You've got 99,999 Coins.");
                EndGame();
            }
            else if (m_coins <= 0)
            {
                Debug.LogError("GAME OVER");
                EndGame();
            }
            else if (m_gameRunning) // Reels are spinning
            {
                bool stop = false;

                if (PC_InputManager.IsConfirmInputPressedDown())
                {
                    PC_AudioManager.PlaySE(m_slotStop);

                    if (m_reels[0].IsSpinning)
                    {
                        m_reels[0].StopSpinning(m_replay);
                        m_buttons[0].SetActive(true);
                    }
                    else if (m_reels[1].IsSpinning)
                    {
                        m_reels[1].StopSpinning(m_replay);
                        m_buttons[1].SetActive(true);
                    }
                    else if (m_reels[2].IsSpinning)
                    {
                        m_reels[2].StopSpinning(m_replay);
                        m_buttons[2].SetActive(true);
                        stop = true;
                    }
                }

                if (stop)
                {
                    m_gameRunning = false;
                    m_gameEnded = true;
                    Payout();
                }
            }
            else if (m_gameEnded && m_panel.m_lose)
            {
                StartCoroutine(Delay(2f));
                if (PC_InputManager.IsConfirmInputPressedDown())
                {
                    ResetLines();
                }
            }
            else if (!m_gameEnded && m_panel.m_lose)
            {
                ResetLines();
                return;
            }
            else if (m_needToPay)
            {
                return;
            }
            else // Awaiting coins for the next spin
            {
                if (m_wager<3 && m_coins > 0 && PC_InputManager.IsWagerInputPressed())
                {
                    PC_AudioManager.PlaySE(m_slotCoin);

                    if (m_wager == 0)
                        PC_EventManager.TriggerEvent(PC_EventSetup.SlotMachine.PLAY_ANIMATION, PC_GameSetup.SlotMachine.PRESS);//m_panel.PlayAnimation(PC_GameSetup.SlotMachine.PRESS);

                    ++m_wager;
                    --m_coins;
                    m_credit.SetScores(m_coins);

                    if (m_wager>=3)
                    {
                        m_lines[4].SetActive(true);
                        m_lines[3].SetActive(true);
                    }
                    else if (m_wager >= 2)
                    {
                        m_lines[2].SetActive(true);
                        m_lines[1].SetActive(true);
                    }
                    if (m_wager >= 1)
                    {
                        m_lines[0].SetActive(true);
                    }
                }
                else if (m_wager >= 3 || (m_wager>0 && m_coins == 0) || (PC_InputManager.IsConfirmInputPressedDown() && m_wager > 0) || m_replay)
                { 
                    if (m_replay)
                    {
                        m_wager = 3;
                        foreach (GameObject ln in m_lines)
                            ln.SetActive(true);
                    }

                    m_gameRunning = true;

                    PC_EventManager.TriggerEvent(PC_EventSetup.SlotMachine.PLAY_ANIMATION, PC_GameSetup.SlotMachine.STOP);
                    //m_panel.PlayAnimation(PC_GameSetup.SlotMachine.STOP);

                    foreach (PC_SlotMachineReel reel in m_reels)
                        reel.StartSpinning();
                }

                if (PC_InputManager.IsCancelInputPressed() && m_wager == 0)
                {
                    //GO OUT
                    Debug.Log("EXIT GAME");
                    GoBackToCasino();
                }

               
            }

            
        }

        void Payout()
        {
            List<int> temp = new List<int>();
            List<int> shownValues= new List<int>();
            foreach (PC_SlotMachineReel r in m_reels)
            {
                temp.Clear();
                temp = r.Show();
                shownValues.Add (temp[0]);
                shownValues.Add (temp[1]);
                shownValues.Add (temp[2]);
            }
           
            int [,] combinations = new int[,]
            {
                { shownValues[1], shownValues[4], shownValues[7] }, // Center row
                { shownValues[0], shownValues[3], shownValues[6] }, // Top row
                { shownValues[2], shownValues[5], shownValues[8] }, // Bottom row
                { shownValues[0], shownValues[4], shownValues[8] }, // Diagonal top left -> bottom right
                { shownValues[2], shownValues[4], shownValues[6] }, // Diagonal bottom left -> top right
             };

            shownValues.Clear();

            m_payout = 0;
            int bonus = 0;
            int length = combinations.GetLength(0);
            bool[] wonRow = new bool[length];
            
            for (int i = 0;i < length; i++)
            {
                // One coin = centre row only and Two coins = three rows only
                if (i >= 1 && m_wager <= 1 || i >= 3 && m_wager <= 2)
                    break;

                wonRow[i] = true;
                
                // Three Magnemites or shellders
                if ((combinations[i, 0] == 1 && combinations[i, 1] == 1 && combinations[i, 2] == 1) || (combinations[i, 0] == 2 && combinations[i, 1] == 2 && combinations[i, 2] == 2))
                {
                    m_payout += 8;
                }
                // Three Pikachus or Three Psyducks
                else if ((combinations[i, 0] == 3 && combinations[i, 1] == 3 && combinations[i, 2] == 3) || (combinations[i, 0] == 4 && combinations[i, 1] == 4 && combinations[i, 2] == 4))
                {
                    m_payout += 15;
                }
                // Three 777 blue or red
                else if ((combinations[i, 0] == 6 && combinations[i, 1] == 6 && combinations[i, 2] == 6) || (combinations[i, 0] == 5 && combinations[i, 1] == 5 && combinations[i, 2] == 5))
                {
                    m_payout += 300;

                    if (bonus<2)
                        bonus = 2;
                }
                // 777, red red blue combinations
                else if ((combinations[i, 0] == 5 && combinations[i, 1] == 5 && combinations[i, 2] == 6) || (combinations[i, 0] == 5 && combinations[i, 1] == 6 && combinations[i, 2] == 5)
                    || (combinations[i, 0] == 6 && combinations[i, 1] == 5 && combinations[i, 2] == 5) || (combinations[i, 0] == 6 && combinations[i, 1] == 6 && combinations[i, 2] == 5)
                    || (combinations[i, 0] == 6 && combinations[i, 1] == 5 && combinations[i, 2] == 6) || (combinations[i, 0] == 5 && combinations[i, 1] == 6 && combinations[i, 2] == 6))
                {
                    m_payout += 90;

                    if (bonus < 1)
                        bonus = 1;
                }
                else if ((combinations[i, 0] == 7 && combinations[i, 1] == 7 && combinations[i, 2] == 7))
                {
                    m_replay = true;
                }
                else
                {
                    if (combinations[i, 0] == 0) // Left cherry
                    {
                        if (combinations[i, 1] == 0) // Center cherry
                        {
                            if (combinations[i, 2] == 0) // Right Cherry
                                m_payout += 4;
                            else
                                m_payout += 2;
                        }   
                    }
                    else
                    {
                        wonRow[i] = false;
                    }
                }
            }

          
            m_payOutScore.SetScores(m_payout);

            if (m_payout>0 || m_replay)
            {
                if (bonus > 0)
                    PC_AudioManager.PlayME(m_slotsBigWin);
                else
                    PC_AudioManager.PlayME(m_slotsWin);

                if (bonus > 0)
                    PC_EventManager.TriggerEvent(PC_EventSetup.SlotMachine.PLAY_ANIMATION, PC_GameSetup.SlotMachine.BONUS + (bonus - 1));//m_panel.PlayAnimation(PC_GameSetup.SlotMachine.BONUS+(bonus-1));
                else
                    PC_EventManager.TriggerEvent(PC_EventSetup.SlotMachine.PLAY_ANIMATION, PC_GameSetup.SlotMachine.WIN);//m_panel.PlayAnimation(PC_GameSetup.SlotMachine.WIN);

                m_needToPay = true;

                StartCoroutine(DelayPayOut());
            }
            else // LOOOSER
            {
                PC_AudioManager.PlaySE(m_error);
                PC_EventManager.TriggerEvent(PC_EventSetup.SlotMachine.LOSE);        
            }
        }

        IEnumerator DelayPayOut(float time = 2f)
        {
            float timer = 0f;

            while (timer < time)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            GivePayOut();
        }

        protected override void EndGame()
        {
            if (m_coins == 0)
                m_coins = 100;

            PC_Casino.Instance.Coins = m_coins;
            //PC_SceneManager.Instance.LoadSceneAndEvent(1, PC_EventSetup.SAVE_GAME);
        }

        public void GoBackToCasino()
        {
            EndGame();
            Application.Quit();
        }
    }
}
