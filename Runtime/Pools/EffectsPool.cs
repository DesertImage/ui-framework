using Framework.FX;
using UnityEngine;

namespace DesertImage.Pools
{
    public class EffectsPool : MonoBehaviourPool<EffectBase>
    {
        public EffectsPool(Transform parent) : base(parent)
        {
        }
        
        protected override int GetId(EffectBase instance)
        {
            return instance && instance.gameObject ? instance.gameObject.GetInstanceID() : -1;
        }

        protected override void GetStuff(EffectBase instance)
        {
            base.GetStuff(instance);
            
            instance.transform.SetParent(null);
            instance.gameObject.SetActive(true);
        }

        protected override void ReturnStuff(EffectBase instance)
        {
            base.ReturnStuff(instance);            
            
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(parent);
        }
    }
}