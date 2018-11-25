
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokemonCasino
{
    /// <summary>
    /// * Instead of mapping the KeyCodes in many scripts, we just ask this script
    /// * to tell if that keyCode is pressed
    /// * Just call:
    /// 
    /// UGT_InputManager.IsConfirmInputPressedDown();
    /// </summary>
    public class PC_InputManager : PC_SingletonMonobehaviour<PC_InputManager>
    {
        [SerializeField] private KeyCode[] m_mainButtonKeyCodes = { KeyCode.Return, KeyCode.Joystick1Button0, KeyCode.Space, KeyCode.C };
        [SerializeField] private KeyCode[] m_wagerKeyCodes = { KeyCode.DownArrow, KeyCode.Joystick1Button2 };
        [SerializeField] private KeyCode[] m_cancelkeyCodes = { KeyCode.Escape, KeyCode.Joystick1Button1 };

        // Use this for initialization
        void Start()
        {

        }
        /// <summary>
        /// Main Button: Interact, confirm...
        /// </summary>
        /// <returns></returns>
        bool _IsConfirmInputPressedDown(bool _down = false)
        {
            if (_down)
            {
                foreach (KeyCode kc in m_mainButtonKeyCodes)
                {
                    if (Input.GetKeyDown(kc))
                        return true;
                }
            }
            else
            {
                foreach (KeyCode kc in m_mainButtonKeyCodes)
                {
                    if (Input.GetKey(kc))
                        return true;
                }
            }
            return false;
        }

        bool _IsWagerInputPressedDown(bool _down = false)
        {
            if (_down)
            {
                foreach (KeyCode kc in m_wagerKeyCodes)
                {
                    if (Input.GetKeyDown(kc))
                        return true;
                }
            }
            else
            {
                foreach (KeyCode kc in m_wagerKeyCodes)
                {
                    if (Input.GetKey(kc))
                        return true;
                }
            }
            return false;
        }

        bool _IsCancelInputPressedDown(bool _down = false)
        {
            if (_down)
            {
                foreach (KeyCode kc in m_cancelkeyCodes)
                {
                    if (Input.GetKeyDown(kc))
                        return true;
                }
            }
            else
            {
                foreach (KeyCode kc in m_cancelkeyCodes)
                {
                    if (Input.GetKey(kc))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Any code
        /// </summary>
        /// <param name="kc"></param>
        /// <returns></returns>
        public static bool IsKeyCodePressed(KeyCode kc)
        {
            return Input.GetKey(kc);
        }

        public static bool IsKeyCodePressedDown(KeyCode kc)
        {
            return Input.GetKeyDown(kc);
        }

        /// <summary>
        /// We just set a confirm keys but any keys can be set
        /// </summary>
        /// <returns></returns>
        public static bool IsConfirmInputPressedDown(bool _down = true)
        {
            return Instance._IsConfirmInputPressedDown(_down);
        }
        public static bool IsWagerInputPressed(bool _down = true)
        {
            return Instance._IsWagerInputPressedDown(_down);
        }
        public static bool IsCancelInputPressed(bool _down = true)
        {
            return Instance._IsCancelInputPressedDown(_down);
        }

    }
}
