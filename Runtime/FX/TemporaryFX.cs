using DesertImage;
using DesertImage.ECS;
using UnityEngine;

namespace Framework.FX
{
    public class TemporaryFX : EffectBase, ITick
    {
        [SerializeField] private float lifetime;

        private float _timeElapsed;

        private bool _isPlayed;


        public override void ReturnToPool()
        {
            _isPlayed = false;

            Stop();

            Core.Instance.Get<FactoryFx>().ReturnInstance(this);
        }

        private bool IsPlaying()
        {
            return _timeElapsed < lifetime;
        }

        private void Update()
        {
            Tick();
        }

        public void Tick()
        {
            _timeElapsed += Time.deltaTime;

            if (!_isPlayed) return;

            if (IsPlaying()) return;

            ReturnToPool();
        }
    }
}