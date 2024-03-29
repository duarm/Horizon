﻿using System.Collections;
using System.Collections.Generic;
using Kurenaiz.Management.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace Otker.Audio
{
    public class MusicPlayer : MonoBehaviour, IUpdate
    {
        [SerializeField] AudioClip[] clips;

        [Header ("Music Settings")]
        [SerializeField] AudioClip musicAudioClip;
        [SerializeField] AudioMixerGroup musicOutput;
        [SerializeField] bool musicPlayOnAwake = true;
        [Range (0f, 1f)]
        public float musicVolume = 1f;
        [Header ("Ambient Settings")]
        [SerializeField] AudioClip ambientAudioClip;
        [SerializeField] AudioMixerGroup ambientOutput;
        [SerializeField] bool ambientPlayOnAwake = true;
        [Range (0f, 1f)]
        [SerializeField] float ambientVolume = 1f;

        protected AudioSource m_MusicAudioSource;
        protected AudioSource m_AmbientAudioSource;

        protected bool m_TransferMusicTime, m_TransferAmbientTime;
        protected MusicPlayer m_OldInstanceToDestroy = null;

        //every clip pushed on that stack throught "PushClip" function will play until completed, then pop
        //once that stack is empty, it revert to the musicAudioClip
        protected Stack<AudioClip> m_MusicStack = new Stack<AudioClip> ();

        void Awake ()
        {
            DontDestroyOnLoad (gameObject);

            m_MusicAudioSource = gameObject.AddComponent<AudioSource> ();
            m_MusicAudioSource.clip = musicAudioClip;
            m_MusicAudioSource.outputAudioMixerGroup = musicOutput;
            m_MusicAudioSource.loop = true;
            m_MusicAudioSource.volume = musicVolume;

            if (musicPlayOnAwake)
            {
                m_MusicAudioSource.time = 0f;
                m_MusicAudioSource.Play ();
            }

            m_AmbientAudioSource = gameObject.AddComponent<AudioSource> ();
            m_AmbientAudioSource.clip = ambientAudioClip;
            m_AmbientAudioSource.outputAudioMixerGroup = ambientOutput;
            m_AmbientAudioSource.loop = true;
            m_AmbientAudioSource.volume = ambientVolume;

            if (ambientPlayOnAwake)
            {
                if (clips.Length > 0)
                {
                    for (int i = 0; i < clips.Length; i++)
                    {
                        PushClip (clips[i]);
                    }
                }
                m_AmbientAudioSource.time = 0f;
                m_AmbientAudioSource.Play ();
            }
        }

        private void OnEnable()
        {
            UpdateManager.Subscribe(this);
        }

        private void OnDisable()
        {
            UpdateManager.Unsubscribe(this);
        }

        void IUpdate.MUpdate ()
        {
            if (m_MusicStack.Count > 0)
            {
                //isPlaying will be false once the current clip end up playing
                if (!m_MusicAudioSource.isPlaying)
                {
                    m_MusicStack.Pop ();
                    if (m_MusicStack.Count > 0)
                    {
                        m_MusicAudioSource.clip = m_MusicStack.Peek ();
                        m_MusicAudioSource.Play ();
                    }
                    else
                    { //Back to looping music clip
                        m_MusicAudioSource.clip = musicAudioClip;
                        m_MusicAudioSource.loop = true;
                        m_MusicAudioSource.Play ();
                    }
                }
            }
        }

        public void PushClip (AudioClip clip)
        {
            m_MusicStack.Push (clip);
            m_MusicAudioSource.Stop ();
            m_MusicAudioSource.loop = false;
            m_MusicAudioSource.time = 0;
            m_MusicAudioSource.clip = clip;
            m_MusicAudioSource.Play ();
        }

        public void ChangeMusic (AudioClip clip)
        {
            musicAudioClip = clip;
            m_MusicAudioSource.clip = clip;
        }

        public void ChangeAmbient (AudioClip clip)
        {
            ambientAudioClip = clip;
            m_AmbientAudioSource.clip = clip;
        }

        public void Play ()
        {
            PlayJustAmbient ();
            PlayJustMusic ();
        }

        public void PlayJustMusic ()
        {
            m_MusicAudioSource.time = 0f;
            m_MusicAudioSource.Play ();
        }

        public void PlayJustAmbient ()
        {
            m_AmbientAudioSource.Play ();
        }

        public void Stop ()
        {
            StopJustAmbient ();
            StopJustMusic ();
        }

        public void StopJustMusic ()
        {
            m_MusicAudioSource.Stop ();
        }

        public void StopJustAmbient ()
        {
            m_AmbientAudioSource.Stop ();
        }

        public void Mute ()
        {
            MuteJustAmbient ();
            MuteJustMusic ();
        }

        public void MuteJustMusic ()
        {
            m_MusicAudioSource.volume = 0f;
        }

        public void MuteJustAmbient ()
        {
            m_AmbientAudioSource.volume = 0f;
        }

        public void Unmute ()
        {
            UnmuteJustAmbient ();
            UnmuteJustMustic ();
        }

        public void UnmuteJustMustic ()
        {
            m_MusicAudioSource.volume = musicVolume;
        }

        public void UnmuteJustAmbient ()
        {
            m_AmbientAudioSource.volume = ambientVolume;
        }

        public void Mute (float fadeTime)
        {
            MuteJustAmbient (fadeTime);
            MuteJustMusic (fadeTime);
        }

        public void MuteJustMusic (float fadeTime)
        {
            StartCoroutine (VolumeFade (m_MusicAudioSource, 0f, fadeTime));
        }

        public void MuteJustAmbient (float fadeTime)
        {
            StartCoroutine (VolumeFade (m_AmbientAudioSource, 0f, fadeTime));
        }

        public void Unmute (float fadeTime)
        {
            UnmuteJustAmbient (fadeTime);
            UnmuteJustMusic (fadeTime);
        }

        public void UnmuteJustMusic (float fadeTime)
        {
            StartCoroutine (VolumeFade (m_MusicAudioSource, musicVolume, fadeTime));
        }

        public void UnmuteJustAmbient (float fadeTime)
        {
            StartCoroutine (VolumeFade (m_AmbientAudioSource, ambientVolume, fadeTime));
        }

        protected IEnumerator VolumeFade (AudioSource source, float finalVolume, float fadeTime)
        {
            float volumeDifference = Mathf.Abs (source.volume - finalVolume);
            float inverseFadeTime = 1f / fadeTime;

            while (!Mathf.Approximately (source.volume, finalVolume))
            {
                float delta = Time.deltaTime * volumeDifference * inverseFadeTime;
                source.volume = Mathf.MoveTowards (source.volume, finalVolume, delta);
                yield return null;
            }
            source.volume = finalVolume;
        }
    }
}