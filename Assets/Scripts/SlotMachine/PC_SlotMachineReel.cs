using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokemonCasino.SlotMachine
{
    [System.Serializable]
    public class PC_SlotMachineReel : MonoBehaviour
    {
        [Header("Configuration")]
        /// <summary>
        /// Did we initial
        /// </summary>
        private bool m_initialized = false;
        /// <summary>
        /// Speed for the slot machine to spin. Must be a divisor of 48
        /// </summary>
        [SerializeField] private float m_speed = 0.125f;
        /// <summary>
        /// Size of each PC_SlotMachineIcon (each icon)
        /// </summary>
        [SerializeField] private float m_offset = 1.5f;
        /// <summary>
        /// Spinning at full speed
        /// </summary>
        [SerializeField] private bool m_spinning = false;
        /// <summary>
        /// Spinning but going to stop in a sudden
        /// </summary>
        [SerializeField] private bool m_stopping = false;

        [Header("Icons")]
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private PC_SlotMachineIcon[] m_icons;

        [Header("Position References")]
        /// <summary>
        /// Max height position vector so we can set topPos
        /// </summary>
        [SerializeField] private Vector3 m_topPosition;
        /// <summary>
        /// modified variable for setting icon y position
        /// </summary>
        [SerializeField] private float m_topPos = 0;
        /// <summary>
        /// Offset used to know how many icons displace while spinning
        /// </summary>
        [SerializeField] private float m_topPosMultiplier = 0;

        private int m_index = 0;
        private int m_slipping = 0;
        PC_SlotMachine m_machine;

        int num = 0;
        private List<int> m_reel;
        private int[,] m_iconspool = new int[,] 
        {
            { 0, 0, 1, 1, 2, 2, 3, 4, 4, 4, 5, 5, 6, 6, 7 },                // 0 - Easy
            { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 4, 4, 5, 6, 7 },                // 1 - Medium (default)
            { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 4, 4, 5, 6, 7 }                 // 2 - Hard
         };

        private int[] m_slippingPool = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 3 };
        
        public bool IsSpinning { get { return m_spinning; } }

        public void InitializeReel(int _dif, PC_SlotMachine _machine, Sprite[] iconsArray)
        {
            m_machine = _machine;
            m_slipping = 0;

            m_topPosition = m_icons[0].transform.localPosition;
            m_topPos = m_topPosition.y;

            m_reel = new List<int>();
            int length = m_iconspool.GetLength(1);
            int val;
            for (int i = 0; i < length; i++)
            {
                val = m_iconspool[_dif, i];
                m_reel.Add(val);

                if (!_machine.m_reelIcons.ContainsKey(val))
                {
                    _machine.m_reelIcons.Add(val, iconsArray[val]);
                }
            }

            Shuffle(m_reel);
            m_index = Random.Range(0, m_reel.Count);

            for (int i = 0; i < m_icons.Length; i++)
            {
                int num = m_index - i;
                if (num < 0)
                    num += m_reel.Count;

                float _y = m_topPos - i * m_offset;

                m_icons[i].SetIconData(m_machine.m_reelIcons[m_reel[num]], m_reel[num], _y);
            }

            m_topPosMultiplier = m_offset * 5;
            m_initialized = true;
        }

        public List<int> Show()
        {
            List<int> ret = new List<int>();
            for (int i = 3; i < 6; i++)
            {           
                ret.Add(m_icons[i].m_spriteNumber);
            }
            return ret;
        }

        public void StartSpinning()
        {
            m_spinning = true;
        }

        public void StopSpinning(bool _slipping = true)
        {
            m_stopping = true;
            m_slipping = (_slipping) ? m_slippingPool[Random.Range(0, m_slippingPool.Length - 1)] : 0;

            if (!_slipping)
            {
                m_stopping = false;
                m_spinning = false;

                m_index = Random.Range(0, m_reel.Count);
                for (int i = 0; i < m_icons.Length; i++)
                {
                    int num = m_index - i;
                    if (num < 0)
                        num += m_reel.Count;

                    m_icons[i].Show(m_machine.m_reelIcons[m_reel[num]], m_reel[num]);
                }

                if (m_topPos != m_topPosition.y)
                {
                    m_topPos = m_topPosition.y;
                }
            }
        }

        void Update()
        {
            if (m_initialized)
                UpdateIcons();
        }

        public void UpdateIcons()
        {
            if (m_spinning)
            {
                m_topPos -= m_speed;
                if (m_topPos<0)
                {
                    m_topPos += m_topPosMultiplier;
                    m_index = (m_index + 1) % m_reel.Count;

                    if (m_slipping > 0)
                        --m_slipping;
                }
              
            }

            float _y = 0;
            for (int i = 0; i < m_icons.Length; i++)
            {
                _y = m_topPos - i * m_offset;

                m_icons[i].SetPosY(_y);
            }
        }

        public void Shuffle (List<int> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public enum ICON_POSITION
        {
            TOP = 0,
            MIDDLE = 1,
            BOTTOM = 2
        }
    }

    
}