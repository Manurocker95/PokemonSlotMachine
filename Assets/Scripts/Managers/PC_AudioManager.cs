using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokemonCasino
{
    public class PC_AudioManager : PC_SingletonMonobehaviour<PC_AudioManager>
    {
        
        [SerializeField] private AudioSource m_seSource;
        [SerializeField] private AudioSource m_meSource;
        [SerializeField] private AudioSource m_bgmSource;
        public float m_mainVolume = 1f;

        protected override void Initialize()
        {
            base.Initialize();
            EnsureAudioSources();
        }

        protected override void Awake()
        {
            base.Awake();
            EnsureAudioSources();
        }

        private void EnsureAudioSources()
        {
            if (m_seSource == null)
                m_seSource = gameObject.AddComponent<AudioSource>();
            if (m_meSource == null)
                m_meSource = gameObject.AddComponent<AudioSource>();
            if (m_bgmSource == null)
                m_bgmSource = gameObject.AddComponent<AudioSource>();
        }

        // Use this for initialization
        void Start()
        {
            m_mainVolume = 1f;
        }

        public void _PlayME(AudioClip _audio, float _volume = 1f)
        {
            if (_audio == null || m_meSource == null) return;
            m_meSource.PlayOneShot(_audio, _volume * m_mainVolume);
        }

        public void _PlaySE(AudioClip _audio, float _volume = 1f)
        {
            if (_audio == null || m_seSource == null) return;
            m_seSource.PlayOneShot(_audio, _volume * m_mainVolume);
        }

        public void _PlayBGM(AudioClip _audio, float _volume = 1f, bool _loop = true)
        {
            if (_audio == null || m_bgmSource == null) return;
            m_bgmSource.clip = _audio;
            m_bgmSource.loop = _loop;
            m_bgmSource.volume = _volume * m_mainVolume;
            m_bgmSource.Play();
        }

        public static void PlaySE(AudioClip _audio, float _volume = 1f)
        {
            Instance._PlaySE(_audio, _volume);
        }

        public static void PlayME(AudioClip _audio, float _volume = 1f)
        {
            Instance._PlayME(_audio, _volume);
        }

        public static void PlayBGM(AudioClip _audio, float _volume = 1f, bool _loop = true)
        {
            Instance._PlayBGM(_audio, _volume, _loop);
        }
    }
}
