using System;
using System.Collections.Generic;
using System.Linq;
using DesertImage.Audio;
using DesertImage.Extensions;
using DesertImage.External;
using DesertImage.Pools;
using External;
using External.Extensions;
using UnityEngine;

namespace DesertImage
{
    public class FactorySound : Factory, IAwake, ISoundLayerManager
    {
        private const string PrefabName = "SoundEffect";

        private const int MaxDuplicateSoundsCount = 70;

        public readonly List<SoundNode> Nodes = new List<SoundNode>();

        private PoolSoundBase _pool;
        private PoolSoundSequence _sequencePool;

        private GameObject _objPrefab;

        private Transform _soundsTransform;

        private CustomDictionary<ushort, ISoundLayer> SoundLayers =
            new CustomDictionary<ushort, ISoundLayer>(3, 3, 65000);

        public Dictionary<int, CustomList<SoundBase>> PlayingSounds = new Dictionary<int, CustomList<SoundBase>>();

        public void OnAwake()
        {
            var poolTransform = new GameObject("SoundPool").transform;

            _pool = new PoolSoundBase(poolTransform);
            _pool.Register(50);

            _sequencePool = new PoolSoundSequence(poolTransform);
            _sequencePool.Register(2);

            Register(0, 10, .9f);
            Register(1, 1);

            _soundsTransform = new GameObject("Sounds").transform;
        }

        public SoundBase Spawn2D(ushort id, float volume = 1f, bool isLooped = false, float pitch = 1f,
            bool forceStop = false, bool isSingle = false)
        {
            return Spawn2D(id, 0, volume, isLooped, pitch, forceStop, isSingle);
        }

        public SoundBase Spawn2D(ushort id, ushort layerId, float volume = 1f, bool isLooped = false, float pitch = 1f,
            bool forceStop = false, bool isSingle = false)
        {
            foreach (var node in Nodes)
            {
                if (node.Id != id) continue;

                return Spawn2D(node.SoundClip, layerId, volume, isLooped, pitch, forceStop, isSingle);
            }

            return null;
        }

        public SoundBase Spawn2D(ushort[] ids, ushort layerId, float volume = 1f, float pitch = 1f)
        {
            return !SoundLayers.TryGetValue(layerId, out var layer) ? default : layer.Play(ids, volume, pitch);
        }

        public SoundBase Spawn2D(AudioClip audioClip, ushort layerId, float volume = 1f, bool isLooped = false,
            float pitch = 1f,
            bool forceStop = false, bool isSingle = false)
        {
            if (_pool == null) return null;

            if (SoundLayers.TryGetValue(layerId, out var layer))
            {
                return layer.Play(audioClip, volume, pitch, isLooped);
            }

            var soundId = audioClip.GetInstanceID();

            if (forceStop)
            {
                foreach (var obj in PlayingSounds.Values)
                {
                    obj[0]?.OnCreate();
                    ReturnInstance(obj[0]);
                }
            }

            if (PlayingSounds.TryGetValue(soundId, out var instances))
            {
                if (instances.Count >= MaxDuplicateSoundsCount)
                {
                    ReturnInstance(instances[0]);
                }
            }

            var soundBase = _pool.GetInstance();

            soundBase.transform.parent = _soundsTransform;

            soundBase.Play(audioClip, volume, isLooped, pitch, isSingle);

            if (PlayingSounds.TryGetValue(soundId, out instances))
            {
                instances.Add(soundBase);
            }
            else
            {
                PlayingSounds.Add(soundId, new CustomList<SoundBase>(MaxDuplicateSoundsCount) { soundBase });
            }

            return soundBase;
        }

        public AudioClip GetTrack(ushort id)
        {
            AudioClip track = null;

            foreach (var soundNode in Nodes)
            {
                if (soundNode.Id != id) continue;

                track = soundNode.SoundClip;

                break;
            }

            return track;
        }

        public void Register(SoundNode node)
        {
            Nodes.Add(node);
        }

        public void Register(ushort id, AudioClip audioClip, int preRegisterCount = 0)
        {
            Nodes.Add(new SoundNode
            {
                Id = id,
                SoundClip = audioClip,
                RegisterCount = preRegisterCount
            });
        }

        public ISoundLayer GetLayer(ushort id)
        {
            SoundLayers.TryGetValue(id, out var layer);

            return layer;
        }

        public void Register(ushort id, int simultaneousSoundsCount = 3, float volume = 1f)
        {
            SoundLayers.AddOrSetValue(id,
                new SoundLayer(id, _pool, _sequencePool, Nodes, simultaneousSoundsCount, volume));
        }

        public AudioSource GetAudioSource()
        {
            return _pool.GetInstance().AudioSource;
        }

        public SoundBase GetReadySoundBase(ushort soundId)
        {
            var soundBase = _pool.GetInstance();

            soundBase.transform.parent = _soundsTransform;

            var node = Nodes.FirstOrDefault(x => x.Id == soundId);

            soundBase.AudioSource.clip = node?.SoundClip;

            return soundBase;
        }

        public float GetTrackLength(ushort id)
        {
            var node = Nodes.FirstOrDefault(x => x.Id == id);

            if (node == null) return -1f;

            return node.SoundClip ? node.SoundClip.length : -1f;
        }

        public float GetTrackLength(ushort[] ids)
        {
            var node = Nodes.Where(x => ids.Contains(x.Id)).Sum(x => x.SoundClip.length);

            return node;
        }

        public void ReturnInstance(SoundBase soundBase)
        {
            if (!soundBase) return;

            if (soundBase.IsSingle) return;

            var idHash = 0;

            foreach (var soundNode in Nodes)
            {
                if (soundNode.SoundClip != soundBase.Clip) continue;

                idHash = soundNode.SoundClip.GetInstanceID();
            }

            if (PlayingSounds.TryGetValue(idHash, out var instances))
            {
                instances.Remove(soundBase);
            }

            _pool.ReturnInstance(soundBase);
        }
    }

    [Serializable]
    public class SoundNode
    {
        [SerializeField] private string _name;

        public ushort Id;
        public AudioClip SoundClip;

        public int RegisterCount;
    }
}