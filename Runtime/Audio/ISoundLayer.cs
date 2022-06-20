using UnityEngine;
using UnityEngine.Audio;

namespace DesertImage.Audio
{
    public interface ISoundLayer
    {
        ushort Id { get; }

        int SimultaneousSoundsCount { get; }

        float Volume { get; }

        AudioMixer AudioMixer { get; }

        SoundBase Play(ushort soundId, float volume = 1f, float pitch = 1f, bool isLooped = false);
        SoundBase Play(AudioClip audioClip, float volume = 1f, float pitch = 1f, bool isLooped = false);
        
        SoundBase Play(ushort[] soundIds, float volume = 1f, float pitch = 1f, bool isLooped = false);
        SoundBase Play(AudioClip[] audioClips, float volume = 1f, float pitch = 1f, bool isLooped = false);

        void Stop(ushort soundId);
        void StopAll();
    }
}