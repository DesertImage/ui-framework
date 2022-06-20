using System;
using DesertImage.Extensions;
using UnityEngine;

namespace DesertImage.Audio
{
    public class AudioClipsPlayer : IAudioPlayer, IAwake, ITick
    {
        public event Action OnStopPlaying;

        protected virtual string SourceName => "AudioSource";

        protected AudioClip[] Tracks;

        private AudioSource _audioSource;

        private AudioClip _currentTrack;

        private float _volume;

        private float _playTime;

        private bool _isPlaying;

        public virtual void OnAwake()
        {
            _audioSource = new GameObject(SourceName).AddComponent<AudioSource>();
            _audioSource.loop = false;
            _audioSource.volume = _volume;
        }

        public void Tick()
        {
            CountPlayingTime();
        }

        public void SetVolume(float volume)
        {
            _volume = volume;

            if (!_audioSource) return;

            _audioSource.volume = _volume;
        }

        public virtual void Play()
        {
            // if (!_currentTrack)
            // {
            // PlayNext();

            if (!_currentTrack)
            {
                Stop();

                OnStopPlaying?.Invoke();

                return;
            }

            // return;
            // }

            Play(_currentTrack);
        }

        public virtual void Play(AudioClip clip)
        {
            if (!clip) return;

            _isPlaying = true;

            SetTrack(clip);

            _audioSource.Play();
        }

        public void LoadTracks(AudioClip[] tracks)
        {
            Tracks = tracks;

            if (tracks != null && tracks.Length > 0)
            {
                SetTrack(tracks[0]);
            }
        }

        public void PlayNext()
        {
            if (Tracks == null || Tracks.Length == 0) return;

            var nextTrack = GetNextTrack(_currentTrack);

            // if (!nextTrack) return;

            SetTrack(nextTrack);

            Play();
        }

        public void Stop()
        {
            _isPlaying = false;

            _audioSource.clip = null;
            _audioSource.Stop();
        }

        public void Pause()
        {
            _isPlaying = false;

            _audioSource.Pause();
        }

        protected virtual AudioClip GetNextTrack(AudioClip currentTrack)
        {
            return Tracks.GetNextElement(currentTrack);
        }

        private void SetTrack(AudioClip audioClip)
        {
            _playTime = 0f;

            _currentTrack = audioClip;
            _audioSource.clip = _currentTrack;
        }

        private void CountPlayingTime()
        {
            if (!_isPlaying) return;

            _playTime += Time.deltaTime;

            if (_playTime < _currentTrack.length) return;

            PlayNext();
        }
    }
}