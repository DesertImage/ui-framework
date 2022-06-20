using DesertImage.Extensions;
using UnityEngine;

namespace DesertImage.Audio
{
    public class MusicPlayer : AudioClipsPlayer
    {
        protected override string SourceName { get; } = "MusicSource";

        public override void OnAwake()
        {
            base.OnAwake();

            // GameSettings.MusicVolume.Subscribe(OnMusicVolumeChanged).AddTo(Starter.Instance);
            // GameSettings.MusicEnabled.Subscribe(OnMusicEnabledChanged).AddTo(Starter.Instance);
        }

        public override void Play()
        {
            // if (!GameSettings.MusicEnabled.Value) return;

            base.Play();
        }

        public override void Play(AudioClip clip)
        {
            // if (!GameSettings.MusicEnabled.Value) return;

            base.Play(clip);
        }

        protected override AudioClip GetNextTrack(AudioClip currentTrack)
        {
            return Tracks.GetRandomElement(currentTrack);
        }

        private void OnMusicEnabledChanged(bool value)
        {
            // OnMusicVolumeChanged(GameSettings.MusicVolume.Value);
        }

        private void OnMusicVolumeChanged(float value)
        {
            // SetVolume(GameSettings.MusicEnabled.Value ? GameSettings.MusicVolume.Value : 0f);
        }
    }
}