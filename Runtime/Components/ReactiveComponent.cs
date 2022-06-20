using System;
using UniRx;

namespace DesertImage.ECS
{
    [Serializable]
    public class ReactiveComponent<T> : DataComponent<T> where T : DataComponent<T>
    {
        public ReactiveProperty<T> Value = new ReactiveProperty<T>();

        private CompositeDisposable _disposable = new CompositeDisposable();

        public override void OnCreate()
        {
            base.OnCreate();

            Value.Subscribe(value => Updated()).AddTo(_disposable);
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();

            _disposable.Clear();
        }
    }
}