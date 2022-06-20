using System.Collections.Generic;
using UnityEngine;

namespace DesertImage.Audio
{
    public class SoundSequence : SoundBase
    {
        private readonly Queue<AudioClip> _soundsQueue = new Queue<AudioClip>();

        private float _volume = 1f;

        public void Play(AudioClip[] audioClips, float volume = 1f, bool isLooped = false, float pitch = 1f,
            bool isSingle = false)
        {
            _volume = volume;

            foreach (var audioClip in audioClips)
            {
                _soundsQueue.Enqueue(audioClip);
            }

            AudioSource.volume = volume;
            AudioSource.pitch = pitch;

            PlayNext(volume);
        }

        protected override void Finish()
        {
            if (PlayNext(_volume)) return;

            base.Finish();
        }

        private bool PlayNext(float volume = 1f)
        {
            if (_soundsQueue.Count == 0)
            {
                return false;
            }

            var audioClip = _soundsQueue.Dequeue();

            Play(audioClip, volume, false);

            return true;
        }

        protected override void Reset()
        {
            base.Reset();

            _soundsQueue.Clear();
        }
    }
}