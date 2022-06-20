using UnityEngine;

namespace DesertImage
{
    public static class ParticleSystemExtensions
    {
        public static void Play(this ParticleSystem[] particleSystems)
        {
            if (particleSystems == null) return;
            
            foreach (var particleSystem in particleSystems)
            {
                particleSystem.Play();
            }
        }
    }
}