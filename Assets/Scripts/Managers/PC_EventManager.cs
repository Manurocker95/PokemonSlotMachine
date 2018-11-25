
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PokemonCasino
{
    /// <summary>
    /// * This is a modified version of Unity's Event Tutorial.
    /// * From any script you can call: 
    /// 
    /// PC_EventManager.StartListening(EventName, methodToCallbackWhenTriggered);
    /// 
    /// * to start listening a specific event. Remember to stop listening on destroy so you don't have listener bugs
    /// 
    /// PC_EventManager.StopListening(EventName, sameStartListeningMethod);
    /// 
    /// * And if you want to trigger an event, just call:
    /// 
    /// PC_EventManager.TriggerEvent(EventName);
    /// 
    /// * For easier use, you can store event names in UGT_EventSetup. Then call it from anywhere:
    /// 
    /// PC_EventManager.TriggerEvent(UGT_EventSetup.Localization.TRANSLATE_TEXTS);
    /// </summary>
    public class PC_EventManager : PC_SingletonMonobehaviour<PC_EventManager>
    {
        /// <summary>
        /// Event dictionary
        /// </summary>
        private Dictionary<string, object> eventDictionary;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }
        /// <summary>
        /// Dictionary initialization
        /// </summary>
        void Init()
        {
            eventDictionary = new Dictionary<string, object>();

        }
        /// <summary>
        /// The desired object is now listening if it wasn't
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="listener"></param>
        public static void StartListening(string eventName, UnityAction listener)
        {
            if (Instance == null)
                return;

            UnityEvent thisEvent = null;
            object thisEventObject = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEventObject))
            {
                thisEvent = thisEventObject as UnityEvent;
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }
        /// <summary>
        /// The desired object is no longer listening
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="listener"></param>
        public static void StopListening(string eventName, UnityAction listener)
        {
            if (Instance == null)
                return;

            UnityEvent thisEvent = null;
            object thisEventObject = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEventObject))
            {
                thisEvent = thisEventObject as UnityEvent;
                thisEvent.RemoveListener(listener);
            }
        }
        /// <summary>
        /// Triggers the event and calls every object that is listening to that event
        /// </summary>
        /// <param name="eventName"></param>
        public static void TriggerEvent(string eventName)
        {
            if (Instance == null)
                return;

            UnityEvent thisEvent = null;
            object thisEventObject = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEventObject))
            {
                thisEvent = thisEventObject as UnityEvent;
                thisEvent.Invoke();
            }
        }

        public static void StartListening<T>(string eventName, UnityAction<T> listener)
        {
            if (Instance == null)
                return;

            CustomEvent<T> thisEvent = null;
            object thisEventObject = null;

            if (Instance.eventDictionary.TryGetValue(eventName, out thisEventObject))
            {
                thisEvent = thisEventObject as CustomEvent<T>;
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new CustomEvent<T>();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening<T>(string eventName, UnityAction<T> listener)
        {
            if (Instance == null)
                return;

            CustomEvent<T> thisEvent = null;
            object thisEventObject = null;

            if (Instance.eventDictionary.TryGetValue(eventName, out thisEventObject))
            {
                thisEvent = thisEventObject as CustomEvent<T>;
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent<T>(string eventName, T value)
        {
            if (Instance == null)
                return;

            object eventObject = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out eventObject))
            {
                CustomEvent<T> thisEvent = eventObject as CustomEvent<T>;
                if (thisEvent!=null)
                    thisEvent.Invoke(value);
            }
        }

        public static void StartListening<T0, T1>(string eventName, UnityAction<T0, T1> listener)
        {
            if (Instance == null)
                return;

            CustomEvent<T0, T1> thisEvent = null;
            object thisEventObject = null;

            if (Instance.eventDictionary.TryGetValue(eventName, out thisEventObject))
            {
                thisEvent = thisEventObject as CustomEvent<T0, T1>;
                if (thisEvent == null)
                {
                    thisEvent = new CustomEvent<T0, T1>();
                    thisEvent.AddListener(listener);

                    Instance.eventDictionary[eventName] = thisEvent;
                }
                else
                {
                    thisEvent.AddListener(listener);
                }
            }
            else
            {
                thisEvent = new CustomEvent<T0, T1>();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening<T0, T1>(string eventName, UnityAction<T0, T1> listener)
        {
            if (Instance == null)
                return;

            CustomEvent<T0, T1> thisEvent = null;
            object thisEventObject = null;

            if (Instance.eventDictionary.TryGetValue(eventName, out thisEventObject))
            {
                thisEvent = thisEventObject as CustomEvent<T0, T1>;
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent<T0, T1>(string eventName, T0 value0, T1 value1)
        {
            if (Instance == null)
                return;

            object eventObject = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out eventObject))
            {
                CustomEvent<T0, T1> thisEvent = eventObject as CustomEvent<T0, T1>;
                thisEvent.Invoke(value0, value1);
            }
        }
    }

    [System.Serializable]
    public class CustomEvent<T> : UnityEvent<T>
    {

    }

    [System.Serializable]
    public class CustomEvent<T0, T1> : UnityEvent<T0, T1>
    {

    }
}
