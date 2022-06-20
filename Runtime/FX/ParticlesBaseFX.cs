using DesertImage;
using UnityEngine;

namespace Framework.FX
{
    public class ParticlesBaseFX : EffectBase, ITick
    {
        [SerializeField] private ParticleSystem[] particleSystems;

        [SerializeField] private bool isLooped;

        private bool _isPlayed;

        private void Start()
        {
            if (particleSystems.Length <= 0) return;

            for (var i = 0; i < particleSystems.Length; i++)
            {
                if (!particleSystems[i].main.loop) continue;

                isLooped = true;

                break;
            }
        }

        public override void Play()
        {
            _isPlayed = true;

            if (particleSystems.Length <= 0) return;

            for (var i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Play();
            }
        }

        public override void Stop()
        {
            if (particleSystems.Length > 0)
            {
                for (var i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].Stop();
                }
            }

            _isPlayed = false;
        }

        public void Pause()
        {
            if (particleSystems.Length <= 0) return;

            for (var i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Pause();
            }
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();

            _isPlayed = false;
        }

        private bool IsPlaying()
        {
            var isPlaying = false;

            if (particleSystems.Length == 0) return false;

            for (var i = 0; i < particleSystems.Length; i++)
            {
                var system = particleSystems[i];

                if (!system.IsAlive()) continue;

                isPlaying = true;

                break;
            }

            return isPlaying;
        }

        private void Update()
        {
            Tick();
        }

        public void Tick()
        {
            if (!_isPlayed) return;

            if (IsPlaying() || isLooped) return;

            ReturnToPool();
        }

        private void OnValidate()
        {
            if (particleSystems is {Length: 0})
            {
                particleSystems = GetComponentsInChildren<ParticleSystem>();
            }
        }

        private void OnDestroy()
        {
            particleSystems = null;
        }
    }
}