using System;
using DesertImage.ECS;
using DesertImage.Extensions;
using UnityEngine;

namespace DesertImage.Audio
{
    public class SoundBase : MonoBehaviour, IPoolable
    {
        public Action<SoundBase> OnFinish = delegate { };

        public ushort SoundId;

        public AudioClip Clip => AudioSource.clip;

        public AudioSource AudioSource;

        public float Delay { get; private set; }

        public virtual bool IsSingle { get; set; }

        protected bool IsPlayed;

        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            AudioSource.playOnAwake = false;
        }

        protected virtual void Reset()
        {
            IsPlayed = false;
            IsSingle = false;

            if (AudioSource)
            {
                AudioSource.Stop();
                AudioSource.clip = null;
                AudioSource.pitch = 1f;
            }

            Delay = 0f;
        }

        public void Play(AudioClip audioClip, float volume, bool isLooped = false, float pitch = 1f,
            bool isSingle = false)
        {
            AudioSource.clip = audioClip;
            AudioSource.volume = volume;
            AudioSource.loop = isLooped;
            AudioSource.pitch = pitch;

            IsSingle = isSingle;

            IsPlayed = true;

            Delay = 0f;

            AudioSource.Play();
        }

        public void Play(AudioClip audioClip, float volume, float delay = 0f, bool isLooped = false)
        {
            AudioSource.clip = audioClip;
            AudioSource.volume = volume;
            AudioSource.loop = isLooped;

            AudioSource.playOnAwake = false;

            Delay = delay;

            AudioSource.PlayDelayed(delay);

            this.DoActionWithDelay
            (
                () => { IsPlayed = true; },
                delay
            );
        }

        protected virtual bool IsPlaying()
        {
            return AudioSource.isPlaying;
        }

        protected virtual void Finish()
        {
            OnFinish(this);

            ReturnToPool();
        }

        private void Update()
        {
            if (!IsPlayed) return;

            if (IsPlaying()) return;

            Finish();
        }

        public void OnCreate()
        {
            Reset();
        }

        public void ReturnToPool()
        {
            if (AudioSource.loop)
            {
                OnFinish?.Invoke(this);
            }

            SoundId = 0;

            Core.Instance?.Get<FactorySound>()?.ReturnInstance(this);
        }
    }
}