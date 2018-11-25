using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokemonCasino
{
    public class PC_SlotMachineIcon : MonoBehaviour
    {
        public SpriteRenderer m_renderer;
        public Sprite m_sprite;
        public int m_spriteNumber;

        // Use this for initialization
        void Start()
        {
            if (!m_renderer)
                m_renderer = GetComponent<SpriteRenderer>();

           
        }

        public void SetIconData(Sprite _sprite, int _num, float _y)
        {
            m_sprite = _sprite;
           m_spriteNumber = _num;
            m_renderer.sprite = _sprite;
            transform.localPosition = new Vector3(transform.localPosition.x, _y, transform.localPosition.z);
        }

        public void Show(Sprite _sprite, int _num)
        {
            m_sprite = _sprite;
            m_spriteNumber = _num;
            m_renderer.sprite = _sprite;
        }

        public void SetPosY(float _y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, _y, transform.localPosition.z);
        }
    }
}
