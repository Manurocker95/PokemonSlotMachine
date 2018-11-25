using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PokemonCasino.SlotMachine
{
    [System.Serializable]
    public class PC_SlotMachineScore
    {
        /// <summary>
        /// Score from Zero to 9
        /// </summary>
        public int m_scoreValue = 0;
        public Dictionary<int, Sprite> m_numberSprites;
        public SpriteRenderer[] m_spriteRenderers;
      
        public PC_SlotMachineScore(PC_SlotMachine _slotMachine)
        {
            Init(0, _slotMachine);
            ResetScore();  
        }

        public PC_SlotMachineScore(int _value, PC_SlotMachine _slotMachine)
        {
            Init(_value, _slotMachine);
        }

        public void Init(int _value, PC_SlotMachine _slotMachine)
        {
            m_numberSprites = _slotMachine.m_numberSprites;
            SetScores(_value);
        }

        public void SetScores(int _value)
        {
            if (_value < 0)
                _value = 0;

            ResetScore();

            m_scoreValue = _value;

            string strScore = _value.ToString();
            int index = m_spriteRenderers.Length - 1;
            for (int i = strScore.Length-1; i >= 0; i--)
            {
                int val = (int)char.GetNumericValue(strScore[i]);
                if (val < 0)
                    val = 0;
                m_spriteRenderers[index].sprite = m_numberSprites[val];
                index--;
            }
        }

        public void ResetScore()
        {
            m_scoreValue = 0;

            foreach (SpriteRenderer r in m_spriteRenderers)
                r.sprite = m_numberSprites[0];
        }

        public void AddScore(int _value)
        {          
            m_scoreValue += _value;

            string strScore = m_scoreValue.ToString();
            int index = m_spriteRenderers.Length - 1;
            for (int i = strScore.Length - 1; i >= 0; i--)
            {
                int val = (int)char.GetNumericValue(strScore[i]);
                if (val < 0)
                    val = 0;

                m_spriteRenderers[index].sprite = m_numberSprites[val];
                index--;
            }
        }
    }

}
