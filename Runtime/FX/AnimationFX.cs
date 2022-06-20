using DesertImage;
using DesertImage.ECS;
using UnityEngine;

namespace Framework.FX
{
    public class AnimationFX : EffectBase, ITick
    {
        [SerializeField] private Animation[] animations;

        [SerializeField] private bool isLooped;

        private bool _isPlayed;

        [ContextMenu("Play")]
        public override void Play()
        {
            _isPlayed = true;

            if (animations.Length <= 0) return;

            for (var i = 0; i < animations.Length; i++)
            {
                animations[i].Play();
            }
        }

        [ContextMenu("Stop")]
        public override void Stop()
        {
            if (animations.Length > 0)
            {
                for (var i = 0; i < animations.Length; i++)
                {
                    animations[i].Stop();
                }
            }

            _isPlayed = false;
        }

        public override void ReturnToPool()
        {
            _isPlayed = false;

            Stop();

            Core.Instance.Get<FactoryFx>().ReturnInstance(this);
        }

        private bool IsPlaying()
        {
            var isPlaying = false;

            if (animations.Length == 0) return false;

            for (var i = 0; i < animations.Length; i++)
            {
                var system = animations[i];

                if (!system.isPlaying) continue;

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
    }
}