using System;
using UnityEngine;

namespace DesertImage.Audio
{
    public interface IAudioPlayer
    {
        event Action OnStopPlaying;
        
        void LoadTracks(AudioClip[] tracks);

        void Play();
        void Play(AudioClip track);

        void PlayNext();
        
        void Pause();
        void Stop();

        void SetVolume(float volume = .5f);
    }
}