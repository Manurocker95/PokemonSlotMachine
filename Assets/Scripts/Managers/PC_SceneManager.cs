/*===============================================================*
 *                                                               *
 *       Script made by Manuel Rodríguez Matesanz                *
 *          Free to use if credits are given                     *
 *                                                               *
 *===============================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PokemonCasino.Localization;
using PokemonCasino.Setup;

namespace PokemonCasino
{
    /// <summary>
    /// Manager that controls scene changing with loading screen
    /// To change the scene just call: 
    /// UGT_SceneManager.LoadScene(index, delayAfterLoading, Event);
    /// </summary>

    public class PC_SceneManager : PC_SingletonMonobehaviour<PC_SceneManager>
    {
        /// <summary>
        /// Loading Screen group. 
        /// </summary>
        [SerializeField] GameObject m_loadingScreenGroup;
        /// <summary>
        /// Text shows "loading..." with an animation
        /// </summary>
        [SerializeField] Text m_loadingText;
        /// <summary>
        /// Load Animator
        /// </summary>
        [SerializeField] Animator m_loadingAnimator;

        // Use this for initialization
        void Start()
        {
            StartAllListeners();
        }

        /// <summary>
        /// When destroying the object we must stop listening events
        /// </summary>
        private void OnDestroy()
        {
            StopAllListeners();
        }
        /// <summary>
        /// Start Listening to events
        /// </summary>
        private void StartAllListeners()
        {
            PC_EventManager.StartListening<int>(PC_EventSetup.Scene.LOAD_SCENE, LoadSceneWithIndex);
            PC_EventManager.StartListening<int, string>(PC_EventSetup.Scene.LOAD_SCENE_WITH_EVENT, LoadSceneAndEvent);
        }
        /// <summary>
        /// Stop Listening to events
        /// </summary>
        private void StopAllListeners()
        {
            //PC_EventManager.StopListening<int>(PC_EventSetup.Scene.LOAD_SCENE, LoadSceneWithIndex);
            //PC_EventManager.StopListening<int, string>(PC_EventSetup.Scene.LOAD_SCENE_WITH_EVENT, LoadSceneAndEvent);
        }
        /// <summary>
        /// Hide Loading panel
        /// </summary>
        private void _HidePanel()
        {
            m_loadingScreenGroup.SetActive(false);
        }
        /// <summary>
        /// method to force the panel hiding
        /// </summary>
        public static void HidePanel()
        {
            Instance._HidePanel();
        }

        /// <summary>
        /// Load Level Async so we can show the 
        /// </summary>
        /// <param name="_scene"></param>
        /// <param name="_delayAfterLoading"></param>
        /// <param name="_eventName"></param>
        private void LoadSceneAsync(int  _scene, float _delayAfterLoading = 1f, string _eventName = "")
        {
            StartCoroutine(LoadingScreen(_scene, _delayAfterLoading, _eventName));
        }
        /// <summary>
        /// Coroutine called for loading the next scene
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_delayAfterLoading"></param>
        /// <param name="_eventName"></param>
        /// <returns></returns>
        IEnumerator LoadingScreen(int _index, float _delayAfterLoading = 1f, string _eventName = "")
        {
            m_loadingScreenGroup.SetActive(true);
            m_loadingText.text = PC_TextManager.GetText(PC_TextSetup.LoadingScreen.LOADING_TEXT);

            if (m_loadingAnimator)
                m_loadingAnimator.SetBool("play", true);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_index);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            yield return new WaitForSeconds(_delayAfterLoading);

            if (m_loadingAnimator)
                m_loadingAnimator.SetBool("play", false);

            m_loadingScreenGroup.SetActive(false);

            if (!string.IsNullOrEmpty(_eventName))
            {
                PC_EventManager.TriggerEvent(_eventName);
            }
        }
        /// <summary>
        /// Method called from event
        /// </summary>
        /// <param name="_index"></param>
        public void LoadSceneWithIndex(int _index)
        {
            LoadSceneAsync(_index);
        }
        /// <summary>
        /// Called from Event
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_eventName"></param>
        public void LoadSceneAndEvent(int _index, string _eventName)
        {
            LoadSceneAsync(_index, 1f, _eventName);
        }
        /// <summary>
        /// UGT_SceneManager.LoadScene 
        /// </summary>
        /// <param name="_sceneIndex"></param>
        /// <param name="_delayAfterLoading"></param>
        /// <param name="_eventName"></param>
        public static void LoadScene(int _sceneIndex, float _delayAfterLoading = 1f, string _eventName = "")
        {
            Instance.LoadSceneAsync(_sceneIndex, _delayAfterLoading, _eventName);
        }
    }
}
