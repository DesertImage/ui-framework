using DesertImage.Audio;
using UnityEngine;

namespace DesertImage.Pools
{
    public class PoolSoundSequence : SimpleMonoPool<SoundSequence>
    {
        public PoolSoundSequence(Transform parent) : base(parent)
        {
        }

        protected override SoundSequence CreateInstance()
        {
            var newObj = new GameObject("SoundSequence");
            
            newObj.AddComponent<AudioSource>();
            
            return newObj.AddComponent<SoundSequence>();
        }
    }
}