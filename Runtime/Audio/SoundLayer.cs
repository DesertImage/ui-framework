using System.Collections.Generic;
using System.Linq;
using DesertImage.Extensions;
using DesertImage.Pools;
using External;
using Framework.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace DesertImage.Audio
{
    public class SoundLayer : ISoundLayer
    {
        public SoundLayer(ushort id, PoolSoundBase pool, PoolSoundSequence sequencePool, List<SoundNode> nodes,
            int simultaneousSoundsCount = 10,
            float volume = 1f,
            AudioMixer audioMixer = null)
        {
            Id = id;
            _pool = pool;
            _sequencePool = sequencePool;
            _nodes = nodes;
            SimultaneousSoundsCount = simultaneousSoundsCount;
            Volume = volume;
            AudioMixer = audioMixer;
        }

        public ushort Id { get; }

        public int SimultaneousSoundsCount { get; }
        public float Volume { get; }
        public AudioMixer AudioMixer { get; }

        private readonly List<SoundNode> _nodes;
        private readonly PoolSoundBase _pool;
        private readonly PoolSoundSequence _sequencePool;

        private readonly LinkedList<SoundBase> _allPlayingSounds = new LinkedList<SoundBase>();

        private readonly CustomDictionary<ushort, List<SoundBase>> _playingSounds =
            new CustomDictionary<ushort, List<SoundBase>>(65000, null);

        public SoundBase Play(ushort soundId, float volume = 1f, float pitch = 1f, bool isLooped = false)
        {
            foreach (var soundNode in _nodes)
            {
                if (soundNode.Id != soundId) continue;

                return Play(soundId, soundNode.SoundClip, volume, pitch, isLooped);
            }

            return null;
        }

        public SoundBase Play(AudioClip audioClip, float volume = 1f, float pitch = 1f, bool isLooped = false)
        {
            var id = _nodes.FirstOrDefault(x => x.SoundClip == audioClip)?.Id ?? default;

            return Play(id, audioClip, volume, pitch, isLooped);
        }

        public SoundBase Play(ushort[] soundIds, float volume = 1, float pitch = 1, bool isLooped = false)
        {
            if (soundIds == null || soundIds.Length == 0) return default;

            var clips = new AudioClip[soundIds.Length];

            var nodesFound = 0;

            for (var i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];

                if (!Enumerable.Contains(soundIds, node.Id) || clips.Contains(node.SoundClip)) continue;

                var index = soundIds.IndexOf(node.Id);

                clips[index] = node.SoundClip;

                nodesFound++;

                if (nodesFound >= clips.Length) break;
            }

            var soundSequence = _sequencePool.GetInstance() as SoundSequence;

            if (soundSequence)
            {
                soundSequence.Play(clips, volume, isLooped, pitch);
            }

            return soundSequence;
        }

        public SoundBase Play(AudioClip[] audioClips, float volume = 1, float pitch = 1, bool isLooped = false)
        {
            var soundSequence = _sequencePool.GetInstance() as SoundSequence;

            if (soundSequence)
            {
                soundSequence.Play(audioClips, volume, isLooped, pitch);
            }

            return soundSequence;
        }

        private SoundBase Play(ushort id, AudioClip audioClip, float volume = 1f, float pitch = 1f,
            bool isLooped = false)
        {
            if (_allPlayingSounds.Count >= SimultaneousSoundsCount)
            {
                StopFirst();
            }

            var soundBase = _pool.GetInstance();

            soundBase.SoundId = id;

            soundBase.OnFinish += OnHandleFinish;

            _allPlayingSounds.AddLast(soundBase);

            if (_playingSounds.TryGetValue(id, out var sounds))
            {
                sounds.Add(soundBase);
            }
            else
            {
                _playingSounds.Add(id, new List<SoundBase> { soundBase });
            }

            soundBase.Play(audioClip, volume * Volume, isLooped, pitch);

            return soundBase;
        }

        public void Stop(ushort soundId)
        {
            if (!_playingSounds.TryGetValue(soundId, out var playingSounds)) return;

            playingSounds.ClearPoolables();
        }

        public void StopAll()
        {
            for (var i = 0; i < _playingSounds.Count; i++)
            {
                StopFirst();
            }

            _playingSounds.Clear();
            _allPlayingSounds.Clear();
        }

        private void StopFirst()
        {
            if (_allPlayingSounds.Count == 0) return;

            var firstSoundBase = _allPlayingSounds.First.Value;

            _allPlayingSounds.RemoveFirst();

            if (!_playingSounds.TryGetValue(firstSoundBase.SoundId, out var sounds)) return;

            if ((sounds?.Count ?? 0) == 0) return;

            sounds[sounds.Count - 1].ReturnToPool();
            sounds.RemoveAt(sounds.Count - 1);
        }

        private void OnHandleFinish(SoundBase soundBase)
        {
            soundBase.OnFinish -= OnHandleFinish;

            if (!_playingSounds.TryGetValue(soundBase.SoundId, out var playingSounds)) return;

            _allPlayingSounds.RemoveIfContains(soundBase);
            playingSounds.RemoveIfContains(soundBase);
        }
    }
}